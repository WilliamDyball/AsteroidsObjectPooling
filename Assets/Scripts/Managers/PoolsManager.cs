using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    public static PoolsManager instance;
    private void Awake()
    {
        if (PoolsManager.instance == null)
        {
            PoolsManager.instance = this;
        }
        else if (PoolsManager.instance != this)
        {
            Destroy(PoolsManager.instance.gameObject);
            PoolsManager.instance = this;
        }

        InitPools();
    }

    [System.Serializable]
    public class Pool
    {
        public string strName;
        public GameObject goPrefab;
        public int iSize;
    }

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> PoolDict;

    public void InitPools()
    {
        PoolDict = new Dictionary<string, List<GameObject>>();
        for (int i = 0; i < pools.Count; i++)
        {
            List<GameObject> objects = new List<GameObject>();
            for (int j = 0; j < pools[i].iSize; j++)
            {
                GameObject obj = Instantiate(pools[i].goPrefab);
                obj.SetActive(false);
                objects.Add(obj);
            }
            PoolDict.Add(pools[i].strName, objects);
        }
    }



    private GameObject GetPooledObject(string _strName)
    {
        for (int i = 0; i < PoolDict[_strName].Count; i++)
        {
            if (!PoolDict[_strName][i].activeInHierarchy)
            {
                return PoolDict[_strName][i];
            }
        }

        GameObject obj = (GameObject)Instantiate(PoolDict[_strName][0]);
        PoolDict[_strName].Add(obj);
        return obj;
    }

    //Spawn an object from the pool
    public GameObject SpawnObjFromPool(string _strName, Vector3 _v3Pos, Quaternion _qRot, int _iGen = 1)
    {
        if (!PoolDict.ContainsKey(_strName))
        {
            Debug.LogError("The pool dictionary does not contain: " + _strName);
            return null;
        }

        GameObject objToSpawn = GetPooledObject(_strName);

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = _v3Pos;
        objToSpawn.transform.rotation = _qRot;

        if (objToSpawn.TryGetComponent(out Asteroid asteroid))
        {
            asteroid.SetGeneration(_iGen);
        }
        if (objToSpawn.TryGetComponent(out Pooled pooled))
        {
            pooled.OnObjectSpawn();
        }

        return objToSpawn;
    }


}
