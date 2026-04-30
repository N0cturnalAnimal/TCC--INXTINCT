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

    public Transform holdPoint;
    private GameObject nextToHold;
    public Action<float> AttentionBringing;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ManageState();

        if (Input.GetAxis("Horizontal") != 0f && state != PlayerEnum.Climbing)
        {
            theRB.linearVelocity = new Vector2(speeds[(int)state] * Input.GetAxis("Horizontal"), theRB.linearVelocity.y);

            if (Input.GetAxis("Horizontal") < 0 && transform.localScale.x > 0 || Input.GetAxis("Horizontal") > 0 && transform.localScale.x < 0)
                transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

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
        canJump = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);

        if (!canJump || state == PlayerEnum.Climbing || state == PlayerEnum.Carrying)
            return;

        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
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
            theRB.gravityScale = 1;
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

        state = PlayerEnum.Carrying;
        nextToHold.transform.position = holdPoint.position;
        nextToHold.transform.SetParent(holdPoint, true);
        UITips.Instance.ShowTip();
        Rigidbody2D rig = nextToHold.GetComponent<Rigidbody2D>();
        rig.angularVelocity = 0f;
        rig.bodyType = RigidbodyType2D.Kinematic;
    }

    void ExitCarryMode()
    {
        state = PlayerEnum.Normal;
        nextToHold.transform.SetParent(null, true);
        Rigidbody2D rig = nextToHold.GetComponent<Rigidbody2D>();
        rig.bodyType = RigidbodyType2D.Dynamic;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Climbable"))
        {
            canClimb = true;
        }

        if (other.CompareTag("Carriable"))
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

        if (Input.GetKey(KeyCode.LeftShift) && state == PlayerEnum.Normal || Input.GetKey(KeyCode.LeftShift) && state == PlayerEnum.Running)
        {
            state = PlayerEnum.Running;
            return;
        }

        if (Input.GetKey(KeyCode.LeftControl) && state != PlayerEnum.Climbing)
        {
            state = PlayerEnum.Crouching;
            return;
        }

        state = PlayerEnum.Normal;
    }

    void AttentionCaller()
    {
        float attention = 0;

        if (Input.GetAxis("Horizontal") == 0)
            return;

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