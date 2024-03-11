using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public GameObject[] tilePrefabs;
    private GameObject[,] grid;
    public Transform originPoint;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GameObject[width, height];
        FillEmptySpaces();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FillEmptySpaces();
        }
    }


    public void UpdateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    // Üstündeki hücreleri aşağıya doğru kaydır
                    for (int aboveY = y + 1; aboveY < height; aboveY++)
                    {
                        if (grid[x, aboveY] != null)
                        {
                            grid[x, y] = grid[x, aboveY];
                            grid[x, aboveY] = null;

                            // Objeyi yeni pozisyonuna taşı
                            grid[x, y].transform.position = new Vector3(x, y, 0);
                            break; // Bir sonraki boş hücreye geç
                        }
                    }
                }
            }
        }

        FillEmptySpaces();
    }

    void FillEmptySpaces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!grid[x, y])
                {
                    Vector3 spawnPosition = new Vector3(originPoint.position.x + x, originPoint.position.y + y, 0);
                    GameObject tile = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], spawnPosition,
                        Quaternion.identity);

                    tile.transform.parent = originPoint;
                    tile.transform.localPosition =
                        new Vector3(tile.transform.localPosition.x, tile.transform.localPosition.y, 0);

                    grid[x, y] = tile;
                }
            }
        }
    }
}