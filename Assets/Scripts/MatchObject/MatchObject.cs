using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class MatchObject : MonoBehaviour
{
    [Header("Object Identity")]
    [SerializeField] private MatcObjectSO matchObjectSO;
    [SerializeField] private TMP_Text matchObjectText;
    public float objectValue;
    private SpriteRenderer spriteRenderer;
    public Color objectColor;

    [Header("Object Interaction")]
    public List<MatchObject> MatchObjectsAround = new List<MatchObject>();
    [SerializeField] private LineRenderer objectLine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeIdentityVo(matchObjectSO,0);
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

    public void ChangeIdentityVo(MatcObjectSO matchObjectSo,float time)
    {
        StartCoroutine(ChangeIdentityCo(matchObjectSo,time));
    }

    IEnumerator ChangeIdentityCo(MatcObjectSO matchObjectSo,float time)
    {
        yield return new WaitForSeconds(time);
        objectValue = matchObjectSo.matchObjectValue;
        matchObjectText.text = objectValue.ToString();
        objectColor = matchObjectSo.matchObjectColor;
        spriteRenderer.color = objectColor;
    }

    public void DrawLine(MatchObject targetObject)
    {
        objectLine.material.color = targetObject.objectColor;
        
        objectLine.positionCount = 2;
        objectLine.SetPosition(0,transform.localPosition);
        objectLine.SetPosition(1,targetObject.transform.localPosition);
    }
    
}