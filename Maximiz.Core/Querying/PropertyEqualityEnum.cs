using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Maximiz.Core.Querying
{

    /// <summary>
    /// Version of a <see cref="PropertyEquality{TEntity}"/> for which the
    /// <typeparamref name="TValue"/> type is an enum.
    /// </summary>
    /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
    /// <typeparam name="TValue"><see cref="Enum"/></typeparam>
    public sealed class PropertyEqualityEnum<TEntity, TValue> : PropertyEquality<TEntity>
        where TEntity : Entity
        where TValue : struct
    {

        /// <summary>
        /// Simplified constructor.
        /// </summary>
        public PropertyEqualityEnum(Expression<Func<TEntity, object>> property, TValue value, Func<TValue, string> translateFunction)
            : this(property, new List<TValue> { value }, translateFunction) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyEqualityEnum(Expression<Func<TEntity, object>> property, IEnumerable<TValue> values, Func<TValue, string> translateFunction)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Values = values ?? throw new ArgumentNullException(nameof(values));
            TranslateFunction = translateFunction ?? throw new ArgumentNullException(nameof(translateFunction));
        }

        /// <summary>
        /// Values for this equality.
        /// </summary>
        new public IEnumerable<TValue> Values { get; set; }


        /// <summary>
        /// Used to translate the values.
        /// </summary>
        new public Func<TValue, string> TranslateFunction { get; set; }

        public override List<string> UseTranslation()
        {
            if (Values == null) { throw new ArgumentNullException(nameof(Values)); }
            if (Values.ToList().Count == 0) { throw new InvalidOperationException(nameof(Values)); }
            if (TranslateFunction == null) { throw new ArgumentNullException(nameof(TranslateFunction)); }
            return Values.Select(x => TranslateFunction(x)).ToList();
        }

    }

}
