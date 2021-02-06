using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask whatIsCollision;
    public LayerMask whatIsSlippery;
    [SerializeField] Transform movePos;
    public bool notMovable;
    [SerializeField] public bool rockMoving;
    PlayerMovement player;

    AudioSource audio;
    [SerializeField] AudioClip slideRock;
    [SerializeField] float minimumRange = 0;
    Collider2D nextTileIsCollision;
    Collider2D isOnSlipperyGround;


    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        // Transform is not parented to player anymore
        movePos.parent = null;

        // No rock is moving
        rockMoving = false;

        // Check if rock can move
        notMovable = false;

        audio = gameObject.GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerMovement>();

        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveRock();
        checkIfMovable();
    }
    
    /// <summary>
    /// Check if rock is movable. Rock is movable only if it stands on slippery ground
    /// </summary>
    private void checkIfMovable()
    {
        if (Physics2D.OverlapCircle(movePos.position, 0f, whatIsSlippery))
        {
            notMovable = false;
        }
    }

    /// <summary>
    /// Starts moving the position the rock has to follow
    /// </summary>
    /// <param name="axis"></param>
    public void PushRock(string axis)
    {

        // check if rock finished moving to new position
        if (Math.Abs(Vector3.Distance(movePos.position, transform.position)) < 0.5f)
        {
            if (axis == "Horizontal")
            {
                // check colliders
                if (!Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), minimumRange, whatIsCollision))
                {
                    // Trigger rock slide sound
                    if (!audio.isPlaying)
                    {
                        audio.clip = slideRock;
                        audio.Play();
                    }

                    // Set bool to check if animation should be horizontal or vertical
                    animator.SetBool("isVertical", false);

                    movePos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    player.GetComponent<Animator>().SetBool("push", true);
                }

                nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), minimumRange, whatIsCollision);
                isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0f, whatIsSlippery);
                // Rock slips in slippery layer
                while (isOnSlipperyGround && !nextTileIsCollision)
                {
                    movePos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), minimumRange, whatIsCollision);
                    isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0f, whatIsSlippery);
                }
            }
            // prevent diagonal movement
            else if (axis == "Vertical")
            {
                // check colliders
                if (!Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), minimumRange, whatIsCollision))
                {
                    // Trigger rock slide sound
                    if (!audio.isPlaying)
                    {
                        audio.clip = slideRock;
                        audio.Play();
                    }
                    
                    // Set bool to check if animation should be horizontal or vertical
                    animator.SetBool("isVertical", true);

                    movePos.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    player.GetComponent<Animator>().SetBool("push", true);

                    nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), minimumRange, whatIsCollision);
                    isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0.005f, whatIsSlippery);

                    // Rock slips in slippery layer
                    while (isOnSlipperyGround && !nextTileIsCollision)
                    {
                        movePos.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), minimumRange, whatIsCollision);
                        isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0.005f, whatIsSlippery);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Moves Rock to movePos Position
    /// </summary>
    private void MoveRock()
    {
        gameObject.transform.position = Vector3.MoveTowards(transform.position, movePos.position, moveSpeed * Time.deltaTime);

        //Check if rock has stopped moving
        if (Math.Abs(Vector3.Distance(movePos.position, transform.position)) > minimumRange)
        {
            rockMoving = true;

            // Set Rock Movement animation
            animator.SetBool("isPushed", true);

        }
        else
        {
            rockMoving = false;

            // Set back Rock Movement animation
            animator.SetBool("isPushed", false);
        }
    }
}
