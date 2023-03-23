using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityGridLayout : MonoBehaviour
{
    public GameObject cellPrefab;
    public int[] columns;
    public int col;
    public int row;

    private GameObject cell;

    public float spacing;

    public LetCPadding padding;

    private RectTransform rTransform;

    private float previousWidth;
    private float previousHeight;


    public void SetCells()
    {
        rTransform = GetComponent<RectTransform>();

        if (cellPrefab != null)
            CreateGrid();
        else
            Debug.LogWarning("Assign cell prefab");
    }

    void CreateGrid()
    {

        previousWidth = rTransform.rect.width;
        previousHeight = rTransform.rect.height;

        // Getting width and height of this rect transform
        float width = rTransform.rect.width - (padding.left + padding.right);
        float height = rTransform.rect.height - (padding.top + padding.bottom);

        float posY = -padding.top;

        for (int i = 0; i < row; i++)
        {
            int col = columns[i];
            float cellWidth = ((width - (spacing * (col - 1))) / (float)col);
            float cellHeight = ((height - (spacing * (columns.Length - 1))) / (float)columns.Length);

            float posX = padding.left;

            for (int j = 0; j < col; j++)
            {
                cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                
                cell.transform.SetParent(transform, false);

                RectTransform _rTransform = cell.GetComponent<RectTransform>();

                _rTransform.pivot = new Vector2(0.5f, 0.5f); // change pivot to (0.5,0.5)
                _rTransform.anchorMin = new Vector2(0f, 1f);
                _rTransform.anchorMax = new Vector2(0f, 1f);

                _rTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
                _rTransform.anchoredPosition = new Vector2(posX + cellWidth/2, posY - cellHeight/2); // adjust position to center cell

                posX += cellWidth + spacing;
            }
            posX = padding.left;
            posY -= cellHeight + spacing;
        }
    }
}