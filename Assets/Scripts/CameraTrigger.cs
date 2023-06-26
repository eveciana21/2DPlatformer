using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private CameraManager _camManager;
    private Player _player;
    // [SerializeField] private int _selectCam;

    private Animator _animator;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GameObject.Find("Screen_Fade").GetComponent<Animator>();
        // _camManager = GameObject.Find("Camera_Manager").GetComponent<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _animator.SetBool("screenFade", true);
            StartCoroutine(ScreenFade());


            /* _camManager.ResetCams();
             _camManager.SelectMasterCam(_selectCam);*/
        }
    }
    IEnumerator ScreenFade()
    {
        yield return new WaitForSeconds(1);
        //_player.PlayerPosOne();
        yield return new WaitForSeconds(1f);
        _animator.SetBool("screenFade", false);
    }
}
