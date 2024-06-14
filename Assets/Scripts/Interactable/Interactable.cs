using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected MeshRenderer mesh;
    protected Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    private void Start()
    {
        if(mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();
        defaultMaterial = mesh.sharedMaterial;
    }

    protected void UpdateMeshAndMaterial(MeshRenderer _newMesh)
    {
        mesh = _newMesh;
        defaultMaterial = _newMesh.sharedMaterial;
    }

    public void HighlightActive(bool _active)
    {
        mesh.material = _active ? highlightMaterial : defaultMaterial;
        
    }

    public virtual void Interaction()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if(playerInteraction != null )
        {
            playerInteraction.GetInteractables().Add(this);
            playerInteraction.UpdateClosestInteractable();
        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.GetInteractables().Remove(this);
            playerInteraction.UpdateClosestInteractable();
        }
    }
}
