using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private float totalProgressBarValue;
    [SerializeField] private float maxProgressBarValue;

    [SerializeField] private TMP_Text pastLevel;
    [SerializeField] private TMP_Text nextLevel;

    private int levelIndex;
    private int pastlevelValue;
    private int nextlevelValue;

    private void Start()
    {
        ActionManager.Instance.OnProgressBarFilled += FillProgressBar;

        levelIndex = 1;
        LevelUp();
    }

    private void FillProgressBar(float fillValue)
    {
        totalProgressBarValue += fillValue;

        if (totalProgressBarValue >= maxProgressBarValue)
        {
            levelIndex++;
            
            LevelUp();
            
            SetFillImage(0);
            totalProgressBarValue = 0;
        }
        else
        {
            SetFillImage(totalProgressBarValue / maxProgressBarValue);
        }
    }

    private void SetFillImage(float value, float duration = .25f, Ease ease = Ease.OutBack)
    {
        DOTween.Kill(progressBar);
        progressBar.DOFillAmount(value, .3f).SetEase(ease).SetId(progressBar);
    }

    private void LevelUp()
    {
        pastlevelValue = levelIndex;
        nextlevelValue = pastlevelValue + 1;
        
        pastLevel.text = pastlevelValue.ToString();
        nextLevel.text = nextlevelValue.ToString();
    }
}