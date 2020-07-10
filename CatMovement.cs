using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CatMovement : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] private float speed = default;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private float climbingSpeed = default;
    [SerializeField] private AnimationCurve walkCurve = default;
    [SerializeField] private Animator walkAnim = default;
    [Header("Jumping")]
    [SerializeField] public static bool canJump = default;
    [SerializeField] private float jumpForce = default;
    [SerializeField] private float canJumpSeconds = 1.0f;
    [SerializeField] private float chargedJumpMaxHeight = 2f;
    [SerializeField] private Volume chargeEffect = default;
    private float jumptimestamp = 0.0f;
    private float jumpPower = 0f;
    [Header("Climbing")]
    [SerializeField] private float modifier = default;

    private bool isClimbing;
    private Rigidbody rb;
    private Vector3 startPosition;


    public static Vector2 movementInput = Vector2.zero;
    float forwardVelocity = 0;
    float sidewaysVelocity = 0;

    public float movementSmoothing = 0.15f;
    public float rotationSmoothing = 0.15f;
    // Start is called before the first frame update
    private void Start()
    {
        jumptimestamp = Time.time + 0.0f;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        //Physics.IgnoreLayerCollision(0, 9);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (CollectorManager.canMove)
        {
            if (!LedgeChecker.IsGrabbingLedge)
            {
                Walk();
                Jump();
            }
            Grab();
        }
    }

    private bool CheckIfOutsideBounds()
    {
        return transform.position.y < -10;
    }

    private void Respawn()
    {
        transform.position = startPosition;
    }

    private void Walk()
    {
        if (CheckIfOutsideBounds())
        {
            Respawn();
        }
        //dampens player input
        movementInput.x = Mathf.SmoothDamp(movementInput.x, Input.GetAxisRaw("Horizontal"), ref forwardVelocity, rotationSmoothing);
        movementInput.y = Mathf.SmoothDamp(movementInput.y, Input.GetAxisRaw("Vertical"), ref sidewaysVelocity, movementSmoothing);

        float _movementSpeed = walkCurve.Evaluate(movementInput.y);
        if (!isClimbing)
        {
            rb.MovePosition(transform.position + transform.forward * _movementSpeed * speed);
        }
        else
        {
            while (isClimbing && modifier > .6f)
            {
                modifier -= .01f;
            }
            rb.MovePosition(transform.position + transform.forward * ((_movementSpeed * speed) * modifier));
        }
        //walkAnim.speed = Input.GetAxis("Vertical");
        //transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed);

        Vector3 _tempRot = transform.eulerAngles;
        _tempRot.y = _tempRot.y + (movementInput.x * rotationSpeed);

        transform.eulerAngles = _tempRot;
    }

    private void Jump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up * 1.5f), out hit, 1))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        
        if (Input.GetButton("A Button") && canJump)
        {
            // Jump power value check
            jumpPower += 0.03f;
            if(jumpPower > 1f)
            {
                // charge jump effect enable
                DOTween.To(() => chargeEffect.weight, x => chargeEffect.weight = x, 1f, 1f).SetEase(Ease.Unset);
                
            }
        }
        
        if(Input.GetButtonUp("A Button") && canJump)
        {
            // checks if can jump and if jump button has been held long enough
            if (Time.time > jumptimestamp && jumpPower <= 1f)
            {
                float _movementSpeed = walkCurve.Evaluate(movementInput.y);


                rb.AddForce(transform.up * jumpForce * 1); // keer anim curve speed voor buildup (experiment hier ff mee)

                jumptimestamp = Time.time + canJumpSeconds;
                jumpPower = 0f;
                // charge jump effect disable
                DOTween.Clear();
                DOTween.To(() => chargeEffect.weight, x => chargeEffect.weight = x, 0f, 1f).SetEase(Ease.Unset);
            }
            else if(Time.time > jumptimestamp)
            {
            // duplicated code, kan anders d.m.v. een functie te maken maar snelle oplossing wegens tijdsnood
                jumpPower = Mathf.Clamp(jumpPower, 0f, chargedJumpMaxHeight);
                float _movementSpeed = walkCurve.Evaluate(movementInput.y);


                rb.AddForce(transform.up * jumpForce * jumpPower); // keer anim curve speed voor buildup (experiment hier ff mee)

                jumptimestamp = Time.time + canJumpSeconds;
                jumpPower = 0f;
                // charge jump effect disable
                DOTween.Clear();
                DOTween.To(() => chargeEffect.weight, x => chargeEffect.weight = x, 0f, 1f).SetEase(Ease.Unset);
            }
        }
    }
    // function removed
    private void Grab()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(LedgeChecker.IsGrabbingLedge == true)
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Climable")
        {
            isClimbing = true;

            Vector3 _tempRot = transform.eulerAngles;
            _tempRot.x = _tempRot.x - 90;
            transform.localEulerAngles = _tempRot;

            speed += climbingSpeed;
            modifier = 1f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Climable")
        {
            isClimbing = false;

            Vector3 _tempRot = transform.eulerAngles;
            _tempRot.x = _tempRot.x + 90;
            transform.localEulerAngles = _tempRot;

            speed -= climbingSpeed;
        }
    }
}
