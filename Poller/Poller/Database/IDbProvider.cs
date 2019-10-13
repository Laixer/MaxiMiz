using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Poller.Database
{
    public interface IDbProvider
    {

        /// <summary>
        /// Gets us a connection scope.
        /// </summary>
        /// <returns></returns>
        IDbConnection ConnectionScope();

    }
}
