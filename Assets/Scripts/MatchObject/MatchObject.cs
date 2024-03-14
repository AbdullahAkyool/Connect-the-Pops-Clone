using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MatchObject : MatchObjectBase
{
    public MatcObjectSO matchObjectSO;
    [Header("Object Interaction")]
    private Vector3 orgScale;
    public List<MatchObject> MatchObjectsAround = new List<MatchObject>();
    public LineRenderer objectLine;

    protected override void Start()
    {
        base.Start();
        ChangeIdentity(matchObjectSO,0);
        orgScale = transform.localScale;
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

    protected override IEnumerator ChangeIdentityCo(MatcObjectSO matchObjectSo, float time)
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

    public void ScaleUp()
    {
        transform.DOScale(Vector3.one*.92f,.2f);
    }

    public void ScaleDown()
    {
        transform.DOScale(orgScale, .2f);
    }

    public void ScaleEffect(float time)
    {
        StartCoroutine(ScaleEffectCo(time));
    }

    IEnumerator ScaleEffectCo(float time)
    {
        yield return new WaitForSeconds(time);
        
        transform.DOScale(Vector3.one*.92f,.2f).OnComplete((() =>
        {
            transform.DOScale(orgScale, .2f);

        }));
    }
}