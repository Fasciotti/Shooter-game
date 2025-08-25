using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private int poolSize;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary
        = new Dictionary<GameObject, Queue<GameObject>>();


    //This must exist or objects that are not "pulled"
    //will not have originalPrefab and PooledObject script
    [Header("To Initialize")]
    [SerializeField] private List<GameObject> objectsToInitialize;

    private void Start()
    {
        foreach(GameObject objectToInitialize in objectsToInitialize)
        {
            InitializeNewPool(objectToInitialize);
        }
    }

    public GameObject GetObject(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
    {
        if (!poolDictionary.ContainsKey(prefab))
            InitializeNewPool(prefab);

        if (poolDictionary[prefab].Count == 0)
            CreateNewObject(prefab);
        
        GameObject objectToGet = poolDictionary[prefab].Dequeue();

        objectToGet.transform.parent = null; // Throw out of the pool

        objectToGet.transform.position = position;
        objectToGet.transform.rotation = rotation;
        
        objectToGet.SetActive(true);

        return objectToGet;
    }

    private IEnumerator DelayReturn(GameObject objectToReturn, float delay)
    {
        yield return new WaitForSeconds(delay);

        ReturnObject(objectToReturn);
    }
    public void ReturnObject(GameObject objectToReturn, float delay)
        => StartCoroutine(DelayReturn(objectToReturn, delay));

    public void ReturnObject(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);

        GameObject objectOrigin =  objectToReturn.GetComponent<PooledObject>().originalPrefab;

        poolDictionary[objectOrigin].Enqueue(objectToReturn);

        objectToReturn.transform.parent = this.transform;
    }

    private void InitializeNewPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
            CreateNewObject(prefab);
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, this.transform);
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;

        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
    }



    #region Awake Singleton
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    #endregion

}
