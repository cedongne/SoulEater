using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    Transform _root;
    public void Init()
    {
        if(_root = null)
        {
            _root = new GameObject { name = "Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void Push(Poolable poolable)
    {
    }
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        return null;
    }

    public GameObject GetOriginal(string name)
    {
        return null;
    }
}
