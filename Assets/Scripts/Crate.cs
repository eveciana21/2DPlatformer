using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private Animator _crateDestroyedAnim;
    private BoxCollider2D _collider;

    [SerializeField] private GameObject _coin;
    private int _random;
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _random = Random.Range(0, 2);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            if (_random == 1)
            {
                Instantiate(_coin, transform.position, Quaternion.identity);
            }
            _crateDestroyedAnim.enabled = true;
            _collider.enabled = false;
            Destroy(this.gameObject, 5f);
        }
    }
}
