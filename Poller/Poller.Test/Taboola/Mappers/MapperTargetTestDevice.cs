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
    public class MapperTargetTestDevice
    {

        /// <summary>
        /// Maps target objects for us.
        /// </summary>
        private MapperTarget _mapperTarget;

        /// <summary>
        /// Constructor to call setup function.
        /// TODO Rethink this design.
        /// </summary>
        public MapperTargetTestDevice()
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

        [TestMethod]
        public void InternalExternal()
        {

        }

        [TestMethod]
        public void ExternalInternal()
        {

        }

    }
}
