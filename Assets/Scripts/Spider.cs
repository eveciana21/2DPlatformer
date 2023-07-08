using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _wallDetector;
    [SerializeField] private CircleCollider2D _clifDetector;
    [SerializeField] private Animator _animator;
    private bool _isFacingRight;
    private bool _canBeDamaged;
    private Player _player;
    private float _speed = 1;
    private bool _isPlayerNearby;
    private bool _canBeFlipped;
    private bool _nearClif;
    private int _direction = 1;
    private bool _facePlayer;



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

        CheckDistanceFromPlayer();
        EnemyMovement();
    }

    void EnemyMovement()
    {
        _animator.SetBool("Run", true);

        // FLIP SPRITE IF ENEMY BUMPS INTO WALL //
        if (_wallDetector.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            if (_isFacingRight == false)
            {
                FlipSprite();
            }
        }

        // FLIP SPRITE IF ENEMY IS NEAR A CLIF //
        if (_canBeFlipped == true)
        {
            if (!_clifDetector.IsTouchingLayers(LayerMask.GetMask("Platform")))
            {
                _nearClif = true;
                FlipSprite();
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
                if (_player.transform.position.x < transform.position.x) // If Enemy is looking LEFT
                {
                    FlipSprite();
                    transform.Translate(Vector3.left * 1f * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.right * 1f * Time.deltaTime);
                }
            }
            else
            {
                if (_player.transform.position.x > transform.position.x) // If Enemy is looking LEFT
                {
                    FlipSprite();
                    transform.Translate(Vector3.right * 1f * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.left * 1f * Time.deltaTime);
                }
            }
        }
        else
        {
            if (_isFacingRight == true)
            {

                FlipSprite();
                transform.Translate(Vector3.left * 1f * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * 1f * Time.deltaTime);
            }
        }
        //}

        /*else
        {
            //IF THE PLAYER IS NEARBY && THE ENEMY IS NEAR A CLIF
            if (_nearClif == true)
            {
                return;
            }
            else
            {
                float speed = _speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, speed);
            }
        }*/

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
                Debug.Log("Destroyed");
                Destroy(this.gameObject);
            }
            if (other.tag == "Arrow")
            {
                Debug.Log("Destroyed");
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(2f);
        _canBeDamaged = true;
        _canBeFlipped = true;
    }

}
