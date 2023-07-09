using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _wallDetector;
    [SerializeField] private CircleCollider2D _clifDetector;
    [SerializeField] private CapsuleCollider2D _bodyCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isPlayerNearby;

    private bool _isFacingRight;
    private bool _canBeDamaged;
    private Player _player;
    private bool _canBeFlipped;
    private bool _nearClif;
    private bool _enemyDead;



    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("Player is null");
        }
        _canBeDamaged = false;
        StartCoroutine(EnableCollider());
    }

    void Update()
    {
        if (gameObject.transform.localScale.x > 0)
        {
            _isFacingRight = true;
        }
        else if (gameObject.transform.localScale.x < 0)
        {
            _isFacingRight = false;
        }

        if (_enemyDead == false)
        {
            CheckDistanceFromPlayer();
            EnemyMovement();
        }
    }

    void EnemyMovement()
    {
        if (_wallDetector.IsTouchingLayers(LayerMask.GetMask("Platform"))) // FLIP SPRITE IF ENEMY BUMPS INTO WALL
        {
            FlipSprite();
        }

        if (_canBeFlipped == true)
        {
            if (!_clifDetector.IsTouchingLayers(LayerMask.GetMask("Platform"))) // FLIP SPRITE IF ENEMY IS NEAR A CLIF //
            {
                _nearClif = true;
                if (_nearClif == true)
                {
                    FlipSprite();
                }
            }
            else
            {
                _nearClif = false;
            }
        }

        if (_isPlayerNearby == true)
        {
            if (_isFacingRight == true)
            {
                if (_player.transform.position.x < transform.position.x) // If Enemy is looking RIGHT && Player is on Left
                {
                    FlipSprite();
                    transform.Translate(Vector3.left * _speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.right * _speed * Time.deltaTime); // If Enemy is looking RIGHT and player is on Right
                }
            }
            else
            {
                if (_player.transform.position.x > transform.position.x) // If Enemy is looking LEFT && Player is on Right
                {
                    FlipSprite();
                    transform.Translate(Vector3.right * _speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.left * _speed * Time.deltaTime); // If Enemy is looking left && Player is on left
                }
            }
        }

        else
        {
            if (_isFacingRight == true)
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
        }
    }

    void CheckDistanceFromPlayer()
    {
        float playerDistance = Vector3.Distance(transform.position, _player.transform.position);
        if (playerDistance < 5)
        {
            _isPlayerNearby = true;
        }
        else if (playerDistance >= 5)
        {
            _isPlayerNearby = false;
        }
        if (playerDistance <= 1.5f)
        {
            _clifDetector.enabled = false;
            _animator.SetBool("Run", false);
            _animator.SetBool("Attack", true);
        }
        else
        {
            _clifDetector.enabled = true;
            _animator.SetBool("Run", true);
            _animator.SetBool("Attack", false);
        }
    }

    void FlipSprite()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _isFacingRight = !_isFacingRight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_canBeDamaged == true)
        {
            if (other.tag == "Sword")
            {
                _enemyDead = true;
                _animator.SetBool("Death", true);
                Destroy(this.gameObject, 3);
            }
            if (other.tag == "Arrow")
            {
                _enemyDead = true;
                _animator.SetBool("Death", true);
                Destroy(this.gameObject, 3);
            }
        }
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _canBeDamaged = true;
        _canBeFlipped = true;
    }
}
