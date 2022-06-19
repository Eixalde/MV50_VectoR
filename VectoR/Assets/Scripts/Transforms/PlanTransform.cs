using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Change the tranform parameters in depends of the Plane attributes
 */
public class PlanTransform : MonoBehaviour
{
    // Plan object (prefab)
    public GameObject _plan;

    // 3D vector object (prefab)
    public GameObject _vector3D;

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
        setTransformFromVector();
    }

    // Select the object 
    public void Select(bool select)
    {
        _vector3D.SetActive(select);
        GetComponent<Outline>().enabled = select;
    }

    // Check if the plan is still selected
    private void CheckSelection()
    {
        if (selectionManager)
        {
            if (selectionManager.GetComponent<ObjectSelect>().getSelectedObject() != _vector3D && selectionManager.GetComponent<ObjectSelect>().getSelectedObject() != gameObject)
            {
                Select(false);
            }
        }
    }

    /*
     * Place, rotates and changes the Plan using the normal vector and coordinate
     */
    private void setTransformFromVector()
    {
        // SET POSITION
        VectorTransform vt = _vector3D.GetComponent<VectorTransform>();
        if (vt)
        {
            Vector3 positionVect = vt.getPositionP1();
            GameObject coordinateSystem = vt.coordinateSystem;

            if (coordinateSystem)
            {
                positionVect += coordinateSystem.transform.position;
            }

            transform.position = positionVect;
        }

        // SET ORIENTATION
        Vector3 directionVect = _vector3D.GetComponent<VectorTransform>().getVectorDirection();
        Quaternion rotationPlan = Quaternion.FromToRotation(Vector3.up, directionVect);
        transform.rotation = rotationPlan;

    }
}