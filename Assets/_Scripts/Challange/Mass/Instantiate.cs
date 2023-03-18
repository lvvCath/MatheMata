using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    public GameObject objDetected;

    void OnTriggerEnter2D (Collider2D collide)
    {
        Rigidbody2D rb = objDetected.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void OnTriggerExit2D (Collider2D collide)
    {
        Rigidbody2D rb = objDetected.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
    }
}
