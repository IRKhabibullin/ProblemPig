using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : StateMachine, IBombInteractable
{
    private IMoveable moveable;
    private List<Vector2Int> path;
    private IMoveable pigInVision;

    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController roamAnimatorController;
    [SerializeField] private AnimatorOverrideController chaseAnimatorController;
    [SerializeField] private AnimatorOverrideController dirtyAnimatorController;
    [SerializeField] private float idlePeriod;
    [SerializeField] private float petrifyDiration;
    [SerializeField] private int visionRange;
    [SerializeField] private string pigTag;

    public bool isStunned = false;

    protected override void Start()
    {
        moveable = GetComponent<IMoveable>();
        if (moveable == null)
        {
            Debug.LogError("Can't find required component of type <IMoveable>");
            Destroy(gameObject);
        }
        path = new List<Vector2Int>();

        base.Start();
    }

    protected override void InitStates()
    {
        base.InitStates();

        stateMachine = new Dictionary<State, Dictionary<Func<bool>, State>>();
        var roamState = new RoamState(Roam);
        var chaseState = new ChaseState(Chase);
        var stunnedState = new StunnedState(Petrify);

        // Roam state
        var stateTransitions = new Dictionary<Func<bool>, State>();
        stateTransitions.Add(PigFound, chaseState);
        stateTransitions.Add(Stunned, stunnedState);
        stateMachine.Add(roamState, stateTransitions);

        // Chase state
        stateTransitions = new Dictionary<Func<bool>, State>();
        stateTransitions.Add(PigLost, roamState);
        stateTransitions.Add(Stunned, stunnedState);
        stateMachine.Add(chaseState, stateTransitions);

        // Stunned state
        stateTransitions = new Dictionary<Func<bool>, State>();
        stateTransitions.Add(Recovered, roamState);
        stateMachine.Add(stunnedState, stateTransitions);

        // set initial state (without activating it)
        currentState = roamState;
    }

    public void TriggerBomb()
    {
        isStunned = true;
    }

    #region behaviours
    private void SetNewDestination()
    {
        var destination = new Vector2Int(
            UnityEngine.Random.Range(0, GridSystem.Instance.GridSize.width),
            UnityEngine.Random.Range(0, GridSystem.Instance.GridSize.height)
        );
        path = GridSystem.Instance.FindPath(moveable, destination);
    }

    private bool PathEnded()
    {
        return path.Count == 0;
    }

    private Vector2Int NextStepDirectionOnPath()
    {
        var nextStep = path[0];
        path.RemoveAt(0);
        return nextStep - moveable.PositionOnGrid;
    }

    private IEnumerator Roam()
    {
        animator.runtimeAnimatorController = roamAnimatorController;
        while (true)
        {
            CheckTriggers();

            if (PathEnded())
            {
                yield return new WaitForSeconds(idlePeriod);
                SetNewDestination();
            }

            if (!PathEnded() && !moveable.IsMoving)
            {
                moveable.Move(NextStepDirectionOnPath());
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator Chase()
    {
        animator.runtimeAnimatorController = chaseAnimatorController;
        while (true)
        {
            CheckTriggers();

            if (pigInVision != null)
            {
                // if pig is next to farmer, catch it. Probably need to move it to another trigger and add new state.
                if (Vector2Int.Distance(moveable.PositionOnGrid, pigInVision.PositionOnGrid) == 1)
                {
                    Debug.Log("You have been caught!");
                }

                if (!PathEnded() && !moveable.IsMoving)
                {
                    path = GridSystem.Instance.FindPath(moveable, pigInVision.PositionOnGrid, false);
                    moveable.Move(NextStepDirectionOnPath());
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator Petrify()
    {
        animator.runtimeAnimatorController = dirtyAnimatorController;
        yield return new WaitForSeconds(petrifyDiration);
        isStunned = false;
        CheckTriggers();
    }
    #endregion

    #region triggers
    private bool PigFound()
    {
        pigInVision = (IMoveable)GridSystem.Instance.FindByTag(moveable.PositionOnGrid, visionRange, pigTag);
        return pigInVision != null;
    }

    private bool Stunned()
    {
        return isStunned;
    }

    private bool PigLost()
    {
        pigInVision = (IMoveable)GridSystem.Instance.FindByTag(moveable.PositionOnGrid, visionRange, pigTag);
        return pigInVision == null;
    }

    private bool Recovered()
    {
        return currentState.GetType() == typeof(StunnedState);
    }
    #endregion
}
