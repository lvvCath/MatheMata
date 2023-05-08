using System.Collections.Generic;
using UnityEngine;

public class DeleteOnExitWithTag : MonoBehaviour
{
    public string objectTag = "fruit";

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider that left the trigger is the one you want to destroy
        if (other.CompareTag(objectTag))
        {
            // Destroy the object
            Destroy(other.gameObject);
        }
    }
}