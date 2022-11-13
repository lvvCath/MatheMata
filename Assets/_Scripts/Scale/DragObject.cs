using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Canvas")]
    public Canvas canvas;

    private string TAG_IGNORE = "fruit";

    private RectTransform rectTrans;

    private float init_gravity;

    private void Start()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        init_gravity = gameObject.GetComponent<Rigidbody2D>().gravityScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TAG_IGNORE)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = init_gravity;
    }
}
