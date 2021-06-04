using UnityEngine;
using System.Collections;

//Script for enemy only usable weapons
public class EnemyWeapon : MonoBehaviour
{
    [Header("Weapon Values")]
    public float damage;
    public float range;
    public float velocity;
    public float spread;
    public int maxAmmo;
    private int currentAmmo;
    public float reloadTime;
    public bool largeWeapon;

    [Header("Shooting Values")]
    private int bulletsShot;
    public bool readyToShoot;
    public float timeBetweenShots = 0;
    public float timeBetweenShooting;
    public int bulletsPerTap;
    public bool allowInvoke = true;
    public bool shooting = false;
    public bool reloading = false;

    //References
    public GameObject bulletPrefab;
    public GameObject firepoint;
    public GameObject muzzleFlash;
    public GameObject bulletCasingEffect;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public void WeaponInput()
    {
        if (readyToShoot && currentAmmo > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }

        else if (currentAmmo <= 0)
        {
            if (!reloading)
            {
                StartCoroutine(EnemyReload());
            }
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        shooting = true;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 projectileDirection = firepoint.transform.eulerAngles + new Vector3(x, y, 0);

        //Projectile firing
        GameObject projectile;
        projectile = Instantiate(bulletPrefab, firepoint.transform.position, Quaternion.Euler(projectileDirection));
        ProjectileScript projectileScript = projectile.gameObject.GetComponent<ProjectileScript>();
        projectileScript.damage = damage;
        projectileScript.range = range;
        projectileScript.speed = velocity;

        currentAmmo --;
        bulletsShot --;

        //Muzzle flash
        GameObject flash;
        flash = Instantiate(muzzleFlash, firepoint.transform.position, Quaternion.identity);
        Destroy(flash, 0.1f);

        GameObject casing;
        casing = Instantiate(bulletCasingEffect, firepoint.transform.position, Quaternion.identity);
        casing.GetComponent<Rigidbody>().AddForce((-transform.up) * 10f, ForceMode.Impulse);
        Destroy(casing, 1.9f);

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot > 0 && currentAmmo > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
        shooting = false;
    }

    private IEnumerator EnemyReload()
    {
        reloading = true;
        readyToShoot = false;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        readyToShoot = true;
        reloading = false;
    }
}
