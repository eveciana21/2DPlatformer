using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _mySpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _rollSpeed;
    private Rigidbody2D _rb;

    private Animator _animator;

    private float _attackTapRate = 0.2f;
    private float _canUseSecondaryAttack;
    private float _attackRate;

    [SerializeField] private bool _isGrounded;

    private bool _isFacingRight;

    private float _lastTapTime = 0;
    private float _tapSpeed = 0.125f;

    [SerializeField] private bool _canClimb;
    [SerializeField] private bool _isClimbing;

    [SerializeField] private CapsuleCollider2D _playerCollider;
    private int _gravityAtStart = 4;

    [SerializeField] private bool _playerHasBow;
    private bool _canFireArrow;
    [SerializeField] private GameObject _arrowPrefab;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.SetLayerWeight(0, 1);


        _rb.gravityScale = _gravityAtStart;
    }

    void Update()
    {
        if (_canClimb == true)
        {
            Climbing();
        }

        if (_isClimbing == false)
        {
            Jump();
        }

        Roll();
        Attack();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }


    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * _mySpeed * horizontal * Time.deltaTime);

        if (horizontal < 0)
        {
            if (_isGrounded == true)
            {
                _animator.SetBool("isRunning", true);
            }
            FlipSprite();

            if (_isFacingRight == true)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (horizontal > 0)
        {
            if (_isGrounded == true)
            {
                _animator.SetBool("isRunning", true);
            }
            FlipSprite();

            if (_isFacingRight == false)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    private void FlipSprite()
    {
        _isFacingRight = !_isFacingRight;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.velocity += new Vector2(0f, _jumpHeight);
            _rb.gravityScale = _gravityAtStart / 2;
        }
        if (Input.GetKeyUp(KeyCode.Space) | _rb.velocity.y < -0.1)
        {
            _rb.gravityScale = _gravityAtStart;
        }
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.D) && _isGrounded == true)
        {
            if ((Time.time - _lastTapTime) < _tapSpeed)
            {
                _animator.SetTrigger("isRolling");
                _rb.AddForce(transform.right * _rollSpeed, ForceMode2D.Force);
            }
            _lastTapTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.A) && _isGrounded == true)
        {
            if ((Time.time - _lastTapTime) < _tapSpeed)
            {
                _animator.SetTrigger("isRolling");
                _rb.AddForce(-transform.right * _rollSpeed, ForceMode2D.Force);
            }
            _lastTapTime = Time.time;
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _animator.SetTrigger("Attack_1");
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if ((Time.time - _lastTapTime) < _tapSpeed)
            {
                _animator.SetTrigger("Attack_2");
            }
            _lastTapTime = Time.time;
        }

        if (_playerHasBow == true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                StartCoroutine("FireArrow");
                _animator.SetBool("isFiringArrow", true);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopCoroutine("FireArrow");
                _animator.SetBool("isFiringArrow", false);

                if (_canFireArrow == true)
                {
                    Instantiate(_arrowPrefab, transform.position + new Vector3(0.1f, -0.04f, 0f), Quaternion.identity);
                    _canFireArrow = false;
                }
            }
        }
    }

    IEnumerator FireArrow()
    {
        yield return new WaitForSeconds(0.3f);
        _canFireArrow = true;
        Debug.Log("Can Fire Arrow");
    }






    void Climbing()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            float vertical = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * vertical * _mySpeed / 2 * Time.deltaTime);
            _animator.SetBool("isClimbing", true);
            _isClimbing = true;
            _rb.gravityScale = 0;
        }
        else
        {
            _animator.SetBool("isClimbing", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            _animator.SetLayerWeight(0, 0f);
            _animator.SetLayerWeight(1, 1f);
            _animator.SetLayerWeight(2, 0f);
            _animator.SetLayerWeight(3, 0f);
            Debug.Log("Picked up Sword!");
        }
        if (other.tag == "Spear")
        {
            _animator.SetLayerWeight(0, 0f);
            _animator.SetLayerWeight(1, 0f);
            _animator.SetLayerWeight(2, 1f);
            _animator.SetLayerWeight(3, 0f);
            Debug.Log("Picked up Spear!");
        }
        if (other.tag == "Bow")
        {
            _playerHasBow = true;
            _animator.SetLayerWeight(0, 0f);
            _animator.SetLayerWeight(1, 0f);
            _animator.SetLayerWeight(2, 0f);
            _animator.SetLayerWeight(3, 1f);
            Debug.Log("Picked up Bow!");
        }

        if (other.tag == "Ladder")
        {
            _canClimb = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            _animator.SetBool("isClimbing", false);
            _rb.gravityScale = _gravityAtStart;
            _canClimb = false;
            _isClimbing = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == 3 && _isClimbing == false)
        {
            _isGrounded = false;
            _animator.SetBool("isJumping", true);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
        {
            _isGrounded = true;
            _animator.SetBool("isJumping", false);
        }
    }

    public void PlayerPosOne()
    {
        transform.position = new Vector3(54f, 8.5f, 0);
    }

}
