using System;
using System.Collections;

public class StunnedState : State
{
    public StunnedState(Func<IEnumerator> behaviourDelegate) : base(behaviourDelegate)
    {
    }
}
