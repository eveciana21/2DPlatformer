using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform _shotPoint;
    [SerializeField] GameObject _arrowPrefab;
    [SerializeField] private float _speed = 5;


    void Update()
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //

        //REMEMBER TO SET THE Z VALUE ON THE PARENT BOW GAMEOBJECT TO -30 FOR THIS TO WORK


        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;


        /*Vector3 direction = Camera.main.WorldToScreenPoint(Input.mousePosition) - transform.position;
        Debug.DrawRay(transform.position, direction, Color.red);
        Quaternion.LookRotation(direction);*/
    }

    public void RotateBowRight()
    {
        if (transform.localEulerAngles.z < 180 && transform.localEulerAngles.z > 60)
        {
            transform.localEulerAngles = new Vector3(0, 0, 60);
        }
        else if (transform.localEulerAngles.z > 180 || transform.localEulerAngles.z < 1)
        {
            transform.localEulerAngles = new Vector3(0, 0, 1);
        }
    }
    public void RotateBowLeft()
    {
        if (transform.localEulerAngles.z < 180 && transform.localEulerAngles.z > 120)
        {
            transform.localEulerAngles = new Vector3(0, 0, 120);
        }
        else if (transform.localEulerAngles.z > 180 || transform.localEulerAngles.z < 179)
        {
            transform.localEulerAngles = new Vector3(0, 0, 179);
        }
    }

    /* public void FireArrowRight()
     {
         GameObject arrow = Instantiate(_arrowPrefab, transform.position, Quaternion.identity);
         arrow.GetComponent<Rigidbody2D>().velocity = transform.right * _speed;
     }*/

}
