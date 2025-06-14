using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected Material highlightMaterial;
    protected MeshRenderer mesh;
    protected Material defaulMaterial;

    private void Start()
    {
        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();

        defaulMaterial = mesh.sharedMaterial;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
        if (!other.TryGetComponent<PlayerInteraction>(out var playerInteraction))
            return;

        playerInteraction.GetInteractables().Add(this);
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

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaulMaterial = mesh.sharedMaterial;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        
        if (!other.TryGetComponent<PlayerInteraction>(out var playerInteraction))
            return;

        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
