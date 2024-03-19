using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TotalMatchObject : MatchObjectBase
{
    private void OnEnable()
    {
        ActionManager.Instance.OnTotalMatchObjectIdentityChange += ChangeIdentity;
    }

    private void OnDisable()
    {
        ActionManager.Instance.OnTotalMatchObjectIdentityChange -= ChangeIdentity;
    }
}
