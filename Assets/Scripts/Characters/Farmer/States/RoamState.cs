using System;
using System.Collections;
using Jarmallnick.Miscellaneous.StateMachine;

public class RoamState : State
{
    public RoamState(Func<IEnumerator> behaviourDelegate) : base(behaviourDelegate)
    {
    }
}
