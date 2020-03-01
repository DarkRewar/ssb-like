using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    public bool IsDummy = false;

    public int Life = 0;

    public float MoveSpeed = 3;

    public float JumpHeight = 1f;

    public float GroundCheckRadius = 0.1f;

    public SpriteRenderer SpritePlayer;
    public Animator Animator;

    private PlayerActions _actions;
    private Rigidbody2D _rigidbody;

    private bool _isGrounded = true;
    private bool _isAttacking = false;
    private bool _isInvicible = false;

    [SerializeField]
    private Vector2 _currentMovementInput;

    private int _lookDirection = 1;

    private void Awake()
    {
        if (IsDummy) return;

        _actions = new PlayerActions();
        _actions.devices = new UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputDevice>();
        _actions.Enable();

        _actions.Main.Movement.performed += OnMovement;
        _actions.Main.Jump.performed += OnJump;
        _actions.Main.Attack.performed += OnAttack;
    }

    private void OnEnable()
    {
        if (IsDummy) return;
        _actions.Main.Enable();
    }

    private void OnDisable()
    {
        if (IsDummy) return;
        _actions.Main.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.velocity = new Vector2(_currentMovementInput.x * MoveSpeed, _rigidbody.velocity.y);
        Animator.SetBool("IsJumping", !_isGrounded);
    }

    private void FixedUpdate()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, GroundCheckRadius);
        _isGrounded = false;
        foreach (var col in cols)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
            }
        }
    }

    internal void BindDevice(InputDevice device)
    {
        _actions.devices = new UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputDevice>(new InputDevice[]
        {
            device
        });
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        _currentMovementInput = obj.ReadValue<Vector2>();
        float deadZone = 0.25f;
        if(Mathf.Abs(_currentMovementInput.x) < deadZone)
        {
            _currentMovementInput.x = 0;
        }

        Animator.SetBool("IsWalking", _currentMovementInput.x != 0);

        if(_currentMovementInput.x != 0 && !_isAttacking)
        {
            _lookDirection = _currentMovementInput.x < 0 ? -1 : 1;
            SpritePlayer.flipX = _lookDirection == -1;
        }
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (_isGrounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpHeight);
        }
    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (!_isAttacking)
        {
            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        _isAttacking = true;
        Animator.SetBool("IsAttacking", _isAttacking);

        yield return new WaitForSecondsRealtime(0.45f);

        float temp = 0f, time = 0.35f;
        List<PlayerBehaviour> playersTouched = new List<PlayerBehaviour>();
        Vector2 originAttack = transform.position;
        originAttack.x += _lookDirection * 1.5f;
        originAttack.y += 0.5f;

        while(temp <= time)
        {
            
            Collider2D[] cols = Physics2D.OverlapBoxAll(originAttack, Vector2.one, 0);

            foreach(var col in cols)
            {
                if (col.gameObject.CompareTag("Player") && !playersTouched.Exists(x => x.gameObject.Equals(col.gameObject)))
                {
                    PlayerBehaviour pb = col.GetComponent<PlayerBehaviour>();
                    playersTouched.Add(pb);
                    pb.Hit(this);
                }
            }

            temp += Time.fixedDeltaTime;
            yield return null;
        }

        _isAttacking = false;
        Animator.SetBool("IsAttacking", _isAttacking);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, GroundCheckRadius);
    }

    public void Hit(PlayerBehaviour origin, float force = 1f)
    {
        if (_isInvicible) return;

        Animator.SetTrigger("GetHit");

        Life += 10;

        _rigidbody.AddForce((transform.position - origin.transform.position).normalized * 100 * Life, ForceMode2D.Force);

        GameManager.Instance.OnPlayerHit(this);
    }

    internal void Die()
    {
        Life = 0;
        GameManager.Instance.OnPlayerHit(this);

        _rigidbody.velocity = Vector2.zero;
        StartCoroutine(DoInvulnerability());
    }

    private IEnumerator DoInvulnerability()
    {
        _isInvicible = true;
        yield return new WaitForSeconds(3f);
        _isInvicible = false;
    }
}
