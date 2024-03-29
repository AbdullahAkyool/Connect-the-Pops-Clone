using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width;
    public int height;
    public CellController cellPrefab;
    private CellController[,] _grid;
    public Transform originPoint;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnGrid();
    }
    
    private void SpawnGrid()
    {
        _grid = new CellController[width, height];

        float maxLength = 4f; //grid scale
        
        cellPrefab.transform.localScale = Vector3.one * (maxLength / width);
        var space = cellPrefab.transform.localScale.x;
        var offsetX = (width - 1) / 2f;
        var offsetY = offsetX;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var pos =originPoint.position + Vector3.up * (j - offsetX) * space + Vector3.right * (i - offsetY) * space;
                var newCell = Instantiate(cellPrefab, transform);
                newCell.name = "(" + i + "," + j + ")";
                _grid[i, j] = newCell;

                newCell.transform.position = pos;
            }
        }
    }
    

    // private void CreateGrid()
    // {
    //     grid = new CellController[width, height];
    //
    //     for (int i = 0; i < width; i++)
    //     {
    //         for (int j = 0; j < height; j++)
    //         {
    //             Vector3 spawnPosition = new Vector3(originPoint.position.x + i, originPoint.position.y + j, 0);
    //             CellController newCell = Instantiate(cellPrefab, spawnPosition, Quaternion.identity);
    //
    //             newCell.transform.parent = originPoint;
    //             newCell.name = "(" + i + "," + j + ")";
    //             newCell.transform.localPosition = new Vector3(newCell.transform.localPosition.x, newCell.transform.localPosition.y, 0);
    //
    //             grid[i, j] = newCell;
    //         }
    //     }
    // }

    public void CollapseEmptySpaces()
    {
        StartCoroutine(CollapseEmptySpacesCo());
    }

    private IEnumerator CollapseEmptySpacesCo()
    {
        yield return new WaitForSeconds(.2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = height - 1; y >= 0; y--) // scan the grid from the bottom to the top
            {
                if (_grid[x, y].transform.childCount == 0)
                {
                    int emptyY = y;
                    for (int searchY = y + 1; searchY < height; searchY++)
                    {
                        if (_grid[x, searchY].transform.childCount >= 1)
                        {
                            var emptyCell = _grid[x, emptyY];
                            var obj = _grid[x, searchY].transform.GetChild(0);

                            obj.parent = emptyCell.transform;
                            
                            obj.DOMove(emptyCell.transform.position, .25f).OnComplete((() =>
                            {
                                obj.localPosition = Vector3.zero;
                                obj.GetComponent<MatchObject>().CollapseEffect();
                            }));

                            emptyY++; //select next empty cell
                        }
                    }
                }
            }
        }

        StartCoroutine(SpawnAtEmptyCells());
    }

    IEnumerator SpawnAtEmptyCells()
    {
        yield return new WaitForSeconds(.21f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y].transform.childCount == 0)
                {
                    _grid[x, y].InitializeRandomMatchObject();
                }
            }
        }
    }
}