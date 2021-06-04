using UnityEngine;

//Script for managing enemy animation states
public class EnemyAnimManager : MonoBehaviour
{
    private Animator animator;
    private EnemyAI enemy;
    private EnemyWeapon enemyWeapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        enemyWeapon = enemy.enemyWeapon;
    }

    private void Update()
    {
        AnimLoop();
    }

    private void AnimLoop()
    {
        if (enemy != null && enemy.isAlive)
        {
            if (enemyWeapon.shooting)
            {
                ShootAnim();
            }

            else if (enemyWeapon.reloading)
            {
                ReloadAnim();
            }

            else if (enemy.moving)
            {
                MovingAnim();
            }
        }
    }

    private void MovingAnim()
    {
        if (!enemyWeapon.largeWeapon)
        {
            animator.Play("WalkAnim");
        }

        else if (enemyWeapon.largeWeapon)
        {
            animator.Play("WalkSmallFirearmAnim");
        }
    }

    private void ReloadAnim()
    {
        if (!enemyWeapon.largeWeapon)
        {
            animator.Play("ReloadSmallFirearmAnim");
        }

        else if (enemyWeapon.largeWeapon)
        {
            animator.Play("ReloadLargeFirearmAnim");
        }
    } 

    private void ShootAnim()
    {
        if (!enemy.moving)
        {
            animator.Play("ShootAnim");
        }

        else if (enemy.moving)
        {
            animator.Play("ShootWalkAnim");
        }
    }

    public void DeathAnim()
    {
        enemy = null;
        int animToPlay = Random.Range(1, 5);
        string animation = "Death" + animToPlay.ToString() + "Anim";
        animator.Play(animation);
    }
}
