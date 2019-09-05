using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Poller.Taboola.Model;

using AccountCore = Maximiz.Model.Entity.Account;
using CampaignCore = Maximiz.Model.Entity.Campaign;
using AdItemCore = Maximiz.Model.Entity.AdItem;
using Poller.Helper;
using System.Linq;
using System.Diagnostics;

namespace Poller.Taboola
{

    /// <summary>
    /// This is the part of our Taboola Poller that uses http. All CRUD
    /// operations are placed within this file.
    /// </summary>
    internal partial class TaboolaPoller
    {
    }
}
