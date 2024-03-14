using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    private RaycastHit2D hit;

    [SerializeField] private List<MatchObject> matchObjects = new List<MatchObject>();
    [SerializeField] private GameObject XPObject;
    [SerializeField] private MatchObject currentMatchObject;
    [SerializeField] private Camera cam;
    [SerializeField] private float sum;
    [SerializeField] private MatcObjectSO[] matchObjectSOS;
    private MatcObjectSO currentMatcObjectSo;

    private void Awake()
    {
        Instance = this;
        XPObject.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector3 touchPosition = Input.GetTouch(0).position;

            Vector2 rayPosition = cam.ScreenToWorldPoint(touchPosition);
            hit = Physics2D.Raycast(rayPosition, Vector2.zero);

            Touch touch = Input.GetTouch(0);

            if (hit.collider)
            {
                if (hit.collider.gameObject.TryGetComponent(out MatchObject matchObject))
                {
                    currentMatchObject = matchObject;
                    
                    if (touch.phase == TouchPhase.Began)
                    {
                        CheckAndSelectObject();
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        CheckAndSelectObject();
                        CheckAndRemoveObjects();
                    }
                }
            }
            
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                currentMatchObject.ScaleEffect(.5f);
                
                foreach (var matchObj in matchObjects)
                {
                    matchObj.ScaleDown();
                    matchObj.DeleteLine();
                }
                        
                ClearSelection();
                        
                GridManager.Instance.CollapseEmptySpaces();
                sum = 0;
                XPObject.SetActive(false);
            }
        }
    }

    private void CheckAndSelectObject()
    {
        if (!matchObjects.Contains(currentMatchObject))
        {
            if (matchObjects.Count == 0 || matchObjects[^1].MatchObjectsAround.Contains(currentMatchObject))
            {
                currentMatchObject.ScaleUp();
                currentMatchObject.CheckMatchObjectsAround();
                matchObjects.Add(currentMatchObject);
                
                AdditionMatchObjects();

                if (matchObjects.Count > 1)
                {
                    matchObjects[^2].DrawLine(currentMatchObject.gameObject);
                }
            }
        }
    }

    private void CheckAndRemoveObjects()
    {
        if (matchObjects.Contains(currentMatchObject) && matchObjects.Count >= 2 && matchObjects[^1].MatchObjectsAround.Contains(currentMatchObject))
        {
            matchObjects[^1].ScaleDown();
            SubtractionMatchObjects();
            matchObjects[^1].DeleteLine();
        }
    }

    private void ClearSelection()
    {
        if (matchObjects.Count >= 2)
        {
            for (int i = 0; i < matchObjects.Count - 1; i++)
            {
                var matchObject = matchObjects[i];

                matchObject.MatchObjectsAround.Clear();

                if (matchObjects[i] == matchObjects[^1]) break;

                matchObject.transform.parent = matchObjects[^1].transform.parent;

                Vector3 lastObjectPos = matchObjects[^1].transform.position;
                
                matchObject.transform.DOMove(new Vector3(lastObjectPos.x,lastObjectPos.y,lastObjectPos.z+1f), .5f).OnComplete(() =>
                {
                    matchObjects.Remove(matchObject);
                    Destroy(matchObject.gameObject);
                });
            }

            matchObjects[^1].ChangeIdentity(currentMatcObjectSo,.7f);

            matchObjects.Clear();
            
            ActionManager.Instance.OnProgressBarFilled?.Invoke(10);
        }
        else
        {
            matchObjects.Clear();
        }
    }

    private void AdditionMatchObjects()
    {
        int listCount = matchObjects.Count;
        bool isPowerOfTwo = (listCount != 0) && ((listCount & (listCount - 1)) == 0);
        var value = matchObjects[0].objectValue;

        if (listCount >= 2)
        {
            if (isPowerOfTwo)
            {
                sum = value * listCount;
            }
            else
            {
                sum = value * (listCount - 1);
            }
        }
        else
        {
            sum = value;
        }
        
        XPObject.SetActive(true);
        ChangeCurrentMatchObjectSo();
        ActionManager.Instance.OnTotalMatchObjectIdentityChange?.Invoke(currentMatcObjectSo,0);
    }

    private void SubtractionMatchObjects()
    {
        if (matchObjects.Count >= 2)
        {
            matchObjects.Remove(matchObjects[^1]);
            AdditionMatchObjects();
        }
    }

    private void ChangeCurrentMatchObjectSo()
    {
        foreach (var mos in matchObjectSOS)
        {
            if ((int)mos.matchObjectValue == (int)sum)
            {
                currentMatcObjectSo = mos;
            }
        }
    }
    
}