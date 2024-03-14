using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TotalMatchObject : MatchObjectBase
{

    protected override void Start()
    {
        base.Start();
        ActionManager.Instance.OnTotalMatchObjectIdentityChange += ChangeIdentity;
    }
}
