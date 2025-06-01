using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactables;

    private Interactable closestInteractable;

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
}
