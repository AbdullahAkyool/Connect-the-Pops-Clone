using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Action<float> OnProgressBarFilled;
    public Action<MatcObjectSO,float> OnTotalMatchObjectIdentityChange;
    public Action<MatcObjectSO,float> OnMatchObjectIdentityChange;
}
