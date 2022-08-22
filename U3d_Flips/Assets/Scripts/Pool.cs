using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private Transform _poolTransform;

    public Pool(Transform poolTransform = null)
    {
        _poolTransform = poolTransform;
    }

    private Dictionary<string, Queue<GameObject>> _pools = new Dictionary<string, Queue<GameObject>>();

    public GameObject Get(GameObject prefab)
    {
        GameObject go;
        if (!_pools.ContainsKey(prefab.name))
            _pools.Add(prefab.name, new Queue<GameObject>());

        if (_pools[prefab.name].Count == 0)
        {
            prefab.SetActive(false);
            go = Object.Instantiate(prefab);
            go.name = prefab.name;
            _pools[prefab.name].Enqueue(go);
        }

        go = _pools[prefab.name].Dequeue();
        return go;
    }

    /* TODO FIX public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject go;
        if (!_pools.ContainsKey(prefab.name))
            _pools.Add(prefab.name, new Queue<GameObject>());

        if (_pools[prefab.name].Count > 0)
        {
            go = _pools[prefab.name].Dequeue();
            go.transform.position = position;
            go.transform.rotation = rotation;
        }
        else
        {
            go = GameObject.Instantiate(prefab, position, rotation);
        }

        go.SetActive(true);
        return go;
    }*/

    public void Return(GameObject gameObject)
    {
        gameObject.SetActive(false);

        if (!_pools.ContainsKey(gameObject.name))
            _pools.Add(gameObject.name, new Queue<GameObject>());

        _pools[gameObject.name].Enqueue(gameObject);

        if (_poolTransform != null)
            gameObject.transform.SetParent(_poolTransform);
    }
}