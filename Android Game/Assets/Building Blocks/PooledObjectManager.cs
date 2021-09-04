using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectManager : MonoBehaviour
{
    #region Singleton
    public static PooledObjectManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion 

    public List<Pool> pools;
    public Dictionary<string,Queue< GameObject>> pooledObjectsDictionary;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    void Start()
    {
        pooledObjectsDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            pooledObjectsDictionary.Add(pool.tag ,objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!pooledObjectsDictionary.ContainsKey(tag))
            return null;


        GameObject objectToSpawn = pooledObjectsDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        pooledObjectsDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
