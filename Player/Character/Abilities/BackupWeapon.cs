using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

//Backup weaopn ability which gives a very deadly weapon
public class BackupWeapon : Ability
{
    //Variables
    public float abilityTime;

    //References
    public GameObject weapon;
    public Transform spawnPoint;
    public Image img;
    public TextMeshProUGUI textDescription;

    public override void Cast()
    {
        StartCoroutine(SpawnWeapon());
        StartCoroutine(Display());
    }

    private IEnumerator SpawnWeapon()
    {
        GameObject wpn = Instantiate(weapon, spawnPoint.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(abilityTime);
        Destroy(wpn);
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
