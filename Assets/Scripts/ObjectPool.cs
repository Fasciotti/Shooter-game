using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize;

    private Queue<GameObject> bulletPool;

    private void Start()
    {
        bulletPool = new Queue<GameObject>();

        CreateInitialPool();
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
            CreateNewBullet();
        
        GameObject bulletToGet = bulletPool.Dequeue();
        bulletToGet.SetActive(true);
        bulletToGet.transform.parent = null;

        return bulletToGet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bulletPool.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.parent = this.transform;
        
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < poolSize; i++)
            CreateNewBullet();
    }

    private void CreateNewBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, this.transform);
        newBullet.SetActive(false);
        bulletPool.Enqueue(newBullet);
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
