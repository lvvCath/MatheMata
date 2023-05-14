using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CapacityDragnDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Canvas")]
    public Canvas canvas;
    public CanvasGroup canvasGroup;

    [Header("Objects")]
    public GameObject object1;
    public GameObject object2;

    [Header("Slot")]
    public GameObject object1Slot;
    public GameObject object2Slot;
    public GameObject object3Slot;

    private RectTransform rectTrans;

    Vector3 initialPosition;

    private void Start()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = gameObject.transform.position;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if ((gameObject.transform.position == object1Slot.transform.position) ||
            (gameObject.transform.position == object2Slot.transform.position) ||
            (gameObject.transform.position == object3Slot.transform.position)
            )
        {
            if (gameObject.transform.position == object1.transform.position)
            {
                object1.transform.position = initialPosition;
            }


            if (gameObject.transform.position == object2.transform.position)
            {
                object2.transform.position = initialPosition;
            }

            initialPosition = gameObject.transform.position;

        }
        else
        {
            gameObject.transform.position = initialPosition;
        }

    }
}