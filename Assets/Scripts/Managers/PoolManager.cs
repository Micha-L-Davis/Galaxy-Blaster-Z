using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The Pool Manager is null!");
            return _instance;
        }
    }

    public Transform blastContainer;
    public Transform enemyBlastContainer;
    public GameObject blastPrefab;
    public GameObject enemyBlastPrefab;
    public List<GameObject> blastPool;
    public List<GameObject> enemyBlastPool;

    private void Start()
    {
        blastPool = GenerateBlasts(16, blastPrefab,blastContainer);
        enemyBlastPool = GenerateBlasts(32, enemyBlastPrefab, enemyBlastContainer);
    }

    List<GameObject> GenerateBlasts(int count, GameObject prefab, Transform parent)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = count; i > 0; i--)
        {
            GameObject blast = Instantiate(prefab, parent);
            blast.SetActive(false);
            result.Add(blast);
        }
        return result;
    }

    public GameObject RequestPoolObject(List<GameObject> pool, GameObject prefab, Transform parent)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject blast = Instantiate(prefab, parent);
        pool.Add(blast);
        return blast;
    }

    private void Awake()
    {
        _instance = this;
    }
}
