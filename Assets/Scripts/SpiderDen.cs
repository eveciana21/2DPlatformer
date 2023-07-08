using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDen : MonoBehaviour
{
    [SerializeField] private GameObject _spiderPrefab;
    [SerializeField] private Animator _animator;


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            Instantiate(_spiderPrefab, transform.position, Quaternion.identity);
            _animator.SetBool("destroy", true);
            Destroy(this.gameObject, 5f);

        }
        if (other.tag == "Arrow")
        {
            Instantiate(_spiderPrefab, transform.position, Quaternion.identity);
            _animator.SetBool("destroy", true);
            Destroy(this.gameObject, 5f);
        }
    }
}
