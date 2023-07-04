using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject _coinPickupAnim;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject coinPickup = Instantiate(_coinPickupAnim, transform.position, Quaternion.identity);
            Destroy(coinPickup, 0.2f);
            Debug.Log("Picked up Coin!");
            Destroy(this.gameObject);
        }
    }
}
