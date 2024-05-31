using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;


    [SerializeField] private GameObject bulletPref;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CreateInitialPool();
    }

    private void CreateInitialPool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            CreateNewBullet();
        }
    }

    private void CreateNewBullet()
    {
        GameObject newBullet = Instantiate(bulletPref, transform);
        newBullet.SetActive(false);
        bulletPool.Enqueue(newBullet);
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
            CreateNewBullet();
        GameObject bullet = bulletPool.Dequeue();
        bullet.SetActive(true);
        bullet.transform.parent = null;

        return bullet;
    }

    public void ReturnBullet(GameObject _bullet)
    {
        _bullet.SetActive(false);
        bulletPool.Enqueue(_bullet);
        _bullet.transform.parent = transform;
    }
}
