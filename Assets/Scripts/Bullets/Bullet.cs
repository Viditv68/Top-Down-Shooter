using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject bulletImpactFX;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider cd;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisable;
    public void BulletSetup(float _flyDistance)
    {
        bulletDisable = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = 0.25f;
        startPosition = transform.position;
        flyDistance = _flyDistance + 0.5f;
    }


    private void Update()
    {
        FadeTrailIfNeeded();
        DisbaleBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }

    private void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
            ObjectPool.instance.ReturnBullet(gameObject);
    }

    private void DisbaleBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisable)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisable = true;
        }
    }

    private void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
            trailRenderer.time -= 2f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx(collision);
        ObjectPool.instance.ReturnBullet(gameObject);

    }

    private void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject newImpactFx = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newImpactFx, 1f);

        }
    }
}
