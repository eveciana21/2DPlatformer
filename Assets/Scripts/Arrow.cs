using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;
    [SerializeField] private bool _hasHitWall;

    void Start()
    {
        // _player = GameObject.Find("Player").GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FireArrow();
    }

    void FireArrow()
    {
        // transform.Translate(transform.forward * _speed * Time.deltaTime);
        if (_hasHitWall == false)
        {
            float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Destroy(this.gameObject, 3f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /*if (other.gameObject.layer == 3)
        {
            _hasHitWall = true;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            Debug.Log("Has Hit Wall");
        }*/
    }
}
