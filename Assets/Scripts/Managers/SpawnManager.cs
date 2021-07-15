using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //prob don't need an action here.
    //public static Action OnWaveStart;

    private void Start()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        //OnWaveStart?.Invoke();
        //come back to this later when we have a UI Manager to display the wave number
        yield return null;

    }
}
