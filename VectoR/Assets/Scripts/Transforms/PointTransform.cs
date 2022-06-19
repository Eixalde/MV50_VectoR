using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Change the tranform parameters in depends of the Point attributes
 */
public class PointTransform : MonoBehaviour
{
    // Position of the point using coordinateSystem
    public Vector3 position;
    // Coordinate system used for positioning the vector
    public GameObject coordinateSystem;

    // Selection manager
    private GameObject selectionManager;

    // Start is called before the first frame update
    void Start()
    {
        selectionManager = GameObject.Find("SelectionManager");
    }

    // Update is called once per frame
    void Update()
    {
        CheckSelection();
        setTranformFromPoint();
    }

    // Select the object 
    public void Select(bool select)
    {
        GetComponent<Outline>().enabled = select;
    }

    // Set position of the selected part of the gameobject
    public void setPosition(Vector3 newPosition)
    {

        position = newPosition - coordinateSystem.transform.position;
    }

    // Place the Point Object position using position and coordinate system
    private void setTranformFromPoint()
    {
        if (coordinateSystem)
        {
            transform.position = position + coordinateSystem.transform.position;
        }
    }

    // Check if the point is still selected
    private void CheckSelection()
    {
        if (selectionManager)
        {
            if (selectionManager.GetComponent<ObjectSelect>()?.getSelectedObject() != gameObject)
                Select(false);
        }

    }
}
