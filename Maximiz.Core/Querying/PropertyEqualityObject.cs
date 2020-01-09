//using Maximiz.Model.Entity;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

//namespace Maximiz.Core.Querying2
//{

//    /// <summary>
//    /// Version of a <see cref="PropertyEquality{TEntity}"/> for which the
//    /// <typeparamref name="TValue"/> type is an object.
//    /// </summary>
//    /// <remarks>
//    /// This does not work with <see cref="Enum"/>s, use the <see cref="
//    /// PropertyEqualityEnum{TEntity, TValue}"/> in this situation.
//    /// </remarks>
//    /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
//    /// <typeparam name="TValue"><see cref="object"/></typeparam>
//    public sealed class PropertyEqualityObject<TEntity, TValue> : PropertyEquality<TEntity, TValue>
//        where TEntity : Entity
//        //where TValue : object
//    {

//        /// <summary>
//        /// Simplified constructor.
//        /// </summary>
//        public PropertyEqualityObject(Expression<Func<TEntity, object>> property, TValue value)
//            : this(property, new List<TValue> { value }, AsString) { }

//        /// <summary>
//        /// Simplified constructor.
//        /// </summary>
//        public PropertyEqualityObject(Expression<Func<TEntity, object>> property, IEnumerable<TValue> values)
//            : this(property, values, AsString) { }

//        /// <summary>
//        /// Simplified constructor.
//        /// </summary>
//        public PropertyEqualityObject(Expression<Func<TEntity, object>> property, TValue value, Func<TValue, string> translateFunction)
//            : this(property, new List<TValue> { value } , translateFunction) { }

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        public PropertyEqualityObject(Expression<Func<TEntity, object>> property, IEnumerable<TValue> values, Func<TValue, string> translateFunction)
//        {
//            Property = property ?? throw new ArgumentNullException(nameof(property));
//            Values = values ?? throw new ArgumentNullException(nameof(values));
//            TranslateFunction = translateFunction ?? throw new ArgumentNullException(nameof(translateFunction));
//        }

//        /// <summary>
//        /// Values for this equality.
//        /// </summary>
//        public override IEnumerable<TValue> Values { get; set; }

//        /// <summary>
//        /// Used to translate the values.
//        /// </summary>
//        public override Func<TValue, string> TranslateFunction { get; set; }

//    }
//}
