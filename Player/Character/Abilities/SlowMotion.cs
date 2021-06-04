using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

//Ability that slows down the ingame time
public class SlowMotion : Ability
{
    [Range(0, 1)]
    public float timeToSet;
    public float abilityTime;

    public Image img;
    public TextMeshProUGUI textDescription;

    public override void Cast()
    {
        StartCoroutine(SlowMo());
        StartCoroutine(Display());
    }

    private IEnumerator SlowMo()
    {
        Time.timeScale = timeToSet;
        yield return new WaitForSeconds(abilityTime);
        Time.timeScale = 1f;
    }

    private IEnumerator Display()
    {
        textDescription.SetText(AbilityName + " activated");
        textDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textDescription.gameObject.SetActive(false);

        img.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(abilityTime + AbilityCooldown);
        img.color = new Color32(0, 255, 40, 255);
    }
}
