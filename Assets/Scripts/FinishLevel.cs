using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    AudioSource audio;
    SpriteRenderer sr;
    [SerializeField] AudioClip winClip;
    [SerializeField] Sprite fullStar;

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        audio = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Checks if level is finished. Level is finished when player has key and player touches the star
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player") && PlayerCollider.hasKey)
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.GetComponent<Animator>().SetBool("win", true);
            

            // Trigger finished level sound
            if (!audio.isPlaying)
            {
                audio.clip = winClip;
                audio.Play();
            }           
        }
    }

    /// <summary>
    /// Change sprite to full star after getting the key
    /// </summary>
    public void ChangeSpriteToFullStar()
    {
        sr.sprite = fullStar;
    }
}
