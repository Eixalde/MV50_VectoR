using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keep in memory the last gameobject selected
public class ObjectSelect : MonoBehaviour
{
    public GameObject currentObjectSelected;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void select(GameObject lastObjectSlected)
    {
        currentObjectSelected = lastObjectSlected;
    }

    public GameObject getSelectedObject()
    {
        return currentObjectSelected;
    }
}
