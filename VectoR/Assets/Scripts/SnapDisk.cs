using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SnapDisk : MonoBehaviour
{

    public GameObject spot;
    public string objTag;

    private GameObject previous;
    private Vector3 prevCoord;

    public Animator sceneTransitionAnim;


    private void Update()
    {
        Vector3 pos = previous.transform.position;
        if (pos != prevCoord)
            GetComponent<BoxCollider>().enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == objTag)
        {
            previous = collision.gameObject;
            prevCoord = collision.gameObject.transform.position;

            GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.transform.position = spot.transform.position;
            collision.gameObject.transform.rotation = spot.transform.rotation;
            StartCoroutine(loadSceneTransition());
        }
    }

    private IEnumerator loadSceneTransition()
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene("SampleScene");
    }
}
