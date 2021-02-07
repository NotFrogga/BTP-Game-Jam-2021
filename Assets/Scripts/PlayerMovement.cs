using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.25f;
    public LayerMask whatIsCollision;
    public LayerMask whatIsSlippery;
    public LayerMask whatIsRock;

    GameManagement gameManagement;
    [SerializeField] Transform movePos;

    [SerializeField] float minimumRange = 0;

    Animator animator;
    Collider2D nextTileIsCollision;
    Collider2D isOnSlipperyGround;

    AudioSource audio;
    public AudioClip slideClip;
    

    // Start is called before the first frame update
    void Start()
    {
        // Transform is not parented to player anymore
        movePos.parent = null;

        animator = gameObject.GetComponent<Animator>();
        audio = gameObject.GetComponent<AudioSource>();

        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
    }

    // Update is called once per frame
    void Update()
    {

        // Cannot move player if one rock is moving
        if (gameManagement != null && !gameManagement.CheckIfRocksAreMoving())
        {
            // if insided if statement, no rocks are moving so no pushing
            animator.SetBool("push", false);
            MoveCharacter();
        }
    }
    
    /// <summary>
    /// Character tile movement
    /// </summary>
    private void MoveCharacter()
    {
        gameObject.transform.position = Vector3.MoveTowards(transform.position, movePos.position, moveSpeed * Time.deltaTime);
        
        // check if player finished moving to new position
        if (Math.Abs(Vector3.Distance(movePos.position, transform.position)) < 0.05f)
        {    
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                MoveRock("Horizontal");

                // Setting animation variables
                animator.SetInteger("horizontal", (int)Input.GetAxisRaw("Horizontal"));
                animator.SetInteger("vertical", 0);

                // flip sprite if player turns left
                if (Input.GetAxisRaw("Horizontal") == -1)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                // check colliders
                if (!Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), minimumRange, whatIsCollision))
                {


                    // Trigger slide sound effect
                    if (!audio.isPlaying)
                    {
                        audio.clip = slideClip;
                        audio.Play();
                    }


                    movePos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);                    
                }

                nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), minimumRange, whatIsCollision);
                isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0f, whatIsSlippery);
                // Player slips in slippery layer
                while (isOnSlipperyGround && !nextTileIsCollision)
                {
                    // Stop when play reaches end of level
                    if (!animator.GetBool("win"))
                    {
                        movePos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), minimumRange, whatIsCollision);
                        isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0f, whatIsSlippery);
                    }

                }


            } 
            // prevent diagonal movement
            else if (Math.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                MoveRock("Vertical");

                // Setting animation variables
                animator.SetInteger("vertical", (int)Input.GetAxisRaw("Vertical"));
                animator.SetInteger("horizontal", 0);

                // check colliders
                if (!Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), minimumRange, whatIsCollision))
                {

                    // Trigger slide sound effect
                    if (!audio.isPlaying)
                    {
                        audio.clip = slideClip;
                        audio.Play();
                    }

                    movePos.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

                    nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), minimumRange, whatIsCollision);
                    isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0.005f, whatIsSlippery);

                    // Player slips in slippery layer
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
    /// Mechanic to move rock if movable
    /// </summary>
    /// <param name="axis"></param>
    private void MoveRock(string axis)
    {
        if (axis == "Horizontal")
        {
            Collider2D collider = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), minimumRange, whatIsRock);
            if (collider != null)
            {
                RockMovement rock = collider.gameObject.GetComponent<RockMovement>();

                if (!rock.notMovable)
                {
                    rock.PushRock(axis);
                }
            }            
        }
        else if (axis == "Vertical")
        {
            Collider2D collider = Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), minimumRange, whatIsRock);

            if (collider != null)
            {
                RockMovement rock = collider.gameObject.GetComponent<RockMovement>();

                if (!rock.notMovable)
                {
                    rock.PushRock(axis);
                }
            }
        }
    }

    // Triggers method in game management to change scene
    public void TriggerChangerScene()
    {
       GameManagement gm =  GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        gm.changeScene = true;
    }
}
