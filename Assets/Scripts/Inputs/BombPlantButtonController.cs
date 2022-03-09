using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BombPlantButtonController : MonoBehaviour
{
    [SerializeField] private Button bombPlantButton;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private int fillFractionsCount;

    public void ResetCooldown()
    {
        bombPlantButton.interactable = true;
        cooldownImage.fillAmount = 0;
    }

    public IEnumerator ActivateCoroutine(float activationTime)
    {
        bombPlantButton.interactable = false;
        yield return new WaitForSeconds(activationTime);
        bombPlantButton.interactable = true;
    }

    public IEnumerator StartCooldownCoroutine(float cooldownTime)
    {
        var yieldPeriod = cooldownTime / (float)fillFractionsCount;

        cooldownImage.fillAmount = 1;
        bombPlantButton.interactable = false;
        while (cooldownImage.fillAmount > 0)
        {
            yield return new WaitForSeconds(yieldPeriod);
            ReduceFillAmount();
        }
        bombPlantButton.interactable = true;
    }

    private void ReduceFillAmount()
    {
        cooldownImage.fillAmount = Mathf.Clamp(cooldownImage.fillAmount - 1 / (float)fillFractionsCount, 0, 1);
    }
}
