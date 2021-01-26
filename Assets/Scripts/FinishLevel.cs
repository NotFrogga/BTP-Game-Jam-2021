using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{

   
    /// <summary>
    /// Checks if level is finished. Level is finished when player has key and player touches the star
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player") && PlayerCollider.hasKey)
        {
            Debug.Log("Finished Level !");
        }
    }
}
