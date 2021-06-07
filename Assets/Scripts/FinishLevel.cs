using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTPGameJam;

public class FinishLevel : MonoBehaviour
{
    AudioSource finishLevelAudioSource;
    SpriteRenderer sr;
    [SerializeField] AudioClip winClip;
    [SerializeField] Sprite fullSprite;

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        finishLevelAudioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Checks if level is finished. Level is finished when player has key and player touches the star
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool playerFinishedLevel = collision.gameObject.CompareTag(Constants.Tags.PLAYER) && PlayerCollider.hasKey;

        if (playerFinishedLevel)
        {
            PlayerFinishLevel(collision);
        }
    }

    /// <summary>
    /// Triggers events to finish level
    /// </summary>
    /// <param name="collision">collision gameobject which is player gameobject</param>
    private void PlayerFinishLevel(Collider2D collision)
    {
        SetPlayerWinAnimation(collision);
        StopPlayerGuideToIgloo();
        TriggerLevelFinishAudioClip();
    }

    /// <summary>
    /// Sets player win animation
    /// </summary>
    /// <param name="collision">collision gameobject which is player gameobject</param>
    private static void SetPlayerWinAnimation(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        player.GetComponent<Animator>().SetBool(Constants.Animation.WIN, true);
    }

    /// <summary>
    /// Stops player guide to igloo
    /// In case igloo is on slippery tile
    /// </summary>
    private void StopPlayerGuideToIgloo()
    {
        GameObject playerMov = GameObject.FindGameObjectWithTag(Constants.Tags.PLAYER_GUIDE);
        playerMov.transform.position = transform.position;
    }

    /// <summary>
    /// Triggers level finished audio clip
    /// </summary>
    private void TriggerLevelFinishAudioClip()
    {
        // Trigger finished level sound
        if (!finishLevelAudioSource.isPlaying)
        {
            finishLevelAudioSource.clip = winClip;
            finishLevelAudioSource.Play();
        }
    }

    /// <summary>
    /// Change end sprite (igloo) to full sprite after getting the key (fish)
    /// </summary>
    public void SetEndLevelToFullSprite()
    {
        sr.sprite = fullSprite;
    }
}
