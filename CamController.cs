using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float horizontalAngle;
    public float verticalAngle;
    public float rotationAngle;
    private Vector3 startAngle;

    Vector2 movementInput = Vector2.zero;
    float forwardVelocity = 0;
    float sidewaysVelocity = 0;

    public float movementSmoothing = 0.15f;

    public float camJump;

    Rigidbody rb;
    float jumpBob;
    float jumpforwardVelocity = 0;
    public float jumpbobSmoothing = 0.15f;
    private void Start()
    {
        startAngle = transform.localEulerAngles;
        rb = transform.parent.transform.parent.transform.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (CollectorManager.canMove)
        {
            // dampens the controller input of the camera
            movementInput.x = Mathf.SmoothDamp(movementInput.x, Input.GetAxisRaw("RightHorizontal"), ref forwardVelocity, movementSmoothing);
            movementInput.y = Mathf.SmoothDamp(movementInput.y, Input.GetAxisRaw("RightVertical"), ref sidewaysVelocity, movementSmoothing);

            if (rb)
            {
                // adds gravity to angle the camera up when the player jumps based on gravitation velocity
                jumpBob = Mathf.SmoothDamp(jumpBob, rb.velocity.y, ref jumpforwardVelocity, jumpbobSmoothing);
            }
            // updates the eulerangles rotation to the player camera
            Vector3 _temp = new Vector3((horizontalAngle * movementInput.y) + -(2 * jumpBob), verticalAngle * movementInput.x, rotationAngle * -CatMovement.movementInput.x);
            transform.localEulerAngles = startAngle + _temp;
        }
    }
}

