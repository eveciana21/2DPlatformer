using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _shakeDuration;
    private float _xShake, _yShake;

    void Start()
    {
        transform.position = new Vector3(0.2f, 0.5f, -1);
    }

    IEnumerator ShakeDuration()
    {
        Vector3 originalPos = transform.position;
        _shakeDuration = Time.time + 0.1f;

        while (_shakeDuration > Time.time)
        {
            //Debug.Break();
            _xShake = (Random.Range(0.12f, 0.28f));
            _yShake = (Random.Range(0.42f, 0.58f));
            transform.position = new Vector3(_xShake, _yShake, -1);
            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPos;
    }

    public void CameraShaking()
    {
        StartCoroutine(ShakeDuration());
    }
}
