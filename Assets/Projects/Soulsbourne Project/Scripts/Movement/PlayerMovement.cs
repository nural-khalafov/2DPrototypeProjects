using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SoulsbourneInput;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    SoulsbourneInputActions _soulsbourneInputActions;
    InputAction _move;
    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _groundCheck;
    Animator _animator;

    Vector2 _moveInput;

    [Header("Movement Settings")]
    [SerializeField] float m_moveSpeed = 10f;
    [SerializeField] float m_jumpSpeed = 15f;
    [SerializeField] float m_dashSpeed = 50f;

    [Header("Player Settings")]
    [SerializeField] float m_stamina = 100f;

    float m_currentSpeed;

    float m_sprintCost = 5f;
    float m_dashCost = 50f;

    bool m_isFlipped = false;
    bool m_isGrounded = false;
    bool m_isSprinting = false;
    bool m_isDashOn = false;

    bool m_isSprintAvailable = true;
    bool m_isDashAvailable = true;

    [Header("Effects Settings")]

    public GameObject SprintEffect;

    public float CurrentSpeed 
    { 
        get => m_currentSpeed;
        set 
        {
            m_currentSpeed = value;

            if (m_isSprinting) 
            {
                m_currentSpeed = m_moveSpeed * 2f;
            }
            else 
            {
                m_currentSpeed = m_moveSpeed;
            }
        }
    }

    public float Stamina 
    {
        get => m_stamina; set => m_stamina = value;
    }

    #region Unity Methods

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _groundCheck = GetComponentInChildren<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _soulsbourneInputActions = new SoulsbourneInputActions();
    }

    private void Start()
    {
    }

    private void Update() 
    {
        CurrentSpeed = CurrentSpeed;

        MovementHandler();
        FlipEffects();

        // SetStamina(); needs refactoring

        if (m_isGrounded) 
            _animator.SetBool("IsGrounded", true);
        else 
        {
            _animator.SetBool("IsGrounded", false);
            _animator.SetBool("IsRunning", false);
        }
    }

    private void FixedUpdate()
    {
        m_isGrounded = _groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void OnEnable()
    {
        _move = _soulsbourneInputActions.Player.Move;
        _move.Enable();

        _soulsbourneInputActions.Player.Jump.performed += OnJump;
        _soulsbourneInputActions.Player.Jump.Enable();

        _soulsbourneInputActions.Player.Sprint.performed += OnSprint;
        _soulsbourneInputActions.Player.Sprint.canceled += OnSprint;
        _soulsbourneInputActions.Player.Sprint.Enable();

        _soulsbourneInputActions.Player.Dash.performed += OnDash;
        _soulsbourneInputActions.Player.Sprint.canceled += OnDash;
        _soulsbourneInputActions.Player.Dash.Enable();
    }
    
    private void OnDisable()
    {
        _move.Disable();
        _soulsbourneInputActions.Player.Jump.Disable();
        _soulsbourneInputActions.Player.Sprint.Disable();
        _soulsbourneInputActions.Player.Dash.Disable();
    }

    #endregion


    void MovementHandler()
    {
        _moveInput = _move.ReadValue<Vector2>();

        Vector2 playerVelocity = new Vector2(_moveInput.x * CurrentSpeed, _rigidbody.velocity.y);

        if (_moveInput.x == 0f)
        {
            _animator.SetBool("IsRunning", false);
        }
         
        if (_moveInput.x < 0f)
        {
            _spriteRenderer.flipX = true;
            m_isFlipped = true;
            _animator.SetBool("IsRunning", true);
        }
        else if (_moveInput.x > 0f)
        {
            _spriteRenderer.flipX = false;
            m_isFlipped = false;
            _animator.SetBool("IsRunning", true);
        }

        if (!m_isDashOn)
            _rigidbody.velocity = playerVelocity;
        else
            return;
    }

    void FlipEffects() 
    {
        if(_moveInput.x < 0f) 
        {
            SprintEffect.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(_moveInput.x > 0f)
        {
            SprintEffect.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // Stamina mechanics needs refactoring
    void SetStamina() 
    {
        if (Stamina <= 0 && Stamina >= 0)
        {
            Stamina += 5f;

            if (Stamina < m_sprintCost)
                m_isSprintAvailable = false;
            if (Stamina < m_dashCost)
                m_isDashAvailable = false;
        }
        else if (Stamina >= 100)
            Stamina = 100f;

        if (m_sprintCost <= Stamina)
            m_isSprintAvailable = false;
        if (m_dashCost <= Stamina)
            m_isDashAvailable = false;

        if (m_isSprinting && m_isSprintAvailable)
            Stamina -= m_sprintCost * Time.deltaTime;
        else if (m_isDashOn && m_isDashAvailable)
            Stamina -= m_dashCost * Time.deltaTime;
    }

    #region Events

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (m_isGrounded)
        {
            _rigidbody.velocity += new Vector2(0f, m_jumpSpeed);
            _animator.SetTrigger("OnJump");
        }
        else { return; }
    }

    private void OnSprint(InputAction.CallbackContext obj)
    {
        if (obj.performed && m_isGrounded)
        {
            m_isSprinting = true;
            _animator.SetBool("IsSprinting", true);
            SprintEffect.SetActive(true);
        }
        else if (obj.canceled) 
        {
            m_isSprinting = false;
            _animator.SetBool("IsSprinting", false);
            SprintEffect.SetActive(false);
        }
    }

    private void OnDash(InputAction.CallbackContext obj)
    {
        if (obj.performed) 
        {
            m_isDashOn = true;

            if (!m_isFlipped)
                _rigidbody.velocity += new Vector2(m_dashSpeed, 0f);
            else
                _rigidbody.velocity += new Vector2(-m_dashSpeed, 0f);
        }
        StartCoroutine(DashCoroutine());
    }


    #endregion

    IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        m_isDashOn = false;
    }
}
