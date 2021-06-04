using UnityEngine;

//Allows for interactions with doors
public class Door : InteractableObject
{
    private Animator anim;
    public bool opened;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override string GetDescription()
    {
        if (opened) return "Press [F] to close the door";
        return "Press [F] to open the door";
    }

    public override void Interact()
    {
        if (opened)
        {
            opened = false;
            anim.SetBool("isOpen", false);
        }

        else
        {
            opened = true;
            anim.SetBool("isOpen", true);
        }
    }
}
