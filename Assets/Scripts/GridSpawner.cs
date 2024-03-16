using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnData
    {
        [Tooltip("n value to create nxn grid ")]
        public int Size;

        public CellController CellPrefab;
    }


    [SerializeField] private SpawnData spawnData;

    public List<CellController> CellList = new List<CellController>();

   // public GridController GridController;

   private void Start()
   {
       SpawnGrid();
   }

   [ContextMenu("Spawn")]
    public void SpawnGrid()
    {
        var xSize = spawnData.Size;
        if (xSize < 2)
        {
            Debug.LogError("Out of Range! Not acceptable values that less than 2. Size has been changed to 2.");
            xSize = 2;
        }
        else if (xSize > 20)
        {
            Debug.LogError("Out of Range! Not acceptable values that more than 20. Size has been changed to 20.");
            xSize = 20;
        }

        var ySize = xSize;

        if (CellList.Count > 0)
            DeleteOldCells();

        float maxLength = 4f;

        var cellPrefab = spawnData.CellPrefab;
        cellPrefab.transform.localScale = Vector3.one * (maxLength / xSize);
        var space = cellPrefab.transform.localScale.x;
        var offsetX = (xSize - 1) / 2f;
        var offsetY = offsetX;

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                var pos = Vector3.right * (j - offsetX) * space +
                    Vector3.up * (i - offsetY) * space;

                var cell = Instantiate(cellPrefab, transform);
                cell.transform.position = pos;
                var gridId = new Vector2Int(j, i);
               // cell.Initialize(gridId, xSize);
                CellList.Add(cell);
            }
        }

       // GridController.FillCellList(CellList);

        spawnData.CellPrefab.transform.localScale = Vector3.one;
    }

    private void DeleteOldCells()
    {
        for (int i = CellList.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(CellList[i].gameObject);
        }
        CellList.Clear();
    }
}
