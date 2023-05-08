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

    Vector3 initialPosition;

    

    private void Start()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        if(container.transform.position == initialPosition)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
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
        initialPosition = gameObject.transform.position;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // if (initialPosition == container.transform.position)
        // {
        //     Instantiate(this, container.transform.position, Quaternion.identity, container.transform);

        //     this.GetComponent<ObjectFunction>().enabled = false;
        //     gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        // }

        Instantiate(this, container.transform.position, Quaternion.identity, container.transform);
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponent<ObjectFunction>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
}
