using System.Collections;
using UnityEngine;

public class DirtBomb : MonoBehaviour
{
    private bool isActive = false;

    [SerializeField] private SpriteRenderer sprite;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        isActive = true;
        sprite.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        var interactable = collision.GetComponent<IBombInteractable>();
        if (interactable != null)
        {
            interactable.TriggerBomb();
            Destroy(gameObject);
        }
    }
}
