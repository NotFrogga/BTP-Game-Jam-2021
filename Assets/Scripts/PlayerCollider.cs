using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    public static bool hasKey;

    private void Start()
    {
        hasKey = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("key"))
        {
            hasKey = true;
            Destroy(collision.gameObject);
            Debug.Log("Key Obtained !");
        }
    }
}
