using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksSetParent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Rift")
        {
            transform.SetParent(collision.gameObject.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Rift")
        {
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
        }
    }
}
