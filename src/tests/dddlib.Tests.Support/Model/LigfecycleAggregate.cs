﻿namespace dddlib.Tests.Support.Model
{
    using dddlib.Tests.Support.Events;

    public class LifecycleAggregate : AggregateRoot
    {
        public void Destroy()
        {
            this.Apply(new LifecycleEnded());
        }

        public void DoSomething()
        {
            this.Apply(new SomethingHappened());
        }

        private void Handle(LifecycleEnded @event)
        {
            this.EndLifecycle();
        }
    }
}
