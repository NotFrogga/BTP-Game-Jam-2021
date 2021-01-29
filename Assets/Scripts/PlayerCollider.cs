using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    public static bool hasKey;

    public AudioSource audio;
    [SerializeField] AudioClip getKeyClip;
    FinishLevel finishGO;
    private void Start()
    {
        finishGO = GameObject.FindObjectOfType<FinishLevel>();
        hasKey = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("key"))
        {
            hasKey = true;
            Destroy(collision.gameObject);

            // Trigger get key sound
            audio.clip = getKeyClip;
            audio.Play();

            finishGO.ChangeSpriteToFullStar();
        }
    }
}
