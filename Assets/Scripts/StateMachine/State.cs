using System;
using System.Collections;

public abstract class State
{
    private Func<IEnumerator> behaviourDelegate;
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
