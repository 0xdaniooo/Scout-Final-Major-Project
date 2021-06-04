using UnityEngine;

//Used to create various player abilities
public abstract class Ability : MonoBehaviour
{
    //Ability values
    [SerializeField] private string abilityName = "New Ability Name";
    [SerializeField] private string abilityDescription = "New Ability Description";
    [SerializeField] private float abilityCooldown = 1f;
    [SerializeField] private KeyCode abilityKey;

    public string AbilityName { get { return abilityName; } }
    public float AbilityCooldown { get {return abilityCooldown; } }
    [Range(1, 2)] public int stateAllowed;
    public GameManager gameManager;

    public abstract void Cast();

    protected virtual void Update()
    {
        if (Input.GetKeyDown(abilityKey) && gameManager.playerState == stateAllowed)
        {
            if (CooldownHandler.Instance.IsOnCooldown(this))
            { 
                return; 
            }

            Cast();

            CooldownHandler.Instance.PutOnCooldown(this);
        }
    }
}
