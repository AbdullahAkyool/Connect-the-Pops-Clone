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

    public int width = 8;
    public int height = 8;
    public CellController cellPrefab;
    private CellController[,] grid;
    public Transform originPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new CellController[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 spawnPosition = new Vector3(originPoint.position.x + i, originPoint.position.y + j, 0);
                CellController newCell = Instantiate(cellPrefab, spawnPosition, Quaternion.identity);

                newCell.transform.parent = originPoint;
                newCell.name = "(" + i + "," + j + ")";
                newCell.transform.localPosition = new Vector3(newCell.transform.localPosition.x, newCell.transform.localPosition.y, 0);

                grid[i, j] = newCell;
            }
        }
    }

    public void CollapseEmptySpaces()
    {
        StartCoroutine(CollapseEmptySpacesCo());
    }

    private IEnumerator CollapseEmptySpacesCo()
    {
        yield return new WaitForSeconds(1.1f);

        for (int x = 0; x < width; x++)
        {
            for (int y = height - 1; y >= 0; y--) // Gridin altından başlayarak yukarı doğru ilerleyin
            {
                if (grid[x, y].transform.childCount == 0)
                {
                    int emptyY = y;
                    for (int searchY = y + 1; searchY < height; searchY++)
                    {
                        if (grid[x, searchY].transform.childCount >= 1)
                        {
                            var emptyCell = grid[x, emptyY];
                            var obj = grid[x, searchY].transform.GetChild(0);

                            obj.parent = emptyCell.transform;
                            
                            obj.DOMove(emptyCell.transform.position, .5f).OnComplete((() =>
                            {
                                obj.localPosition = Vector3.zero;
                            }));

                            emptyY++; // Bir sonraki boş hücreyi işaret edin
                        }
                    }
                }
            }
        }

        StartCoroutine(SpawnAtEmptyCells());
    }

    IEnumerator SpawnAtEmptyCells()
    {
        yield return new WaitForSeconds(1f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].transform.childCount == 0)
                {
                    grid[x, y].Spawn(x, y);
                }
            }
        }
    }
}