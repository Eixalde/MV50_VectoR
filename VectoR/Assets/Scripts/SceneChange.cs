using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Class managing scene changes
 */
public class SceneChange : MonoBehaviour
{
    public void OnQuitScene()
    {
        StartCoroutine(loadSceneTransition());
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }


    private IEnumerator loadSceneTransition()
    {
        yield return new WaitForSeconds(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Classroom");
    }
}
