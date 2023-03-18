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
        Debug.Log("OBJ initial position: " + initialPosition);

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        // mousePos.z = gameObject.transform.position.z; // keep Z position the same
        // rectTrans.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Debug.Log("GameObject position: " + gameObject.transform.position);
        Debug.Log("Object1Slot position: " + object1Slot.transform.position);
        Debug.Log("Object2Slot position: " + object2Slot.transform.position);
        Debug.Log("Object3Slot position: " + object3Slot.transform.position);

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