using System;
using System.Collections.Generic;
using System.Text;

using CampaignInternal = Maximiz.Model.Entity.Campaign;
using CampaignExternal = Poller.Taboola.Model.Campaign;
using ApprovalStateInternal = Maximiz.Model.Enums.ApprovalState;
using ApprovalStateExternal = Poller.Taboola.Model.ApprovalState;
using CampaignStatusExternal = Poller.Taboola.Model.CampaignStatus;
using CampaignStatusInternal = Maximiz.Model.Enums.CampaignStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poller.Taboola.Mapper;
using Maximiz.Model.Enums;
using Poller.Taboola.Model;
using System.Linq;

namespace Poller.Test.Taboola.Mappers
{

    /// <summary>
    /// Tests our mapper for target objects.
    /// TODO Integers are very hard coded at the moment.
    /// </summary>
    [TestClass]
    public class MapperTargetTestLocation
    {

        /// <summary>
        /// Maps target objects for us.
        /// </summary>
        private MapperTarget _mapperTarget;

        /// <summary>
        /// Constructor to call setup function.
        /// TODO Rethink this design.
        /// </summary>
        public MapperTargetTestLocation()
        {
            Setup();
        }

        /// <summary>
        /// Sets up our testing objects.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mapperTarget = new MapperTarget();
        }

        /// <summary>
        /// Simlulates a valid conversion for location ints to a target object.
        /// The object should include.
        /// </summary>
        [TestMethod]
        public void LocationIntsToTargetsInclude()
        {
            var locationIncludes = new[] { 1, 2 };
            var locationExcludes = new int[0];
            var target = _mapperTarget.LocationIntsToTarget(locationIncludes, locationExcludes);
            MapperTargetUtility.CompareTargets(target, new TargetDefault
            {
                Type = TargetType.Include,
                Value = new[] { "UK", "DE" }
            });
        }

        /// <summary>
        /// Simlulates a valid conversion for location ints to a target object.
        /// The object should exclude.
        /// </summary>
        [TestMethod]
        public void LocationIntsToTargetsExclude()
        {
            var locationIncludes = new int[0];
            var locationExcludes = new[] { 0, 4, 3 };
            var target = _mapperTarget.LocationIntsToTarget(locationIncludes, locationExcludes);
            MapperTargetUtility.CompareTargets(target, new TargetDefault
            {
                Type = TargetType.Include,
                Value = new[] { "NL", "FR", "ES" }
            });
        }

        /// <summary>
        /// Simlulates a valid conversion for location ints to a target object.
        /// The object should exclude.
        /// TODO Do we actually want this to happen? I do think so.
        /// </summary>
        [TestMethod]
        public void LocationIntsToTargetsAll()
        {
            var locationIncludes = new int[0];
            var locationExcludes = new int[0];
            var target = _mapperTarget.LocationIntsToTarget(locationIncludes, locationExcludes);
            MapperTargetUtility.CompareTargets(target, new TargetDefault { Type = TargetType.All });
        }

        /// <summary>
        /// Simulates a situation where a non-existing location integer is present
        /// in either of the location include arrays.
        /// </summary>
        [TestMethod]
        public void InvalidLocationIntsToTargets()
        {
            var locationIncludes = new[] { -1, 8 };
            var locationExcludes = new[] { 2, 3 };
            var target = _mapperTarget.LocationIntsToTarget(locationIncludes, locationExcludes);
            MapperTargetUtility.CompareTargets(target, new TargetDefault { Type = TargetType.All });

            locationIncludes = new[] { 4, 0 };
            locationExcludes = new[] { -347, 374 };
            target = _mapperTarget.LocationIntsToTarget(locationIncludes, locationExcludes);
            MapperTargetUtility.CompareTargets(target, new TargetDefault { Type = TargetType.All });
        }

        /// <summary>
        /// Simulates converting a target to location ints.
        /// </summary>
        [TestMethod]
        public void TargetToLocationIntsInclude()
        {
            var target = new TargetDefault
            {
                Type = TargetType.Include,
                Value = new[] { "NL", "UK" }
            };
            (var includeInts, var excludeInts) = _mapperTarget.TargetToLocationInts(target);
            CompareLocations(new[] { Location.NL, Location.UK }, includeInts);
            Assert.AreEqual(excludeInts.Length, 0);
        }

        /// <summary>
        /// Simulates converting a target to location ints.
        /// </summary>
        [TestMethod]
        public void TargetToLocationIntsExclude()
        {
            var target = new TargetDefault
            {
                Type = TargetType.Exclude,
                Value = new[] { "FR", "DE", "ES" }
            };
            (var includeInts, var excludeInts) = _mapperTarget.TargetToLocationInts(target);
            CompareLocations(new[] { Location.FR, Location.DE, Location.ES }, excludeInts);
            Assert.AreEqual(includeInts.Length, 0);
        }

        /// <summary>
        /// Simulates converting a target to location ints.
        /// </summary>
        [TestMethod]
        public void TargetToLocationIntsAll()
        {
            var target = new TargetDefault
            {
                Type = TargetType.All
            };
            (var includeInts, var excludeInts) = _mapperTarget.TargetToLocationInts(target);
            CompareLocations(new[] { Location.NL, Location.UK, Location.FR, Location.DE, Location.ES }, includeInts);
            Assert.AreEqual(excludeInts.Length, 0);
        }

        /// <summary>
        /// Simulates converting a target to location ints.
        /// </summary>
        [TestMethod]
        public void TargetToLocationIntsInvalid()
        {
            var target = new TargetDefault
            {
                Type = TargetType.Include,
                Value = new[] { "ENL", "UK", "" }
            };
            (var includeInts, var excludeInts) = _mapperTarget.TargetToLocationInts(target);
            Assert.AreEqual(includeInts.Length, 0);
            Assert.AreEqual(excludeInts.Length, 0);
        }

        /// <summary>
        /// Compares two location ranges. Use this for readable and consistent
        /// test result comparison.
        /// </summary>
        /// <param name="locations">As locations</param>
        /// <param name="integers">As integers</param>
        /// <returns>True if equal</returns>
        private bool CompareLocations(Location[] locations, int[] integers)
        {
            try
            {
                Assert.AreEqual(locations.Length, integers.Length);
                for (int i = 0; i < locations.Length; i++)
                {
                    Assert.AreEqual(locations[i], IntToLocation(integers[i]));
                }
                return true;
            }
            catch (AssertFailedException e) { return false; }
        }

        /// <summary>
        /// Used for readability conversion.
        /// TODO This is not bullet proof because we have 2 of these functions now!
        /// </summary>
        /// <param name="integer">The integer</param>
        /// <returns>The location</returns>
        private Location IntToLocation(int integer)
        {
            switch (integer)
            {
                case 0:
                    return Location.NL;
                case 1:
                    return Location.UK;
                case 2:
                    return Location.DE;
                case 3:
                    return Location.ES;
                case 4:
                    return Location.FR;
                default:
                    throw new ArgumentException($"Could not convert" +
                        $" location integer {integer} to location enum.");
            }
        }

    }
}
