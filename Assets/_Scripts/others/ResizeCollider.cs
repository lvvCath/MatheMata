using UnityEngine;

public class ResizeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        var rect = rectTransform.rect;
        boxCollider2D.size = new Vector2(rect.width, rect.height);
    }
}
