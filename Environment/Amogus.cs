using UnityEngine;

//Easter egg. Spawns a random weapno
public class Amogus : InteractableObject
{
    public GameObject[] weapons;
    public Transform spawnPoint;
    private GameObject choseNWeapon;
    private bool interacted;

    public override string GetDescription()
    {
        if (!interacted) return "Press [F] to interact with Amogus";
        return "You have already interacted with Amogus";
    }

    public override void Interact()
    {
        if (!interacted)
        {
            interacted = true;

            int weaponToSpawn = Random.Range(0, weapons.Length);
            choseNWeapon = weapons[weaponToSpawn];

            GameObject wpn = Instantiate(choseNWeapon, spawnPoint.transform.position, Quaternion.identity);
            wpn.gameObject.tag = "Weapon";
            wpn.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
