using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

//Temporarily higjlights weapons for the player to use
public class HighlightWeapons : Ability
{
    //Variables
    public float radius;
    public float abilityTime;
    public int weaponsFound;
    
    //References
    public LayerMask scanLayer;
    public Transform dronePos;
    public Transform parent;
    public Image marker;
    public Image img;
    public TextMeshProUGUI textDescription;

    public override void Cast()
    {
        Scan();
    }

    private void Scan()
    {
        Collider[] colliders = Physics.OverlapSphere(dronePos.transform.position, radius, scanLayer);

        foreach (Collider hit in colliders)
        {
            WeaponScript wpnScript = hit.GetComponent<WeaponScript>();

            if (wpnScript != null && wpnScript.gameObject.transform.tag == "Weapon")
            {
                ObjectMarker om = wpnScript.gameObject.AddComponent(typeof(ObjectMarker)) as ObjectMarker;
                om.img = marker;
                om.ui_parent = parent;

                weaponsFound ++;
            }
        }

        StartCoroutine(RemoveMarkers(colliders));

        StartCoroutine(Display());
    }

    private IEnumerator RemoveMarkers(Collider[] markers)
    {
        yield return new WaitForSeconds(abilityTime);  
        
        foreach(Collider m in markers)
        {
            ObjectMarker om = m.GetComponent<ObjectMarker>();

            if (om != null)
            {
                om.DestroyMarker();
            }
        }
    }

    private IEnumerator Display()
    {
        textDescription.SetText(AbilityName + " activated, found " + weaponsFound + " weapons");
        textDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        textDescription.gameObject.SetActive(false);

        img.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(AbilityCooldown);
        img.color = new Color32(0, 255, 40, 255);

        weaponsFound = 0;
    }
}
