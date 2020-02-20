using Maximiz.Model.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.StateMachine.Abstraction
{

    public interface IStateMachineManager
    {

        Task AttemptStartStateMachineAsync(MyOperation operation, CancellationToken token);

    }
}
