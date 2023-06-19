using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;

    private bool _arrowFired;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (_player.transform.localScale.x > 0)
        {
            FireRight();
        }
        if (_player.transform.localScale.x < 0)
        {
            FireLeft();
        }
    }

    void FireRight()
    {
        if (_arrowFired == false)
        {
            _rb.velocity = transform.right * _speed;
            transform.localScale = new Vector3(1, 1, 1);
            Destroy(this.gameObject, 3f);
        }
    }
    void FireLeft()
    {
        if (_arrowFired == false)
        {
            _rb.velocity = -transform.right * _speed;
            transform.localScale = new Vector3(-1, 1, 1);
            Destroy(this.gameObject, 3f);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _arrowFired = true;
        }
    }
}
