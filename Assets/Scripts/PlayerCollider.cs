using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [Header("Key to finish level parameter")]
    public static bool hasKey;

    [Header("Audio Settings")]
    public AudioSource playerColliderAudioSource;
    [SerializeField] AudioClip getKeyClip;

    FinishLevel finishGO;
    private void Start()
    {
        finishGO = FindObjectOfType<FinishLevel>();
        hasKey = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("key"))
        {
            PlayerAcquiredKey(collision);
        }
    }

    /// <summary>
    /// Actions to accomplish after player acquired key to finish level
    /// </summary>
    /// <param name="collision">collision gameobject which is key gameobject</param>
    private void PlayerAcquiredKey(Collider2D collision)
    {
        // save information that player has key
        hasKey = true;
        GameObject keyGO = collision.gameObject;

        // Dispose of the key
        Destroy(keyGO);

        TriggerGetKeyAudioClip();

        // Warn player that they can finish level
        finishGO.SetEndLevelToFullSprite();
    }

    /// <summary>
    /// Triggers Key Audio Clip
    /// </summary>
    private void TriggerGetKeyAudioClip()
    {
        // Trigger get key sound
        playerColliderAudioSource.clip = getKeyClip;
        playerColliderAudioSource.Play();
    }
}
