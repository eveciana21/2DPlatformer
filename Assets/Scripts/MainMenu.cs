using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playDirector, _optionsDirector, _quitDirector;


    public void PlayButton()
    {
        StartCoroutine(Play());
    }
    public void OptionsButton()
    {
        StartCoroutine(Options());
    }
    public void QuitButton()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Play()
    {
        _playDirector.Play();
        yield return new WaitForSeconds(2);
        //SceneManager.LoadScene(2); // LEVEL ONE
    }
    IEnumerator Options()
    {
        _optionsDirector.Play();
        yield return new WaitForSeconds(2);
        //SceneManager.LoadScene(2); // LEVEL ONE
    }
    IEnumerator Quit()
    {
        _quitDirector.Play();
        yield return new WaitForSeconds(2);
        //Application.Quit();
    }
}
