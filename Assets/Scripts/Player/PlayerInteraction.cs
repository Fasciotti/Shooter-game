using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new List<Interactable>();

    private Interactable closestInteractable;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();

        player.controls.Character.Interact.performed += context => InteractWithClosest();
    }

    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);
        UpdateClosestInteractable();
    }

    public void UpdateClosestInteractable()
    {
        closestInteractable?.HighlighActive(false);

        closestInteractable = null;

        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance <  closestDistance)
            {
                closestInteractable = interactable;
                closestDistance = distance;
            }
        }
        closestInteractable?.HighlighActive(true);
    }

    public List<Interactable> GetInteractables() => interactables;
}
