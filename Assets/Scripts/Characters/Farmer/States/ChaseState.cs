using System;
using System.Collections;

public class ChaseState : State
{
    public ChaseState(Func<IEnumerator> behaviourDelegate) : base(behaviourDelegate)
    {
    }
}
