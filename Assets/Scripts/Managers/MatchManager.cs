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
    
    [SerializeField] private List<GameObject> matchObjects = new List<GameObject>();
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
                GridManager.Instance.FillEmptySpaces();
            }
        }
    }

    private void CheckAndSelectObject()
    {
        if (hit.collider && !matchObjects.Contains(hit.collider.gameObject))
        {
            if (hit.collider.gameObject.TryGetComponent(out MatchObject matchObject))
            {
                if (matchObjects.Count == 0 || matchObjects[^1].GetComponent<MatchObject>().MatchObjectsAround.Contains(matchObject))
                {
                    matchObject.CheckMatchObjectsAround();
                    matchObjects.Add(matchObject.gameObject);
                    
                    AdditionMatchObjects();
                }
            }
        }
    }

    private void CheckAndRemoveObjects()
    {
        if (hit.collider && matchObjects.Contains(hit.collider.gameObject) && matchObjects.Count >= 2)
        {
            if (hit.collider.gameObject.TryGetComponent(out MatchObject matchObject))
            {
                if (matchObjects.Contains(matchObject.gameObject) && matchObjects[^1].GetComponent<MatchObject>().MatchObjectsAround.Contains(matchObject))
                {
                    SubtractionMatchObjects();
                }   
            }
        }
    }
    
    private void ClearSelection()
    {
        if (matchObjects.Count >= 2)
        {
            for (int i = 0; i < matchObjects.Count-1; i++)
            {
                var matchObject = matchObjects[i].GetComponent<MatchObject>();
                
                matchObject.MatchObjectsAround.Clear();
                
                if(matchObjects[i] == matchObjects[^1]) break;
                
                matchObject.transform.parent = matchObjects[^1].transform.parent;
                
                matchObject.transform.DOMove(matchObjects[^1].transform.position, 1f).OnComplete(() =>
                {
                    //matchObject.gameObject.SetActive(false);
                    matchObjects.Remove(matchObject.gameObject);
                    Destroy(matchObject.gameObject);
                });
            }
            

            foreach (var mos in matchObjectSOS)
            {
                if ((int)mos.matchObjectValue == (int)sum)
                {
                    matchObjects[^1].GetComponent<MatchObject>().ChangeIdentityVo(mos);                     
                }
            }
            
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
        var value = matchObjects[0].GetComponent<MatchObject>().objectValue;

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
    }

    private void SubtractionMatchObjects()
    {
        if (matchObjects.Count >= 2)
        {
            matchObjects.Remove(matchObjects[^1]);
            AdditionMatchObjects();
        }
    }
}
