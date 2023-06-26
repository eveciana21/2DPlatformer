using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _mySpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _rollSpeed;
    private Rigidbody2D _rb;

    [SerializeField] private float _canAttack = 0;
    [SerializeField] private float _attackRate = 0.3f;

    private Animator _animator;

    [SerializeField] private bool _isGrounded;

    [SerializeField] private bool _isFacingRight;

    private float _lastTapTime = 0;
    private float _tapSpeed = 0.125f;

    [SerializeField] private bool _canClimb;
    [SerializeField] private bool _isClimbing;

    [SerializeField] private CapsuleCollider2D _playerCollider;
    [SerializeField] private BoxCollider2D _playerFeet;

    private int _gravityAtStart = 4;

    [SerializeField] private bool _playerHasBow;
    private bool _canFireArrow;
    [SerializeField] private GameObject _arrowPrefab;

    [SerializeField] private GameObject _bowAndArrow;

    [SerializeField] private Bow _bow;

    [SerializeField] private float _arrowSpeed;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.SetLayerWeight(0, 1);
        _bowAndArrow.SetActive(false);

        _rb.gravityScale = _gravityAtStart;

        _playerFeet = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (_canFireArrow == true)
        {
            _animator.SetBool("isRunning", false);
        }

        if (_isClimbing == false)
        {
            Jump();
        }

        if (gameObject.transform.localScale.x > 0)
        {
            _isFacingRight = true;
        }
        else if (gameObject.transform.localScale.x < 0)
        {
            _isFacingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded == true)
        {
            _rb.velocity += new Vector2(0f, _jumpHeight);
            _rb.gravityScale = _gravityAtStart / 2;
        }
        if (Input.GetKeyUp(KeyCode.Space) | _rb.velocity.y < -0.1)
        {
            _rb.gravityScale = _gravityAtStart;
        }

        Climbing();
        Roll();
        Attack();
        FireArrow();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }


    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (_canFireArrow == false)
        {
            transform.Translate(Vector3.right * _mySpeed * horizontal * Time.deltaTime);
        }

        if (horizontal > 0)
        {
            if (!_isFacingRight)
            {
                FlipSprite();
            }
            if (_isGrounded == true)
            {
                _animator.SetBool("isRunning", true);
            }
        }
        else if (horizontal < 0)
        {
            if (_isFacingRight)
            {
                FlipSprite();
            }
            if (_isGrounded == true)
            {
                _animator.SetBool("isRunning", true);
            }
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    private void FlipSprite()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _isFacingRight = !_isFacingRight;
    }

    private void Jump()
    {
        if (!_playerFeet.IsTouchingLayers(LayerMask.GetMask("Platform")) && _isClimbing == false)
        {
            _isGrounded = false;
            _animator.SetBool("isJumping", true);
        }
        if (_playerFeet.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            _isGrounded = true;
            _animator.SetBool("isJumping", false);
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > _canAttack)
        {
            if (_isGrounded == true)
            {
                _animator.SetTrigger("Attack_1");
            }
            else
            {
                _animator.SetTrigger("Attack_2");
            }
            _canAttack = Time.time + _attackRate;
        }
    }

    private void FireArrow()
    {
        if (_playerHasBow == true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _animator.SetBool("isFiringArrow", true);
                StartCoroutine("CanFireArrow");
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopCoroutine("CanFireArrow");
                _animator.SetBool("isFiringArrow", false);

                if (_canFireArrow == true)
                {
                    if (_isFacingRight == true)
                    {
                        GameObject newArrow = Instantiate(_arrowPrefab, transform.position + new Vector3(0.1f, -0.04f, 0f), Quaternion.identity);
                        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * _arrowSpeed;
                        _canFireArrow = false;
                    }
                    else if (_isFacingRight == false)
                    {
                        GameObject newArrow = Instantiate(_arrowPrefab, transform.position + new Vector3(0.1f, -0.04f, 0f), Quaternion.identity);
                        newArrow.GetComponent<Rigidbody2D>().velocity = -transform.right * _arrowSpeed;
                        _canFireArrow = false;
                    }
                }
            }
        }
    }


    IEnumerator CanFireArrow()
    {
        yield return new WaitForSeconds(0.4f);
        _canFireArrow = true;
    }

    void Climbing()
    {
        if (_playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                float vertical = Input.GetAxis("Vertical");
                transform.Translate(Vector3.up * vertical * _mySpeed / 2 * Time.deltaTime);
                _isClimbing = true;
                _rb.gravityScale = _gravityAtStart * 0;
                _animator.SetBool("isClimbing", true);
                if (_isClimbing == true)
                {
                    _isGrounded = false;
                }
            }
            else
            {
                _animator.SetBool("isClimbing", false);
            }
        }
        else
        {
            _animator.SetBool("isClimbing", false);
            _rb.gravityScale = _gravityAtStart;
            _isClimbing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            _playerHasBow = false;
            _animator.SetLayerWeight(0, 0f);
            _animator.SetLayerWeight(1, 1f);
            _animator.SetLayerWeight(2, 0f);
            _animator.SetLayerWeight(3, 0f);
            Debug.Log("Picked up Sword!");
        }
        if (other.tag == "Spear")
        {
            _playerHasBow = false;
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

        /*if (other.tag == "Ladder")
        {
            _canClimb = true;
        }*/
    }











    /* private void OnTriggerExit2D(Collider2D other)
     {
         if (other.tag == "Ladder")
         {
             _animator.SetBool("isClimbing", false);
             _rb.gravityScale = _gravityAtStart;
             _canClimb = false;
             _isClimbing = false;
         }
     }*/







    /*private void OnCollisionExit2D(Collision2D other)
    {
        if (!_feet.IsTouchingLayers(LayerMask.GetMask("Platform")) && _isClimbing == false)
        {
            Debug.Log("isnottouchingcollision");
            _isGrounded = false;
            _animator.SetBool("isJumping", true);
        }

        if (other.gameObject.layer == 3 && _isClimbing == false)
        {
            _isGrounded = false;
            _animator.SetBool("isJumping", true);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_feet.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            Debug.Log("istouchingcollision");
            _isGrounded = true;
            _animator.SetBool("isJumping", false);
        }
        if (other.gameObject.layer == 3)
        {
            _isGrounded = true;
            _animator.SetBool("isJumping", false);
        }
    }*/




}
