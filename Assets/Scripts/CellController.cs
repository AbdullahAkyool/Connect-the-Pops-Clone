using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellController : MonoBehaviour
{
    public MatcObjectSO[] matchObjectSos;
    public MatchObject matchPrefab;

    private void Start()
    {
        InitializeRandomMatchObject();
    }

    public void InitializeRandomMatchObject()
    {
        var randomSpawnIndex = Random.Range(0, matchObjectSos.Length);
        var newMatchObject = Instantiate(matchPrefab, transform.position, quaternion.identity);
        newMatchObject.ChangeIdentity(matchObjectSos[randomSpawnIndex], 0);
        newMatchObject.transform.parent = this.transform;
        newMatchObject.ScaleEffect(0);
    }
}