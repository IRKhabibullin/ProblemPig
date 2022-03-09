using System;
using System.Collections;
using Jarmallnick.Miscellaneous.StateMachine;

public class ChaseState : State
{
    public ChaseState(Func<IEnumerator> behaviourDelegate) : base(behaviourDelegate)
    {
    }
}
