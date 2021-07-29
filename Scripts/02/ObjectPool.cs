using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }
    private Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;

    public GameObject GetObject(GameObject prefab)
    {
        GameObject go;
        if (!poolDict.ContainsKey(prefab.name) || poolDict[prefab.name].Count == 0)
        {
            go = GameObject.Instantiate(prefab);
            PushObject(go);
        }
        if (pool == null)
        {
            pool = new GameObject("ObjectPool");
        }
        GameObject childPool = GameObject.Find(prefab.name + "Pool");
        if (childPool == null)
        {
            childPool = new GameObject(prefab.name + "Pool");
            childPool.transform.SetParent(pool.transform);
        }

        go = poolDict[prefab.name].Dequeue();
        go.transform.SetParent(childPool.transform);
        go.SetActive(true);
        return go;
    }

    public void PushObject(GameObject prefab)
    {
        string name = prefab.name.Replace("(Clone)", string.Empty);
        if (!poolDict.ContainsKey(name))
        {
            poolDict.Add(name, new Queue<GameObject>());
        }
        poolDict[name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}
