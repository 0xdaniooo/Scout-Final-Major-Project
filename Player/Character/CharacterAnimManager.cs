using UnityEngine;

//Script for managing player animation states
public class CharacterAnimManager : MonoBehaviour
{
    //References
    public Animator animator;
    public Interaction interaction;
    public CharacterMovement characterMovement;

    private void Update()
    {
        AnimLoop();
    }

    public void IdleAnim()
    {
        //Empty handed idle
        if (interaction.weaponInUse == null)
        {
            animator.Play("IdleAnim");
        }

        //Idle with small weapon
        else if (!interaction.weaponInUse.largeWeapon)
        {
            animator.Play("IdleSmallWeaponAnim");
        }

        //Idle with large weapon
        else
        {
            animator.Play("IdleLargeWeaponAnim");
        }
    }

    public void WalkingAnim()
    {
        //Walking empty handed
        if (interaction.weaponInUse == null)
        {
            animator.Play("WalkAnim");
        }

        //Walking with small weapon
        else if (!interaction.weaponInUse.largeWeapon)
        {
            animator.Play("WalkSmallFirearmAnim");
        }

        //Walking with large weapon
        else
        {
            animator.Play("WalkLargeFirearmAnim");
        }
    }

    public void SprintingAnim()
    {
        //Sprinting empty handed
        if (interaction.weaponInUse == null)
        {
            animator.Play("SprintAnim");
        }

        //Sprinting with small weapon
        else if (!interaction.weaponInUse.largeWeapon)
        {
            animator.Play("SprintAnim");
        }

        //Sprinting with large weapon
        else
        {
            animator.Play("SprintLargeFirearmAnim");
        }
    }

    public void JumpingAnim()
    {
        animator.Play("JumpingAnim");
    }
    
    public void ShootingAnim()
    {
        //Walking empty handed
        if (interaction.weaponInUse == null)
        {
            animator.Play("WalkAnim");
        }

        //Walking with small weapon
        else if (!interaction.weaponInUse.largeWeapon)
        {
            animator.Play("ShootingSmallFirearmAnim");
        }

        //Walking with large weapon
        else
        {
            animator.Play("ShootingLargeFirearmAnim");
        }
    }

    public void AnimLoop()
    {
        if (characterMovement.jumping)
        {
            JumpingAnim();
        }

        else if (characterMovement.sprinting)
        {
            SprintingAnim();
        }

        else if (characterMovement.aiming)
        {
            ShootingAnim();
        }

        else if (!characterMovement.moving)
        {
            IdleAnim();
        }

        else if (characterMovement.moving)
        {
            WalkingAnim();
        }
    }
}
