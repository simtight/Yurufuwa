using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGround : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SendMessageUpwards("HandleIsGroundTriggerTrue", collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SendMessageUpwards("HandleIsGroundTriggerFalse", collision);
    }
}
