using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public class MatchObject : MatchObjectBase
{
    public MatcObjectSO matchObjectSO;
    [Header("Object Interaction")] 
    private Vector3 orgScale;
    private Vector3 collapseScale;
    public List<MatchObject> MatchObjectsAround = new List<MatchObject>();
    public LineRenderer objectLine;
    private Collider2D matchObjectCollider;
    protected override void Start()
    {
        base.Start();
        orgScale = Vector3.one * .875f;
        collapseScale = new Vector3(.875f,.7f,.875f);
        matchObjectCollider = GetComponent<Collider2D>();
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

        objectLine.SetPosition(0, startPos);
        objectLine.SetPosition(1, targetPos);
    }

    public void DeleteLine()
    {
        objectLine.positionCount = 0;
    }

    public void ScaleUp()
    {
        transform.DOScale(Vector3.one * .92f, .2f);
    }

    public void ScaleDown()
    {
        transform.DOScale(orgScale, .2f);
    }

    private Tween scaleTween;
    public void ScaleEffect(float time)
    {
        if (scaleTween != null && scaleTween.IsActive() && !scaleTween.IsComplete())
        {
            scaleTween?.Kill();
        }
        
        scaleTween = transform.DOScale(orgScale * 1.2f, .1f).SetDelay(time).OnComplete((() =>
        {
            transform.DOScale(orgScale, .1f);
        }));
        
        //scaleTween = transform.DOScale(orgScale * .92f, .1f).SetLoops(2, LoopType.Yoyo).OnKill(() => transform.localScale = orgScale);
    }

    public void CollapseEffect()
    {
        scaleTween = transform.DOScale(collapseScale, .1f).OnComplete((() =>
        {
            transform.DOScale(orgScale, .1f);
        }));
    }
    public void Move(Vector3 pos)
    {
        matchObjectCollider.enabled = false;
        transform.DOMove(pos, .175f).OnComplete(() =>
        {
            Destroy(gameObject);
            MatchManager.Instance.canSelect = true;
        });
    }
}