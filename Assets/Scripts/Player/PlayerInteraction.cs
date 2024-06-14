using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Player player;
    private List<Interactable> interactables = new List<Interactable>();

    private Interactable ClosestInteractable;


    private void Start()
    {
        player.controls.Character.Interaction.performed += Context => InteractWithClosest();
    }

    private void InteractWithClosest()
    {
        ClosestInteractable?.Interaction();
        interactables.Remove(ClosestInteractable);

        UpdateClosestInteractable();
    }

    public void UpdateClosestInteractable()
    {
        ClosestInteractable?.HighlightActive(false);
        ClosestInteractable = null;

        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance) 
            {
                closestDistance = distance;
                ClosestInteractable = interactable;
            }
                
        }

        ClosestInteractable?.HighlightActive(true);
    }

    public List<Interactable> GetInteractables() => interactables;
}
