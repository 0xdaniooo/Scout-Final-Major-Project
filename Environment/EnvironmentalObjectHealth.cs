using UnityEngine;

//Health script for environmental objects which can be inherited from
public abstract class EnvironmentalObjectHealth : MonoBehaviour
{
    //Variables
    public float objectHealth;
    public bool destroyed = false;

    public void TakeDamage(float damage)
    {
        if (destroyed)
        {
            return;
        }

        objectHealth -= damage;

        if (objectHealth <= 0)
        {
            objectHealth = 0;
            destroyed = true;
            Action();
        }
    }

    public abstract void Action();
}
