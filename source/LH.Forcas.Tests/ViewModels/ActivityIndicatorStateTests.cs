using System;
using System.Threading;
using System.Threading.Tasks;
using LH.Forcas.ViewModels;
using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels
{
    public class ActivityIndicatorStateTests
    {
        public ActivityIndicatorStateTests()
        {
            this.State = new ActivityIndicatorState();
            this.ResetEvent = new ManualResetEvent(false);
        }

        protected ManualResetEvent ResetEvent { get; }

        protected ActivityIndicatorState State { get; }

        protected void VerifyIndicatorIsSet(Func<Task> busyAction)
        {
            Assert.False(this.State.IsBusy);

            var task = busyAction.Invoke();
            Assert.True(this.State.IsBusy);

            this.ResetEvent.Set();
            task.Wait();
            
            Assert.False(this.State.IsBusy);
        }

        public class WhenRunningWithIndicatorState : ActivityIndicatorStateTests
        {
            [Test]
            public void ShouldSetBusyWhenRunning()
            {
                this.VerifyIndicatorIsSet(() => this.State.RunWithIndicator(() => this.ResetEvent.WaitOne()));
            }
        }
    }
}
