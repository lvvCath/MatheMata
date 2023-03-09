using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectFunction : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Canvas")]
    public Canvas canvas;

    public GameObject container;

    private string TAG_IGNORE = "fruit";

    private RectTransform rectTrans;


    private void Start()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
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
        Instantiate(this, container.transform.position, Quaternion.identity, container.transform);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
}
