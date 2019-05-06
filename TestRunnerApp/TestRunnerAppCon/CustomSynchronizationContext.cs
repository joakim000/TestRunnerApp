﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestRunnerAppCon
{
    public class CustomSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback action, object state)
        {
            SendOrPostCallback actionWrap = (object state2) =>
            {
                SynchronizationContext.SetSynchronizationContext(new CustomSynchronizationContext());
                action.Invoke(state2);
            };
            var callback = new WaitCallback(actionWrap.Invoke);
            ThreadPool.QueueUserWorkItem(callback, state);
        }
        public override SynchronizationContext CreateCopy()
        {
            return new CustomSynchronizationContext();
        }
        public override void Send(SendOrPostCallback d, object state)
        {
            base.Send(d, state);
        }
        public override void OperationStarted()
        {
            base.OperationStarted();
        }
        public override void OperationCompleted()
        {
            base.OperationCompleted();
        }
    }
}
