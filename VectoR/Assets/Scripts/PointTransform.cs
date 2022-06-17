using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTransform : MonoBehaviour
{
    public Vector3 position;
    public GameObject coordinateSystem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckSelection();
        setTranformFromPoint();
    }

    public void Select(bool select)
    {
        GetComponent<Outline>().enabled = select;
    }

    // Set position of the selected part of the gameobject
    public void setPosition(Vector3 newPosition)
    {

        position = newPosition - coordinateSystem.transform.position;
    }

    private void setTranformFromPoint()
    {
        if (coordinateSystem)
        {
            transform.position = position + coordinateSystem.transform.position;
        }
    }

    private void CheckSelection()
    {
        GameObject selectionManager = GameObject.Find("SelectionManager");
        if (selectionManager == null)
            return;

        if (selectionManager.GetComponent<ObjectSelect>().getSelectedObject() != gameObject)
            Select(false);
    }
}
