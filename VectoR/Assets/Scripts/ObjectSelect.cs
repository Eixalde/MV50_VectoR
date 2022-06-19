using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Keep in memory the last gameobject selected
 */
public class ObjectSelect : MonoBehaviour
{
    // Current selected object
    public GameObject currentObjectSelected;

    public void select(GameObject lastObjectSlected)
    {
        currentObjectSelected = lastObjectSlected;
    }

    public GameObject getSelectedObject()
    {
        return currentObjectSelected;
    }
}
