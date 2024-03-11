using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MatchObject : MonoBehaviour
{
    [SerializeField] private MatcObjectSO matchObjectSO;
    [SerializeField] private TMP_Text matchObjectText;
    public float objectValue;
    private Color objectColor;

    public List<MatchObject> MatchObjectsAround = new List<MatchObject>();

    private void Start()
    {
        ChangeIdentity(matchObjectSO);
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

    public void ChangeIdentity(MatcObjectSO matchObjectSo)
    {
        objectValue = matchObjectSo.matchObjectValue;
        matchObjectText.text = objectValue.ToString();
        objectColor = matchObjectSo.matchObjectColor;
        GetComponent<SpriteRenderer>().color = objectColor;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.1f);
    }
}