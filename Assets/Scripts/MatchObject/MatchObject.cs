using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MatchObject : MonoBehaviour
{
    [Header("Object Identity")]
    [SerializeField] private MatcObjectSO matchObjectSO;
    [SerializeField] private TMP_Text matchObjectText;
    public float objectValue;
    private SpriteRenderer spriteRenderer;

    [Header("Object Interaction")]
    public List<MatchObject> MatchObjectsAround = new List<MatchObject>();
    public LineRenderer objectLine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeIdentity(matchObjectSO,0);
    }

    public void CheckMatchObjectsAround()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.1f);

        MatchObjectsAround.Clear();

        foreach (var hit in hits)
        {
            if (hit.gameObject.TryGetComponent(out MatchObject hitObject) && hit.gameObject != gameObject)
            {
                if (!MatchObjectsAround.Contains(hitObject) && (int)hitObject.objectValue == (int)objectValue)
                {
                    MatchObjectsAround.Add(hitObject);
                }
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.1f);
    }

    public void ChangeIdentity(MatcObjectSO matchObjectSo,float time)
    {
        StartCoroutine(ChangeIdentityCo(matchObjectSo,time));
    }

    IEnumerator ChangeIdentityCo(MatcObjectSO matchObjectSo,float time)
    {
        yield return new WaitForSeconds(time);
        matchObjectSO = matchObjectSo;
        objectValue = matchObjectSo.matchObjectValue;
        matchObjectText.text = objectValue.ToString();
        spriteRenderer.color = matchObjectSo.matchObjectColor;
    }

    public void DrawLine(GameObject targetObject)
    {
        Vector3 targetPos = targetObject.transform.position;
        Vector3 startPos = gameObject.transform.position;
        
        objectLine.material.color = matchObjectSO.matchObjectColor;
        
        objectLine.positionCount = 2;

        objectLine.SetPosition(0,startPos);
        objectLine.SetPosition(1,targetPos);
    }

    public void DeleteLine()
    {
        objectLine.positionCount = 0;
    }
    
}