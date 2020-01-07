using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Allows us to do a switch on <see cref="Type"/>s.
    /// TODO This is not used at the moment.
    /// </summary>
    public class TypeSwitch<TResult>
    {

        private readonly Dictionary<Type, Func<object, TResult>> matches = new Dictionary<Type, Func<object, TResult>>();

        public TypeSwitch<TResult> Case<T>(Func<T, TResult> func)
        {
            matches.Add(typeof(T), (x) => func((T)x));
            return this;
        }

        public TResult Switch(object x)
        { 
            try
            {
                foreach (var key in matches.Keys)
                {
                    var matched = key.Equals(x);
                    if (matched)
                    {
                        var result = matches[key](x);
                        return result;
                    }
                }
            } catch (Exception e)
            {
                throw e;
            }

            return matches[x.GetType()](x);
        }

    }
}
