using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Maximiz.Core.Querying
{

    /// <summary>
    /// TODO This is unsafe because of object.
    /// TODO This became a mess.
    /// The person activating this object can put anything in the translatefunction
    /// and anything in the values. This means they can pretty much execute any
    /// kind of code in our codebase. I just couldn't make it work with generics.
    /// @Yorick we should look at this.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PropertyEquality<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PropertyEquality() { }

        /// <summary>
        /// Simplified constructor.
        /// </summary>
        public PropertyEquality(Expression<Func<TEntity, object>> property, object value)
            : this(property, new List<object> { value }, AsString) { }

        /// <summary>
        /// Simplified constructor.
        /// </summary>
        public PropertyEquality(Expression<Func<TEntity, object>> property, IEnumerable<object> values)
            : this(property, values, AsString) { }

        /// <summary>
        /// Simplified constructor.
        /// </summary>
        public PropertyEquality(Expression<Func<TEntity, object>> property, object value, Func<object, string> translateFunction)
            : this(property, new List<object> { value }, translateFunction) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyEquality(Expression<Func<TEntity, object>> property, IEnumerable<object> values, Func<object, string> translateFunction)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Values = values ?? throw new ArgumentNullException(nameof(values));
            TranslateFunction = translateFunction ?? throw new ArgumentNullException(nameof(translateFunction));
        }

        /// <summary>
        /// The property for the equality.
        /// </summary>
        public Expression<Func<TEntity, object>> Property { get; set; }

        public virtual IEnumerable<object> Values { get; set; }

        public virtual Func<object, string> TranslateFunction { get; set; }

        protected static string AsString(object input) => input.ToString();

        public virtual List<string> UseTranslation()
        {
            if (Values == null) { throw new ArgumentNullException(nameof(Values)); }
            if (Values.ToList().Count == 0) { throw new InvalidOperationException(nameof(Values)); }
            if (TranslateFunction == null) { throw new ArgumentNullException(nameof(TranslateFunction)); }
            return Values.Select(x => TranslateFunction(x)).ToList();
        }

    }
}
