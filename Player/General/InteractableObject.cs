using UnityEngine;

//Used for setting up interactable objects
public abstract class InteractableObject : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold
    }

    float holdTime;

    public InteractionType interactionType;

    public abstract string GetDescription();
    public abstract void Interact();

    public void IncreaseHoldTime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;

    public float GetHoldTime() => holdTime;
}
