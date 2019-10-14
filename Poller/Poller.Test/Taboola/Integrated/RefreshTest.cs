using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poller.Poller;
using System;
using Poller.Extensions;

using AccountInternal = Maximiz.Model.Entity.Account;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using AdItemInternal = Maximiz.Model.Entity.AdItem;
using System.Threading.Tasks;
using System.Linq;
using Poller.Taboola.Mapper;
using System.Collections.Generic;
using Poller.Test.Taboola.Mappers;

namespace Poller.Test.Taboola.Integrated
{

    /// <summary>
    /// Tests the integration of our software with our own database and the
    /// Taboola API, for the <see cref="IPollerDataSyncback"/> interface.
    /// 
    /// TODO Initialize and cleanup run before and after each test. This means
    /// some objects are disposed after they are created a second time.
    /// </summary>
    [TestClass]
    public class RefreshTest : PollerBase
    {

        /// <summary>
        /// This creates all required object to perform poller executions.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Setup();
        }

        /// <summary>
        /// Simulates a refresh operation.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestMethod()
        {
            var context = new PollerContext(0, null);
            await _poller.RefreshAdvertisementDataAsync(context, _source.Token);
        }

        /// <summary>
        /// Removes all the mess that we have created.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            CleanUp();
        }

    }
}
