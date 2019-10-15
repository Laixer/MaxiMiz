using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Test.Taboola.Mappers
{

    /// <summary>
    /// Contains static utility functions for testing our target mapper.
    /// </summary>
    internal static class MapperTargetUtility
    {

        /// <summary>
        /// Compares two target objects which each other.
        /// TODO This might not be the cleanest way. It's here because we never
        /// need it outside of testing functions and we don't want to clog the 
        /// code with an extra equals function.
        /// </summary>
        /// <param name="one">One object</param>
        /// <param name="other">Other object</param>
        /// <returns>True if they are equal</returns>
        internal static bool CompareTargets(TargetDefault one, TargetDefault other)
        {
            try
            {
                Assert.AreEqual(one.Type, other.Type);
                Assert.AreEqual(one.Href, other.Href);

                // Values can be null
                if (one.Value == null) { Assert.IsNull(other.Value); }
                if (other.Value == null) { Assert.IsNull(one.Value); }
                else
                {
                    Assert.AreEqual(one.Value.Length, other.Value.Length);
                    for (int i = 0; i < one.Value.Length; i++)
                    {
                        Assert.AreEqual(one.Value[i], other.Value[i]);
                    }
                }
                return true;
            }
            catch (AssertFailedException e) { return false; }
        }

    }
}
