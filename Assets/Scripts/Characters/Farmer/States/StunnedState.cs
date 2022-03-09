using System;
using System.Collections;
using Jarmallnick.Miscellaneous.StateMachine;

public class StunnedState : State
{
    public StunnedState(Func<IEnumerator> behaviourDelegate) : base(behaviourDelegate)
    {
    }
}
