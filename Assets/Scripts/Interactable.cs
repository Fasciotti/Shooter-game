using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material highlightMaterial;
    private Material defaulMaterial;

    private void Start()
    {
        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();

        defaulMaterial = mesh.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.TryGetComponent<PlayerInteraction>(out var playerInteraction))
            return;

        playerInteraction.interactables.Add(this);
        playerInteraction.UpdateClosestInteractable();
    }

    public void HighlighActive(bool active)
    {
        if (active)
            mesh.material = highlightMaterial;
        else
            mesh.material = defaulMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (!other.TryGetComponent<PlayerInteraction>(out var playerInteraction))
            return;

        playerInteraction.interactables.Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
