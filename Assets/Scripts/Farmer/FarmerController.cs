using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : MonoBehaviour
{
    private IMoveable moveable;
    public List<Vector2Int> path { get; private set; }
    private IEnumerator roamCoroutine;

    [SerializeField] private float idlePeriod;

    void Start()
    {
        moveable = GetComponent<IMoveable>();
        if (moveable == null)
        {
            Debug.LogError("Can't find required component of type <IMoveable>");
            Destroy(gameObject);
        }
        path = new List<Vector2Int>();

        roamCoroutine = RoamCoroutine();
        StartCoroutine(roamCoroutine);
    }

    private void SetNewDestination()
    {
        var destination = new Vector2Int(Random.Range(0, GridSystem.Instance.GridSize.width), Random.Range(0, GridSystem.Instance.GridSize.height));
        path = GridSystem.Instance.FindPath(moveable, destination);
    }

    private IEnumerator RoamCoroutine()
    {
        while (true)
        {
            if (path.Count == 0)
            {
                yield return new WaitForSeconds(idlePeriod);
                SetNewDestination();
            }
            if (path.Count > 0 && !moveable.IsMoving)
            {
                moveable.Move(path[0] - moveable.PositionOnGrid);
                path.RemoveAt(0);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
