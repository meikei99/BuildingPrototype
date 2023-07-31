using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float minMovementSpeed = 0.1f;

    private CharacterController characterController;
    private Animator ch_animator;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        ch_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CharacterMove();
    }

    private void CharacterMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Normalize the input vector.
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized; 

        // Apply movement in the camera's forward and right direction to ensure isometric movement.
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0f; // Ignore any vertical movement.

        characterController.Move(movement * movementSpeed * Time.deltaTime);

        // Calculate the player's speed
        float playerSpeed = movement.magnitude;

        // If the player is moving, set the "Move" parameter to true, otherwise false.
        bool isMoving = playerSpeed > minMovementSpeed;
        ch_animator.SetBool("Move", isMoving);

        if (playerSpeed > minMovementSpeed)
        {
            Vector3 direct = Vector3.RotateTowards(transform.forward, movement, movementSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
    }
}
