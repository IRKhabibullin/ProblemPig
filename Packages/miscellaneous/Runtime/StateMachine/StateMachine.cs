using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jarmallnick.Miscellaneous.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State currentState;
        protected Dictionary<State, Dictionary<Func<bool>, State>> stateMachine;

        /// <summary>
        /// Bind states and triggers (build state machine). It could be manual binding, or loading data from file.<para/>
        /// Also need to set initial state without activating it's behaviour
        /// </summary>
        protected virtual void InitStates()
        {
        }

        protected virtual void Start()
        {
            InitStates();
            if (currentState == null)
            {
                Debug.LogError("Initial state not set in StateMachine instance");
                Destroy(this);
            }
        }

        protected void CheckTriggers()
        {
            foreach ((var trigger, var state) in stateMachine[currentState])
            {
                if (trigger())
                {
                    SetState(state);
                    break;
                }
            }
        }

        protected virtual void SetState(State newState)
        {
            if (currentState != null && currentState.activeBehaviour != null)
                StopCoroutine(currentState.activeBehaviour);
            currentState = newState;
            StartCoroutine(currentState.ActivateBehaviour());
        }
    }
}
