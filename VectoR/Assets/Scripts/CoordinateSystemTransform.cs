using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystemTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

    private void CheckSelection()
    {
        GameObject selectionManager = GameObject.Find("SelectionManager");
        if (selectionManager == null)
            return;
        
        if (selectionManager.GetComponent<ObjectSelect>().getSelectedObject().name != gameObject.name)
            Select(false);
    }

}
