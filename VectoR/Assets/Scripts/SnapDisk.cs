using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Snap the cassette and load the ManipulationRoom scene
 */
public class SnapDisk : MonoBehaviour
{

    public GameObject spot;

    // Tag of object to snap
    public string objTag;

    // Name of the scene to load
    public string sceneToLoad;  

    private GameObject previous;
    private Vector3 prevCoord;
    private Vector3 pos;

    public Animator sceneTransitionAnim;


    private void Update()
    {
        if (previous)
        {
            pos = previous.transform.position;
        }
        // Check if the object previous is still snapped
        if (pos != prevCoord)
            GetComponent<BoxCollider>().enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == objTag)
        {
            previous = collision.gameObject;
            prevCoord = collision.gameObject.transform.position;

            // Snap to the box
            GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.transform.position = spot.transform.position;
            collision.gameObject.transform.rotation = spot.transform.rotation;

            StartCoroutine(loadSceneTransition());
        }
    }

    private IEnumerator loadSceneTransition()
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(sceneToLoad);
    }
}
