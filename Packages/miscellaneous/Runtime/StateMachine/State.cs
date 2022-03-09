using System;
using System.Collections;

namespace Jarmallnick.Miscellaneous.StateMachine
{
    public abstract class State
    {
        protected Func<IEnumerator> behaviourDelegate;
        public IEnumerator activeBehaviour;

        public State(Func<IEnumerator> behaviourDelegate)
        {
            this.behaviourDelegate = behaviourDelegate;
        }

        public IEnumerator ActivateBehaviour()
        {
            activeBehaviour = behaviourDelegate();
            return activeBehaviour;
        }
    }

    public class FinalState : State
    {
        public FinalState() : base(EmptyBehaviour)
        {
        }

        private static IEnumerator EmptyBehaviour()
        {
            yield break;
        }
    }
}
