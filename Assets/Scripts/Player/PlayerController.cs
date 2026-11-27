
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerEnum state;
    public float[] speeds;
    private Rigidbody2D theRB;
    public float jumpForce;
    public float rayLength = 1.0f;
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
    public float inclineRayLength = 1.2f;
    private Transform visual;

    public Transform holdPoint;
    private GameObject nextToHold;
    public Action<float> AttentionBringing;

    public AudioSource meowSource;
    private PlayerAudio playerAudio;

    private Animator animator;

    public StaminaWheel staminaWheel;

    [Header("Konami Cheat")]
    public float godModeSpeed = 23f;

    private bool godMode = false;
    private float originalGravityScale;
    private int konamiIndex = 0;

    private readonly KeyCode[] konamiCode =
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A,
        KeyCode.Return
    };

    public void Meow()
    {
        meowSource.Play();
    }

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        originalGravityScale = theRB.gravityScale;

        playerAudio = GetComponent<PlayerAudio>();

        visual = transform.Find("CutsceneOffset/Visual");

        if (visual == null)
        {
            return;
        }

        animator = visual.GetComponent<Animator>();

        if (animator == null)
        {
            return;
        }

        carrying = false;
    }

    void Update()
    {
        CheckKonamiCode();

        if (godMode)
        {
            HandleGodMode();
            return;
        }

        ManageState();

        staminaWheel.UpdateStamina(
            state == PlayerEnum.Running && Input.GetAxis("Horizontal") != 0f
        );

        CheckIncline();

        bool isMovingOnTree =
            state == PlayerEnum.Climbing &&
            (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f ||
             Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f);

        // Detecta se a onça está no chão
        canJump = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            rayLength,
            groundLayer
        );

        animator.SetBool("isGrounded", canJump);

        float verticalSpeed = theRB.linearVelocity.y;

        bool canUseJumpAnimation =
            state != PlayerEnum.Climbing &&
            state != PlayerEnum.Carrying;

        // Animações de movimento aéreo
        bool isJumping = !canJump && verticalSpeed > 0.01f;
        bool isFalling = !canJump && verticalSpeed < -0.01f;

        animator.SetBool("isJumping", canUseJumpAnimation && isJumping);
        animator.SetBool("isFalling", canUseJumpAnimation && isFalling);

        // Animações normais
        animator.SetBool("isRunning",
            canJump &&
            Input.GetAxis("Horizontal") != 0f &&
            state != PlayerEnum.Crouching &&
            state != PlayerEnum.Climbing &&
            state != PlayerEnum.Carrying);

        animator.SetBool("isCarrying", state == PlayerEnum.Carrying);
        animator.SetBool("isMoving", Mathf.Abs(theRB.linearVelocity.x) > 0.1f);

        animator.SetBool("isClimbing", state == PlayerEnum.Climbing);
        animator.SetBool("isClimbingMoving", isMovingOnTree);
        animator.SetBool("isCrouching", state == PlayerEnum.Crouching);
        animator.SetBool("isMoving",
            canJump && Mathf.Abs(theRB.linearVelocity.x) > 0.1f);

        animator.SetFloat("yVelocity", theRB.linearVelocity.y);

        // Atualiza a última direção para o Blend Tree de Idle
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(inputX) > 0.01f || Mathf.Abs(inputY) > 0.01f)
        {
            Vector2 lastInput = new Vector2(inputX, inputY).normalized;

            animator.SetFloat("LastInputX", lastInput.x);
            animator.SetFloat("LastInputY", lastInput.y);
        }

        if (Input.GetKeyDown(KeyCode.M))
            Meow();

        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            // Vira a onça conforme a direção pressionada
            if (horizontalInput < 0 && transform.localScale.x > 0 ||
                horizontalInput > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x * -1,
                    transform.localScale.y,
                    transform.localScale.z
                );
            }

            // Movimentação horizontal normal (exceto durante escalada)
            if (state != PlayerEnum.Climbing)
            {
                theRB.linearVelocity = new Vector2(
                    speeds[(int)state] * horizontalInput,
                    theRB.linearVelocity.y
                );
            }
        }

        if (state == PlayerEnum.Climbing)
        {
            theRB.linearVelocity = new Vector2(
                speeds[(int)state] * Input.GetAxis("Horizontal"),
                speeds[(int)state] * Input.GetAxis("Vertical")
            );
        }

        if (Input.GetButtonDown("Jump"))
            Jump();

        if (Input.GetKeyDown(KeyCode.E))
            EnterCarryMode();

        if (Input.GetKeyDown(KeyCode.F))
            ConsumeCarriedItem();

        if (Input.GetAxis("Vertical") != 0f && canClimb && state != PlayerEnum.Carrying)
            EnterClimbMode();

        // Detecta se a onça está no chão
        canJump = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            rayLength,
            groundLayer
        );

        AttentionCaller();
    }

    void Jump()
    {
        if (!canJump || state == PlayerEnum.Climbing || state == PlayerEnum.Carrying)
            return;

        animator.SetTrigger("Jump");

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

        float targetAngle = 0f;

        if (hitB.collider != null && hitF.collider != null)
        {
            Vector2 leftPoint = hitB.point.x < hitF.point.x
                ? hitB.point
                : hitF.point;

            Vector2 rightPoint = hitB.point.x < hitF.point.x
                ? hitF.point
                : hitB.point;

            float deltaX = rightPoint.x - leftPoint.x;
            float deltaY = rightPoint.y - leftPoint.y;

            if (Mathf.Abs(deltaX) > 0.01f)
            {
                targetAngle = Mathf.Atan2(
                    deltaY,
                    deltaX
                ) * Mathf.Rad2Deg;

                targetAngle = Mathf.Clamp(
                    targetAngle,
                    -maxInclineAngle,
                    maxInclineAngle
                );
            }
        }

        float direction = Mathf.Sign(transform.lossyScale.x);
        float localTargetAngle = targetAngle * direction;
        float currentAngle = visual.localEulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            localTargetAngle,
            180f * Time.deltaTime
        );

        visual.localRotation = Quaternion.Euler(
            0f,
            0f,
            newAngle
        );
    }

    void OnDrawGizmos()
    {
        if (inclineCheckB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                inclineCheckB.position,
                inclineCheckB.position + Vector3.down * inclineRayLength
            );
        }

        if (inclineCheckF != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                inclineCheckF.position,
                inclineCheckF.position + Vector3.down * inclineRayLength
            );
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                groundCheck.position,
                groundCheck.position + Vector3.down * rayLength
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

        if (playerAudio != null)
            playerAudio.PlayCarrySound();

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

    void ConsumeCarriedItem()
    {
        if (state != PlayerEnum.Carrying || nextToHold == null)
            return;

        CarryableItem item = nextToHold.GetComponent<CarryableItem>();

        if (item == null)
            return;

        if (item.effect == CarryableItem.ItemEffect.RecoverStamina)
        {
            staminaWheel.RecoverStamina(item.staminaAmount);
        }
        else if (item.effect == CarryableItem.ItemEffect.DrainStamina)
        {
            staminaWheel.DrainStamina(item.staminaAmount);
        }

        Destroy(nextToHold);

        nextToHold = null;
        carrying = false;
        canCarry = false;

        state = PlayerEnum.Normal;
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

    // Detecta o código Konami
    void CheckKonamiCode()
    {
        for (int i = 0; i < konamiCode.Length; i++)
        {
            if (Input.GetKeyDown(konamiCode[i]))
            {
                if (konamiCode[i] == konamiCode[konamiIndex])
                {
                    konamiIndex++;

                    if (konamiIndex >= konamiCode.Length)
                    {
                        ToggleGodMode();
                        konamiIndex = 0;
                    }
                }
                else
                {
                    konamiIndex = 0;
                }

                return;
            }
        }
    }

    // Ativa ou desativa o modo de voo
    void ToggleGodMode()
    {
        godMode = !godMode;

        if (godMode)
        {
            theRB.gravityScale = 0;
            theRB.linearVelocity = Vector2.zero;
            state = PlayerEnum.Normal;

            Debug.Log("KONAMI CODE: GOD MODE ATIVADO!");
        }
        else
        {
            theRB.gravityScale = originalGravityScale;
            theRB.linearVelocity = Vector2.zero;

            Debug.Log("GOD MODE DESATIVADO!");
        }
    }

    // Controla o voo
    void HandleGodMode()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        theRB.linearVelocity = new Vector2(
            horizontal * godModeSpeed,
            vertical * godModeSpeed
        );

        if (animator != null)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isClimbing", false);
            animator.SetBool("isClimbingMoving", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isMoving", false);
        }
    }
}
