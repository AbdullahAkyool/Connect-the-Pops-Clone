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
        //Here, smaller numbers can be kept in the array for the start and larger numbers can be added to the array as the level progresses. 
        //But for now, since it is in the test phase, all numbers are kept in the array
        
        var randomSpawnIndex = Random.Range(0, matchObjectSos.Length);
        var newMatchObject = Instantiate(matchPrefab, transform.position, quaternion.identity);
        newMatchObject.ChangeIdentity(matchObjectSos[randomSpawnIndex], 0);
        newMatchObject.transform.parent = this.transform;
        newMatchObject.ScaleEffect(0);
    }
}