using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float _speed;

    private bool _arrowFired;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

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
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _arrowFired = true;
        }
    }
    void FireRight()
    {
        if (_arrowFired == false)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            transform.localScale = new Vector3(1, 1, 1);
            Destroy(this.gameObject, 3f);
        }
    }
    void FireLeft()
    {
        if (_arrowFired == false)
        {
            transform.Translate(-Vector3.right * _speed * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1);
            Destroy(this.gameObject, 3f);
        }
    }


}
