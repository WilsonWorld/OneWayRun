using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject DefeatScreenUI;
    [SerializeField] GameObject CameraSocketRef;
    [SerializeField] TextMeshProUGUI m_PowerDisplay;
    [SerializeField] int m_PowerCount = 0;

    [SerializeField] float m_Speed = 5.0f, m_JumpHeight = 10.0f, m_GravityScale = 5.0f;
    [SerializeField] float m_ButtonTime = 0.5f, m_CancelRate = 100.0f;

    Rigidbody m_Rigidbody;
    float m_JumpTime = 0.0f;
    bool bIsJumping = false;
    bool bJumpCancelled = false;
    bool bIsInAir = false;
    bool bControlsEnabled = false;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PowerDisplay.text = PowerCount.ToString();
    }

    private void Update()
    {
        if (bControlsEnabled == false)
            return;

        UpdateVerticalMovement();
    }

    void FixedUpdate()
    {
        if (bControlsEnabled == false)
            return;

        // Add constant downward force on the player character, reducing the 'floaty' aspect of jumping
        m_Rigidbody.AddForce(Physics.gravity * (m_GravityScale - 1) * m_Rigidbody.mass);

        // Adds force to bring the player character down to the ground sooner if  the jump is released early
        if (bJumpCancelled && bIsJumping && m_Rigidbody.velocity.y > 0)
            m_Rigidbody.AddForce(Vector3.down * m_CancelRate);

        UpdateHorizontalMovement();
    }

    // Changes the velocity based on the Horizontal axis input and speed. Updates the change of velocity each frame until hitting the max speed.
    void UpdateHorizontalMovement()
    {
        Vector3 curVelocity = m_Rigidbody.velocity;
        Vector3 targetVelocity = new Vector3(0.0f, curVelocity.y, Input.GetAxis("Horizontal"));
        targetVelocity.z *= m_Speed;
        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = (targetVelocity - curVelocity);
        m_Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    // Performs a jump if the player presses the Spacebar, and isn't already in the air, while updating the jump time.
    void UpdateVerticalMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CheckAirStatus();

            if (bIsInAir)
                return;

            Jump();
        }

        UpdateJumpTime();
    }

    // Raycasts beneath the player's current position to check if it's colliding with any objects tagged as 'Environment' and updates the In Air status.
    void CheckAirStatus()
    {
        RaycastHit hitInfo;
        int layerMask = 1 << 6;
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo, transform.localScale.y * 0.5f + 1.1f, layerMask)) {
            if (hitInfo.collider.tag == "Environment")
                bIsInAir = false;
        }
        else
            bIsInAir = true;
    }

    // Adds a burst of force upward to the player, based on desired jump height and the world gravity.
    void Jump()
    {
        float jumpForce = Mathf.Sqrt(m_JumpHeight * -2 * (Physics.gravity.y));
        m_Rigidbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.Impulse);

        bIsInAir = true;
        bIsJumping = true;
        bJumpCancelled = false;
        m_JumpTime = 0.0f;
    }

    // Increase the jump time during a jump, stopping when the button is release or the jump time reaches the held button time limit.
    void UpdateJumpTime()
    {
        if (bIsJumping) {
            m_JumpTime += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space))
                bJumpCancelled = true;

            if (m_JumpTime > m_ButtonTime)
                bIsJumping = false;
        }
    }

    public void Defeat()
    {
        OnDefeat();
    }

    void OnDefeat()
    {
        PowerCount = 0;
        UpdatePowerDisplayUI();
        m_Rigidbody.velocity = Vector3.zero;
        CameraSocketRef.GetComponent<MoveOverTime>().CancelMovement();
        this.gameObject.SetActive(false);
        DefeatScreenUI.SetActive(true);
    }

    public void IncreasePower(int amount)
    {
        PowerCount += amount;
        UpdatePowerDisplayUI();
    }

    void UpdatePowerDisplayUI()
    {
        m_PowerDisplay.text = PowerCount.ToString();
    }

    public int PowerCount
    {
        get { return m_PowerCount; }
        set { m_PowerCount = value; }
    }

    public void EnableControls(bool enable)
    {
        bControlsEnabled = enable;
    }
}
