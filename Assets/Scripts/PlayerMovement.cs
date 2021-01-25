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
    [SerializeField] Transform movePos;
    
    Collider2D nextTileIsCollision;
    Collider2D isOnSlipperyGround;

    // Start is called before the first frame update
    void Start()
    {
        // Transform is not parented to player anymore
        movePos.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Cannot move player if one rock is moving
        if (!RockMovement.rockMoving)
        {
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
        if (Math.Abs(Vector3.Distance(movePos.position, transform.position)) < 0.5f)
        {
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                MoveRock("Horizontal");

                // check colliders
                if (!Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.005f, whatIsCollision))
                {
                    movePos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);                    
                }

                nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.005f, whatIsCollision);
                isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0f, whatIsSlippery);
                // Player slips in slippery layer
                while (isOnSlipperyGround && !nextTileIsCollision)
                {
                   movePos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                   nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.005f, whatIsCollision);
                   isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0f, whatIsSlippery);
                }


            } 
            // prevent diagonal movement
            else if (Math.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                MoveRock("Vertical");

                // check colliders
                if (!Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.005f, whatIsCollision))
                {
                    movePos.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

                    nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.005f, whatIsCollision);
                    isOnSlipperyGround = Physics2D.OverlapCircle(movePos.position, 0.005f, whatIsSlippery);

                    // Player slips in slippery layer
                    while (isOnSlipperyGround && !nextTileIsCollision)
                    {
                        movePos.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        nextTileIsCollision = Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.005f, whatIsCollision);
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
            Collider2D collider = Physics2D.OverlapCircle(movePos.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.005f, whatIsRock);
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
            Collider2D collider = Physics2D.OverlapCircle(movePos.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.005f, whatIsRock);

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
}
