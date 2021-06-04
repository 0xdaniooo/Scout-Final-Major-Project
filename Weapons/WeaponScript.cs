using UnityEngine;

//Base script for every weapon usable for the player
public class WeaponScript : MonoBehaviour
{
    [Header("Weapon Values")]
    public string weaponName;
    public float damage;
    public float range;
    public float velocity;
    public float spread;
    public int ammo;
    public bool explosiveWeapon;
    public float explosiveRadius;
    public int bulletsPerTap;
    public bool allowKeyHold;
    public float timeBetweenShooting;

    //Other variables
    private int bulletsShot;
    private bool shooting;
    public bool readyToShoot;
    public float timeBetweenShots = 0; 
    private bool allowInvoke = true;
    public bool largeWeapon;

    //References
    public Recoil recoil;
    public GameObject bulletPrefab;
    public GameObject firepoint;
    public GameObject muzzleFlash;
    public GameObject bulletCasingEffect;
    [HideInInspector] public Transform camPos;

    private void Start()
    {
        CharacterMovement charMoveScript = transform.root.GetComponent<CharacterMovement>();
        recoil = GetComponent<Recoil>();
        recoil.playerCamera = charMoveScript.cinemachineRef;
        recoil.camRotation = charMoveScript;

        camPos = charMoveScript.camPos;
    }

    private void Update()
    {
        WeaponInput();
    }

    private void WeaponInput()
    {
        if (allowKeyHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting && ammo > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Calculate weapon spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 projectileDirection = camPos.transform.eulerAngles + new Vector3(x, y, 0);

        recoil.GenerateRecoil();

        //Shooting logic
        GameObject projectile;
        //projectile = Instantiate(bulletPrefab, firepoint.transform.position, Quaternion.Euler(projectileDirection));
        projectile = Instantiate(bulletPrefab, firepoint.transform.position, Quaternion.Euler(projectileDirection));
        //Debug.Break();

        //Parse values onto bullet
        ProjectileScript projectileScript = projectile.gameObject.GetComponent<ProjectileScript>();
        projectileScript.explosive = explosiveWeapon;
        projectileScript.radius = explosiveRadius;
        projectileScript.damage = damage;
        projectileScript.range = range;
        projectileScript.speed = velocity;

        ammo --;
        bulletsShot --;

        //Muzzle flash
        GameObject flash;
        flash = Instantiate(muzzleFlash, firepoint.transform.position, Quaternion.identity);
        Destroy(flash, 0.1f);

        //Bullet casing ejection
        GameObject casing;
        casing = Instantiate(bulletCasingEffect, firepoint.transform.position, Quaternion.identity);
        casing.GetComponent<Rigidbody>().AddForce((-transform.up) * 10f, ForceMode.Impulse);
        Destroy(casing, 1.5f);
        //Debug.Break();

        //Allow weapon to be fired again
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //Fire off all prepared shots
        if (bulletsShot > 0 && ammo > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    public void Status()
    {
        if (ammo <= 0)
        {
            Destroy(gameObject);
        }
    }
}
