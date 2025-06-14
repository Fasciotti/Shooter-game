using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

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

    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
            InitializeNewPool(prefab);

        if (poolDictionary[prefab].Count == 0)
            CreateNewObject(prefab);
        
        GameObject objectToGet = poolDictionary[prefab].Dequeue();
        objectToGet.SetActive(true);
        objectToGet.transform.parent = null; // Throw out of the pool

        return objectToGet;
    }

    private IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);

        ReturnObject(objectToReturn);
    }
    public void ReturnObject(float delay, GameObject objectToReturn)
        => StartCoroutine(DelayReturn(delay, objectToReturn));

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
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    #endregion

}
