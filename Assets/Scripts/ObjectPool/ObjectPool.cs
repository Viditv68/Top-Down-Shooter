using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;


    [SerializeField] private GameObject bulletPref;
    [SerializeField] private int poolSize = 10;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();


    [Header("To Initialize")]
    [SerializeField] private GameObject weaponPickup;
    [SerializeField] private GameObject ammoPickup;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializeNewPool(weaponPickup);
        InitializeNewPool(ammoPickup);
    }

    private void InitializeNewPool(GameObject _prefab)
    {
        poolDictionary[_prefab] = new Queue<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            CreateNewObject(_prefab);  
        }
    }

    private void CreateNewObject(GameObject _prefab)
    {
        GameObject newObject = Instantiate(_prefab, transform);
        newObject.AddComponent<PooledObject>().originalPref = _prefab;
        newObject.SetActive(false);
        poolDictionary[_prefab].Enqueue(newObject);
    }

    public GameObject GetObject(GameObject _prefab)
    {
       if(!poolDictionary.ContainsKey(_prefab))
            InitializeNewPool(_prefab);

        if (poolDictionary[_prefab].Count == 0)
            CreateNewObject(_prefab);

        GameObject objectToGet = poolDictionary[_prefab].Dequeue();
        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;

        return objectToGet;
    }


    public void ReturnObject(GameObject _objectToReturn, float _delay = .001f)
    {
        StartCoroutine(DelayReturn(_delay, _objectToReturn));
    }

    private void ReturnToPool(GameObject _objectToReturn)
    {
        GameObject originalPref = _objectToReturn.GetComponent<PooledObject>().originalPref;
        _objectToReturn.SetActive(false);
        _objectToReturn.transform.parent = transform;
        
        poolDictionary[originalPref].Enqueue(_objectToReturn);
    }

    private IEnumerator DelayReturn(float _delay, GameObject _objectToReturn)
    {
        yield return new WaitForSeconds(_delay);

        ReturnToPool(_objectToReturn);
    }
}
