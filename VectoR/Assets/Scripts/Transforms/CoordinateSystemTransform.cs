using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystemTransform : MonoBehaviour
{
    GameObject selectionManager;

    // Start is called before the first frame update
    void Start()
    {
        selectionManager = GameObject.Find("SelectionManager");
    }

    // Update is called once per frame
    void Update()
    {
        CheckSelection();
    }

    public void setPosition(Vector3 newPosition)
    {
        if (GetComponent<Outline>().enabled)
            transform.position = newPosition;
    }

    public void Select(bool select)
    {
        GetComponent<Outline>().enabled = select;
    }

    // Check if the coordinate system is still selected
    private void CheckSelection()
    {  
        if (selectionManager.GetComponent<ObjectSelect>().getSelectedObject() != gameObject)
            Select(false);
    }
}
