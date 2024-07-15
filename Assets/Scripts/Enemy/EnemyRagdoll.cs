using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollParent;
    [SerializeField] private Collider[] ragdollColliders;
    [SerializeField] private Rigidbody[] ragdollRigidbodies;

    private void Awake()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        RagdollActive(false);
    }

    public void RagdollActive(bool _active)
    {
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !_active;
        }
    }

    public void CollidersActive(bool _active)
    {
        foreach(Collider cd in ragdollColliders)
        {
            cd.enabled = _active;
        }
    }
}
