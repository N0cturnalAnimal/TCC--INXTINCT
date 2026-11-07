using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerEnum state;
    public float[] speeds;
    private Rigidbody2D theRB;
    public float jumpForce;
    public float rayLength = 0.3f;
    public LayerMask groundLayer;
    public bool canClimb;
    public bool canCarry;
    public bool canJump;
    private bool carrying;
    public Transform groundCheck;
    public Transform inclineCheckB;
    public Transform inclineCheckF;
    
    [Header("Inclinação")]
    public float inclineSmooth = 8f;
    public float maxInclineAngle = 35f;
    public float inclineRayLength = 0.8f;
    private Transform visual;

    public Transform holdPoint;
    private GameObject nextToHold;
    public Action<float> AttentionBringing;

    public AudioSource meowSource;
    public AudioSource footstepAudio;

    private Animator animator;

    public StaminaWheel staminaWheel;

    public void Meow()
    {
        meowSource.Play();
    }

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        visual = animator.transform;
        carrying = false;
    }

    void Update()
    {
        ManageState();
        staminaWheel.UpdateStamina(
        state == PlayerEnum.Running && Input.GetAxis("Horizontal") != 0f
        );
        CheckIncline();

        animator.SetBool("isRunning", Input.GetAxis("Horizontal") != 0f && state != PlayerEnum.Crouching && state != PlayerEnum.Climbing && state != PlayerEnum.Carrying);

        animator.SetBool("isCrouching", state == PlayerEnum.Crouching);

        if (Input.GetKeyDown(KeyCode.M))
            Meow();

        if (Input.GetAxis("Horizontal") != 0f && state != PlayerEnum.Climbing)
        {
            theRB.linearVelocity = new Vector2(speeds[(int)state] * Input.GetAxis("Horizontal"), theRB.linearVelocity.y);
            footstepAudio.Play();

            if (Input.GetAxis("Horizontal") < 0 && transform.localScale.x > 0 || Input.GetAxis("Horizontal") > 0 && transform.localScale.x < 0)
                transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else
            footstepAudio.Stop();

        if (state == PlayerEnum.Climbing)
            theRB.linearVelocity = new Vector2(speeds[(int)state] * Input.GetAxis("Horizontal"), speeds[(int)state] * Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump"))
            Jump();

        if (Input.GetKeyDown(KeyCode.E))
            EnterCarryMode();

        if (Input.GetAxis("Vertical") != 0f && canClimb && state != PlayerEnum.Carrying)
            EnterClimbMode();

        AttentionCaller();
    }

    void Jump()
    {
        canJump = Physics2D.Raycast(groundCheck.position, Vector2.down, rayLength, groundLayer);

        if (!canJump || state == PlayerEnum.Climbing || state == PlayerEnum.Carrying)
            return;
        
        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
    }

    

void CheckIncline()
{
    if (state == PlayerEnum.Climbing)
        return;

    RaycastHit2D hitB = Physics2D.Raycast(
    inclineCheckB.position,
    Vector2.down,
    inclineRayLength,
    groundLayer
);

RaycastHit2D hitF = Physics2D.Raycast(
    inclineCheckF.position,
    Vector2.down,
    inclineRayLength,
    groundLayer
);
Debug.Log(
    $"Sensor B: {hitB.collider != null} | " +
    $"Sensor F: {hitF.collider != null}"
);

    float targetAngle = 0f;

    if (hitB.collider != null && hitF.collider != null)
    {
        float deltaX = hitF.point.x - hitB.point.x;
        float deltaY = hitF.point.y - hitB.point.y;

        if (Mathf.Abs(deltaX) > 0.01f)
        {
            targetAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

            // Corrige a direção quando a onça está virada para a esquerda.
            if (transform.localScale.x < 0)
                targetAngle *= -1;

            targetAngle = Mathf.Clamp(
                targetAngle,
                -maxInclineAngle,
                maxInclineAngle
            );
        }
    }

    float currentAngle = transform.eulerAngles.z;

    float newAngle = Mathf.LerpAngle(
        currentAngle,
        targetAngle,
        inclineSmooth * Time.deltaTime
    );

    transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
}

    void OnDrawGizmos()
    {
        if (inclineCheckB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
            inclineCheckB.position,
            inclineCheckB.position + Vector3.down * rayLength
            );
        }

        if (inclineCheckF != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                inclineCheckF.position,
                inclineCheckF.position + Vector3.down * rayLength
            );
        }
    }


    void EnterClimbMode()
    {
        if (!canClimb)
            return;

        theRB.gravityScale = 0;
        state = PlayerEnum.Climbing;
        UITips.Instance.ShowTip();
    }

    public void ExitClimbMode()
    {
        canClimb = false;

        if (state == PlayerEnum.Climbing)
        {
            state = PlayerEnum.Normal;
            theRB.gravityScale = 2;
        }
    }

    void EnterCarryMode()
    {
        if (state == PlayerEnum.Carrying)
        {
            ExitCarryMode();
            return;
        }

        if (!canCarry)
            return;

        carrying = true;
        state = PlayerEnum.Carrying;
        nextToHold.transform.position = holdPoint.position;
        nextToHold.transform.SetParent(holdPoint, true);
        UITips.Instance.ShowTip();
        Rigidbody2D rig = nextToHold.GetComponent<Rigidbody2D>();
        rig.angularVelocity = 0f;
        rig.bodyType = RigidbodyType2D.Kinematic;
        foreach (Transform child in nextToHold.transform)
        {
            Collider2D col = child.GetComponent<Collider2D>();
            col.enabled = false;
        }
    }

    void ExitCarryMode()
    {
        state = PlayerEnum.Normal;
        nextToHold.transform.SetParent(null, true);
        Rigidbody2D rig = nextToHold.GetComponent<Rigidbody2D>();
        rig.bodyType = RigidbodyType2D.Dynamic;
        carrying = false;
        foreach (Transform child in nextToHold.transform)
        {
            Collider2D col = child.GetComponent<Collider2D>();
            col.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Climbable"))
        {
            canClimb = true;
        }

        if (other.CompareTag("Carriable") && carrying == false)
        {
            nextToHold = other.gameObject;
            canCarry = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Climbable"))
        {
            ExitClimbMode();
        }

        if (other.CompareTag("Carriable"))
        {
            canCarry = false;
        }
    }

    
void ManageState()
{
    if (state == PlayerEnum.Climbing || state == PlayerEnum.Carrying)
        return;

    bool wantsToRun = Input.GetKey(KeyCode.LeftShift);

    if (wantsToRun && staminaWheel.CanRun)
    {
        state = PlayerEnum.Running;
        return;
    }

    if (Input.GetKey(KeyCode.LeftControl))
    {
        state = PlayerEnum.Crouching;
        return;
    }

    state = PlayerEnum.Normal;
}


    void AttentionCaller()
    {
        float attention = 0f;

        //if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && theRB.linearVelocity.magnitude != 0)
            //return;

        switch (state)
        {
            case PlayerEnum.Normal:
                attention = 1;

                AttentionBringing.Invoke(attention);
                break;

            case PlayerEnum.Running:
                attention = 6;

                AttentionBringing.Invoke(attention);
                break;

            case PlayerEnum.Crouching:
                attention = 0.5f;

                AttentionBringing.Invoke(attention);
                break;

            case PlayerEnum.Climbing:
                attention = 6;

                AttentionBringing.Invoke(attention);
                break;

            case PlayerEnum.Carrying:
                attention = 4;

                AttentionBringing.Invoke(attention);
                break;

            default:
                AttentionBringing.Invoke(attention);
                break;
        }
    }
}