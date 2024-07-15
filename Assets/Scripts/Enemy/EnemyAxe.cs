using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private GameObject impactFx;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisual;
    public float rotationSpeed;


    private Transform player;
    private Vector3 direction;

    private float flySpeed;
    private float timer = 1f;


    public void AxeSetup(float _flySpeed, Transform _player, float _timer)
    {
        this.flySpeed = _flySpeed;
        this.player = _player;
        this.timer = _timer;
    }

    private void Update()
    {
        axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        timer -= Time.deltaTime;

        if(timer > 0 )
            direction = player.position + Vector3.up - transform.position;
        

        rb.velocity = direction.normalized * flySpeed;
        transform.forward = rb.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Player player = other.GetComponent<Player>();

        if(bullet != null || player != null)
        {
            GameObject newFx = ObjectPool.instance.GetObject(impactFx);
            newFx.transform.position = transform.position;

            ObjectPool.instance.ReturnObject(gameObject);
            ObjectPool.instance.ReturnObject(newFx, 1f);
        }
    }
}
