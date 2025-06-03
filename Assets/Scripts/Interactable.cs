using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    private MeshRenderer mesh;
    private Material defaulMaterial;

    private void Start()
    {
        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();

        defaulMaterial = mesh.material;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
        if (!other.TryGetComponent<PlayerInteraction>(out var playerInteraction))
            return;

        playerInteraction.interactables.Add(this);
        playerInteraction.UpdateClosestInteractable();
    }

    public virtual void Interaction()
    {
        Debug.Log("Interacted with: " + gameObject.name);
    }

    public void HighlighActive(bool active)
    {
        if (active)
            mesh.material = highlightMaterial;
        else
            mesh.material = defaulMaterial;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        
        if (!other.TryGetComponent<PlayerInteraction>(out var playerInteraction))
            return;

        playerInteraction.interactables.Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
