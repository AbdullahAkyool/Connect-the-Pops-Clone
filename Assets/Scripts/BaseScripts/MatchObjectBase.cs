using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class MatchObjectBase : MonoBehaviour
{
    [Header("Object Identity")]
    public TMP_Text matchObjectText;
    public float objectValue;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeIdentity(MatcObjectSO matchObjectSo,float time)
    {
        StartCoroutine(ChangeIdentityCo(matchObjectSo,time));
    }

    protected virtual IEnumerator ChangeIdentityCo(MatcObjectSO matchObjectSo,float time)
    {
        yield return new WaitForSeconds(time);
        objectValue = matchObjectSo.matchObjectValue;
        matchObjectText.text = objectValue.ToString();
        spriteRenderer.color = matchObjectSo.matchObjectColor;
    } 
}
