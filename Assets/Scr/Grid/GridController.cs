using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _offset = new Vector2(-5.45f, 4);
    //[SerializeField]
    private LevelData _currentLevelData;

    public void BuildGrid(LevelData levelData)
    {
        _currentLevelData = levelData;
        ClearGrid();
        BuildGrid();
    }
    
    
    private void BuildGrid()
    {
        int rowCount = _currentLevelData.RowCount;
        float verticalSpacing = _currentLevelData.rowSpacing;

        for (int j = 0; j < rowCount; j++)
        {
            GridRowData rowData = _currentLevelData.Rows[j];
            
            int blockCount = rowData.BlockAmount;
            float horizontalSpacing = rowData.blockSpacing;
            Vector2 blockSize = GetBlockSize(rowData.BlockType);
            BlockTile blockTilePerfab = Resources.Load<BlockTile>(GetBlockPath(rowData.BlockType));

            if (blockTilePerfab == null)
            {
                return;
            }
            
            for (int i = 0; i < blockCount; i++)
            {
                BlockTile blockTile = Instantiate<BlockTile>(blockTilePerfab, transform);
                float x = _offset.x + blockSize.x/2 + (blockSize.x + horizontalSpacing) * i;
                float y = _offset.y - (blockSize.y + verticalSpacing) * j;
                blockTile.transform.position = new Vector3(x, y, 0);
                
                blockTile.Init();
            }
        }
    }

    private void ClearGrid()
    {
        int totalChildren = transform.childCount;
        for (int i = totalChildren - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
    
    private Vector2 GetBlockSize(BlockType type)
    {
        if (type == BlockType.Big)
        {
            return new Vector2(1.5f, 0.5f);
        }

        return Vector2.zero;
    }

    private string GetBlockPath(BlockType type)
    {
        if (type == BlockType.Big)
        {
            return "Prefabs/BigBlockTile";
        }

        return string.Empty;
    }
}