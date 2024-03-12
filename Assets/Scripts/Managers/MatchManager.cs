using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    private RaycastHit2D hit;

    [SerializeField] private List<MatchObject> matchObjects = new List<MatchObject>();
    [SerializeField] private MatchObject XPObject;
    [SerializeField] private MatchObject currentMatchObject;
    [SerializeField] private Camera cam;
    [SerializeField] private float sum;
    [SerializeField] private MatcObjectSO[] matchObjectSOS;

    private void Awake()
    {
        Instance = this;
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

                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        ClearSelection();
                        GridManager.Instance.CollapseEmptySpaces();
                        sum = 0;
                    }
                }
            }
        }
    }

    private void CheckAndSelectObject()
    {
        if (!matchObjects.Contains(currentMatchObject))
        {
            if (matchObjects.Count == 0 || matchObjects[^1].MatchObjectsAround.Contains(currentMatchObject))
            {
                currentMatchObject.CheckMatchObjectsAround();
                
                matchObjects.Add(currentMatchObject);

                AdditionMatchObjects();

                if (matchObjects.Count > 0)
                {
                    matchObjects[^1].DrawLine(currentMatchObject);
                }
            }
        }
    }

    private void CheckAndRemoveObjects()
    {
        if (matchObjects.Contains(currentMatchObject) && matchObjects.Count >= 2 && matchObjects[^1].MatchObjectsAround.Contains(currentMatchObject))
        {
            SubtractionMatchObjects();
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

                matchObject.transform.DOMove(matchObjects[^1].transform.position, 1f).OnComplete(() =>
                {
                    //matchObject.gameObject.SetActive(false);
                    matchObjects.Remove(matchObject);
                    Destroy(matchObject.gameObject);
                });
            }

            ChangeTargetMatchObjectIdentity(matchObjects[^1],1.1f);

            matchObjects.Clear();
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
        
        ChangeTargetMatchObjectIdentity(XPObject,0);
    }

    private void SubtractionMatchObjects()
    {
        if (matchObjects.Count >= 2)
        {
            matchObjects.Remove(matchObjects[^1]);
            AdditionMatchObjects();
        }
    }

    void ChangeTargetMatchObjectIdentity(MatchObject targetMatchObject, float time)
    {
        foreach (var mos in matchObjectSOS)
        {
            if ((int)mos.matchObjectValue == (int)sum)
            {
                targetMatchObject.ChangeIdentityVo(mos,time);
            }
        }
    }
}