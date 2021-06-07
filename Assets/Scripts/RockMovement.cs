using System;
using UnityEngine;
using BTPGameJam;

public class RockMovement : MonoBehaviour
{
    PlayerMovement player;
    Animator animator;
    Collider2D isOnSlipperyGround;

    [Header("Audio")]
    AudioSource rockAudioSource;
    [SerializeField] AudioClip slideRock;

    [Header("Movement Properties")]
    [SerializeField] float minimumDistance = 0;
    [SerializeField] Transform movementGuideTf;
    public bool rockMoving;
    public float moveSpeed = 5f;
    public bool isOnSlipperyTile;


    [Header("Movement LayerMask")]
    public LayerMask whatIsCollision;
    public LayerMask whatIsSlippery;

    // Start is called before the first frame update
    void Start()
    {
        // Transform is not parented to player anymore
        movementGuideTf.parent = null;
        // No rock is moving
        rockMoving = false;
        // Check if rock can move
        isOnSlipperyTile = false;

        rockAudioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag(Constants.Tags.PLAYER).GetComponent<PlayerMovement>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkIfMovable();
        MoveRock();
    }

    #region Move Rock
    /// <summary>
    /// Moves Rock to movementGuideTf Position
    /// </summary>
    private void MoveRock()
    {
        gameObject.transform.position = Vector3.MoveTowards(transform.position, movementGuideTf.position, moveSpeed * Time.deltaTime);
        float guideToRockDistance = Vector3.Distance(movementGuideTf.position, transform.position);

        //Check if rock has stopped moving
        if (guideToRockDistance > minimumDistance)
        {
            rockMoving = true;

            // Set Rock Movement animation
            animator.SetBool(Constants.Animation.IS_PUSHED, true);
        }
        else
        {
            rockMoving = false;

            // Set back Rock Movement animation
            animator.SetBool(Constants.Animation.IS_PUSHED, false);
        }
    }
    #endregion

    #region Move Rock Guide
    /// <summary>
    /// Starts moving the position the rock has to follow
    /// </summary>
    /// <param name="direction">Direction vector of rock movement</param>
    /// <param name="checkSlippery">Check if guide position is on slippery tile</param>
    public void MoveRockGuide(Vector3 direction, bool checkSlippery)
    {
        Vector3 nextTilePos = movementGuideTf.position + direction;
        Collider2D nextTileStopsRock = Physics2D.OverlapCircle(nextTilePos, minimumDistance, whatIsCollision);
        if (!checkSlippery)
        {
            if (!nextTileStopsRock)
            {
                TriggerRockAudioClip();
                SetAnimationAxis(direction);
                movementGuideTf.position = nextTilePos;
                player.GetComponent<Animator>().SetBool(Constants.Animation.PUSH, true);
                MoveRockGuide(direction, true);
            }
        }
        else
        {
            isOnSlipperyGround = Physics2D.OverlapCircle(movementGuideTf.position, 0f, whatIsSlippery);
            
            // Rock slips in slippery layer
            while (isOnSlipperyGround && !nextTileStopsRock)
            {
                movementGuideTf.position = nextTilePos;
                nextTilePos = movementGuideTf.position + direction;
                nextTileStopsRock = Physics2D.OverlapCircle(nextTilePos, minimumDistance, whatIsCollision);
                isOnSlipperyGround = Physics2D.OverlapCircle(movementGuideTf.position, 0f, whatIsSlippery);
            }
        }
    }
    #endregion

    #region Utils
    /// <summary>
    /// Set bool to check if animation should be horizontal or vertical
    /// </summary>
    /// <param name="direction">Direction vector of rock movement</param>
    private void SetAnimationAxis(Vector3 direction)
    {
        if (direction.y == 0)
        {
           
            animator.SetBool(Constants.Animation.IS_VERTICAL, false);
        }
        else
        {
            animator.SetBool(Constants.Animation.IS_VERTICAL, true);
        }
    }

    /// <summary>
    /// Triggers Rock slide audio clip
    /// </summary>
    private void TriggerRockAudioClip()
    {
        if (!rockAudioSource.isPlaying)
        {
            rockAudioSource.clip = slideRock;
            rockAudioSource.Play();
        }
    }



    /// <summary>
    /// Check if rock is on slippery tile
    /// </summary>
    private void checkIfMovable()
    {
        if (Physics2D.OverlapCircle(movementGuideTf.position, 0f, whatIsSlippery))
        {
            isOnSlipperyTile = false;
        }
    }
    #endregion
}
