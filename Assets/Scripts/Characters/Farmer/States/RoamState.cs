using System;
using System.Collections;

public class RoamState : State
{
    public RoamState(Func<IEnumerator> behaviourDelegate) : base(behaviourDelegate)
    {
    }
}
