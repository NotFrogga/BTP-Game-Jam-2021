using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManagement gameManagement;
    Animator animator;

    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    [Header("Audio")]
    AudioSource playerAudioSource;
    public AudioClip slideClip;

    [Header("Movement Properties")]
    public float moveSpeed = 1.25f;
    [SerializeField] Transform movementGuideTf;
    [SerializeField] float minimumRange = 0;
    float minDistanceGuideToPlayer = 0.05f;
    Collider2D nextTileIsCollision;
    Collider2D isOnSlipperyGround;

    [Header("Movement LayerMask")]
    public LayerMask whatIsCollision;
    public LayerMask whatIsSlippery;
    public LayerMask whatIsRock;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();

        // Transform is not parented to player anymore
        movementGuideTf.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Cannot move player if one rock is moving
        if (gameManagement != null && !gameManagement.CheckIfRocksAreMoving())
        {
            // if insided if statement, no rocks are moving so no pushing
            animator.SetBool("push", false);
            PlayerFollowGuide();
            GuideMovement();
        }
    }

    #region Player Movement

    /// <summary>
    /// Make player follow mouvement guide gameobject
    /// </summary>
    private void PlayerFollowGuide()
    {
        Vector3 playerPos = transform.position;
        Vector3 movementGuidePos = movementGuideTf.position;

        // Player follows child guide
        transform.position = Vector3.MoveTowards(playerPos, movementGuidePos, moveSpeed * Time.deltaTime);
    }
    #endregion

    #region Guide Movement
    /// <summary>
    /// Character tile movement
    /// </summary>
    private void GuideMovement()
    {
        Vector3 playerPos = transform.position;
        Vector3 movementGuidePos = movementGuideTf.position;
        Vector3 direction;
        string DIRECTION;

        // check if player finished moving to new position
        if (Vector3.Distance(movementGuidePos, playerPos) < minDistanceGuideToPlayer)
        {
            if (Math.Abs(Input.GetAxisRaw(HORIZONTAL)) == 1f)
            {
                direction = new Vector3(Input.GetAxisRaw(HORIZONTAL), 0f, 0f);
                DIRECTION = HORIZONTAL;
            }
            // prevent diagonal movement
            else
            {
                direction = new Vector3(0f, Input.GetAxisRaw(VERTICAL), 0f);
                DIRECTION = VERTICAL;
            }

            if (direction != Vector3.zero)
            {
                MoveRock(direction);
                SetPlayerAnimation(DIRECTION);
                MoveGuideToPos(direction, false);
            }
        }
    }

    /// <summary>
    /// Move guide to new position
    /// </summary>
    /// <param name="direction">direction of the new position</param>
    /// <param name="checkSlippery">check if guide position is on slippery tile</param>
    private void MoveGuideToPos(Vector3 direction, bool checkSlippery)
    {
        Vector3 nextTilePos = movementGuideTf.position + direction;
        Collider2D nextTileStopsPlayer = Physics2D.OverlapCircle(nextTilePos, minimumRange, whatIsCollision);

        // Move Guide to the first tile
        if (!checkSlippery)
        {
            if (!nextTileStopsPlayer)
            {
                TriggerSlipClip();
                movementGuideTf.position = nextTilePos;
            }

            MoveGuideToPos(direction, true);
        }
        // Handle Guide movement if guide tile is slippery
        else
        {
            nextTileIsCollision = Physics2D.OverlapCircle(movementGuideTf.position + direction, minimumRange, whatIsCollision);
            isOnSlipperyGround = Physics2D.OverlapCircle(movementGuideTf.position, 0.005f, whatIsSlippery);

            // Player slips in slippery layer
            while (isOnSlipperyGround && !nextTileIsCollision)
            {
                movementGuideTf.position += direction;
                nextTileIsCollision = Physics2D.OverlapCircle(movementGuideTf.position + direction, minimumRange, whatIsCollision);
                isOnSlipperyGround = Physics2D.OverlapCircle(movementGuideTf.position, 0.005f, whatIsSlippery);
            }
        }
    }

    /// <summary>
    /// Mechanic to move rock if movable
    /// </summary>
    /// <param name="axis">Player input movement axis</param>
    private void MoveRock(Vector3 direction)
    {
        Vector3 nextTilePos = movementGuideTf.position + direction;
        Collider2D nextTileContainsRock = Physics2D.OverlapCircle(nextTilePos, minimumRange, whatIsRock); ;
        if (nextTileContainsRock != null)
        {
            RockMovement rock = nextTileContainsRock.gameObject.GetComponent<RockMovement>();
            if (!rock.isOnSlipperyTile)
            {
                rock.MoveRockGuide(direction, false);
            }
        }
    }
    #endregion

    #region Utils
    /// <summary>
    /// Triggers method in game management to change scene
    /// </summary>
    public void TriggerChangerScene()
    {
        GameManagement gm = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        gm.levelFinished = true;
    }

    /// <summary>
    /// Triggers Player slip audio clip
    /// </summary>
    private void TriggerSlipClip()
    {
        // Trigger slide sound effect
        if (!playerAudioSource.isPlaying)
        {
            playerAudioSource.clip = slideClip;
            playerAudioSource.Play();
        }
    }

    /// <summary>
    /// Sets player animation according to its input
    /// </summary>
    /// <param name="axis">Player input axis</param>
    private void SetPlayerAnimation(string axis)
    {
        if (axis == HORIZONTAL)
        {
            // Setting animation variables
            animator.SetInteger("horizontal", (int)Input.GetAxisRaw(HORIZONTAL));
            animator.SetInteger("vertical", 0);

            // flip sprite if player turns left
            if (Input.GetAxisRaw(HORIZONTAL) == -1)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (axis == VERTICAL)
        {
            // Setting animation variables
            animator.SetInteger("vertical", (int)Input.GetAxisRaw(VERTICAL));
            animator.SetInteger("horizontal", 0);
        }
    }
    #endregion
}
