using UnityEngine;

public class LetCGridLayout : MonoBehaviour
{

    public GameObject cellPrefab;
    public GameObject cellPrefab2;
    public int noMissing;

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

    [ExecuteInEditMode]
    void Update()
    {
        UpdateGrid();
    }

    void CreateGrid()
    {

        previousWidth = rTransform.rect.width;
        previousHeight = rTransform.rect.height;

        // Getting width and height of this rect transform
        float width = rTransform.rect.width - (padding.left + padding.right);
        float height = rTransform.rect.height - (padding.top + padding.bottom);

        // Getting width and height of the cell
        float cellWidth = ((width - (spacing * (col - 1))) / (float)col);
        float cellHeight = ((height - (spacing * (row - 1))) / (float)row);

        float posX = padding.left;
        float posY = -padding.bottom;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (j >= (col - noMissing))
                {
                    cell = Instantiate(cellPrefab2, Vector3.zero, Quaternion.identity) as GameObject;
                }
                else
                {
                    cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                }
                
                cell.transform.SetParent(transform, false);

                RectTransform _rTransform = cell.GetComponent<RectTransform>();

                _rTransform.pivot = new Vector2(0, 0);

                _rTransform.anchorMin = new Vector2(0, 0);
                _rTransform.anchorMax = new Vector2(0, 0);

                _rTransform.sizeDelta = new Vector2(cellWidth, cellHeight);

                _rTransform.anchoredPosition = new Vector2(posX, posY);
                posX += cellWidth + spacing;
            }
            posX = padding.left;
            posY -= cellHeight + spacing;
        }
    }

    void UpdateGrid()
    {

        if (previousWidth != rTransform.rect.width || previousHeight != rTransform.rect.height)
        {

            if (transform.childCount <= 0)
                return;

            previousWidth = rTransform.rect.width;
            previousHeight = rTransform.rect.height;

            // Getting width and height of this rect transform
            float width = rTransform.rect.width - (padding.left + padding.right);
            float height = rTransform.rect.height - (padding.top + padding.bottom);

            // Getting width and height of the cell
            float cellWidth = ((width - (spacing * (col - 1))) / (float)col);
            float cellHeight = ((height - (spacing * (row - 1))) / (float)row);

            float posX = padding.left;
            float posY = -padding.bottom;

            int childIndex = 0;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {

                    RectTransform _rTransform = transform.GetChild(childIndex).GetComponent<RectTransform>();

                    _rTransform.anchorMin = new Vector2(0, 0);
                    _rTransform.anchorMax = new Vector2(0, 0);

                    _rTransform.sizeDelta = new Vector2(cellWidth, cellHeight);

                    _rTransform.anchoredPosition = new Vector2(posX, posY);
                    posX += cellWidth + spacing;

                    childIndex++;
                }
                posX = padding.left;
                posY -= cellHeight + spacing;
            }
        }
    }
}

[System.Serializable]
public class LetCPadding
{
    public int left;
    public int right;
    public int top;
    public int bottom;
}