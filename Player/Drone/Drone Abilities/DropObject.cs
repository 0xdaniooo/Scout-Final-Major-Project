using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

//Allows the drone to drop objects down
public class DropObject : Ability
{
    public GameObject[] objToDrop;
    public Transform dronePos;
    public TextMeshProUGUI textDescription;
    public Image img;

    public override void Cast()
    {
        ObjectDrop();
        StartCoroutine(Display());
    }

    private void ObjectDrop()
    {
        int rand = Random.Range(0, objToDrop.Length);
        Instantiate(objToDrop[rand], dronePos.transform.position, Quaternion.identity);
    }

    private IEnumerator Display()
    {
        textDescription.SetText(AbilityName + " activated");
        textDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textDescription.gameObject.SetActive(false);

        img.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(AbilityCooldown);
        img.color = new Color32(0, 255, 40, 255);
    }
}
