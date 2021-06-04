using UnityEngine;
using TMPro;

//Displays player weapon ammo count
public class AmmoCount : MonoBehaviour
{
    //References
    public TextMeshProUGUI ammoText;
    public WeaponScript weapon;

    private void Update()
    {
        //Try to grab weapon script
        weapon = GetComponentInChildren<WeaponScript>();

        if (weapon != null)
        {
            //If player has no ammo
            if (weapon.ammo <= 0)
            {
                ammoText.SetText("No ammo remaining");
            }
            

            //Display normal ammo count
            {
                ammoText.SetText(weapon.weaponName + " : " + weapon.ammo);
            }

            if (transform.childCount == 0)
            {
                weapon = null;
            }
        }

        else if (transform.childCount == 0)
        {
            ammoText.SetText("No weapon equipped");
        }
    }
    
}
