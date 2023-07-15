using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Menu")
        {
            _animator.SetLayerWeight(0, 0f);
            _animator.SetLayerWeight(1, 0f);
            _animator.SetLayerWeight(2, 0f);
            _animator.SetLayerWeight(3, 1f);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(1);
        }
    }




}
