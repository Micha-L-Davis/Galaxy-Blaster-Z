using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    Transform _blastContainer;
    [SerializeField]
    GameObject _blastPrefab;
    public List<GameObject> _blastPool;

    private void Start()
    {
        _blastPool = GenerateBlasts(20);
    }

    List<GameObject> GenerateBlasts(int count)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = count; i > 0; i--)
        {
            GameObject blast = Instantiate(_blastPrefab, _blastContainer.transform);
            blast.SetActive(false);
            result.Add(blast);
        }
        return result;
    }

    public GameObject RequestPoolObject(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject blast = Instantiate(_blastPrefab, _blastContainer.transform);
        _blastPool.Add(blast);
        return blast;
    }
}
