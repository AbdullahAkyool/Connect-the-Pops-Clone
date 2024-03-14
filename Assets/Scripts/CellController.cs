using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellController : MonoBehaviour
{
    public MatchObject[] matchPrefabs;

    void Start()
    {
        InitializeRandomMatchObject();
    }

    public void InitializeRandomMatchObject()
    {
        int randomSpawnIndex = Random.Range(0, matchPrefabs.Length);
        var newMatchObject = Instantiate(matchPrefabs[randomSpawnIndex], transform.position, quaternion.identity);
        newMatchObject.transform.parent = this.transform;
        newMatchObject.ScaleEffect(0);
    }
}