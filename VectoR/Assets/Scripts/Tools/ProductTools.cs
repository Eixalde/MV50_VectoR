using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class allowing to use dot and cross products
 */
public class ProductTools : MonoBehaviour
{
    // Prefab to represent the dot product area
    public GameObject _scalarPlane;
    public GameObject selectedVector;

    // Boolean concerning use of cross and dot products
    private bool usingDotProduct;
    private bool usingCrossProduct;

    // ToogGestion script
    private ToolGestion tg;

    // Selection manager
    private GameObject selectionManager;

    public void Start()
    {
        tg = GetComponent<ToolGestion>();
        selectionManager = GameObject.Find("SelectionManager");
    }

    /* Function called when two vectors are selected and the trigger for vectorial product is on. It needs the two gameobjects corresponding
    to the vectors. It creates a 3D Vector corresponding to the vectorial product of the two first vectors. */
    public void crossProductFromVector (GameObject vector_one, GameObject vector_two)
    {
        VectorTransform vt1 = vector_one.GetComponent<VectorTransform>();
        VectorTransform vt2 = vector_two.GetComponent<VectorTransform>();
        if (vt1 && vt2)
        {
            Vector3 vector1 = vt1.getVector();
            Vector3 vector2 = vt2.getVector();
            Vector3 vectorProductDirection = Vector3.Cross(vector1, vector2);


            VectorTool vt = gameObject.GetComponent<VectorTool>();
            if (vt)
            {
                vt.createVectorFrom2CoordinateSystemPoints(vt1.coordinateSystem, vt1.positionP1, vt1.positionP1 + vectorProductDirection);
                deselectProductTools();
            }
        }
    }


    /* Function called when two vectors are selected and the trigger for scalar product is on. It needs the two gameobjects corresponding
    to the vectors. It creates a rectangle that corresponds to a specific area, that is geometrically related to both vectors. */
    public void dotProductFromVectors (GameObject vector_one, GameObject vector_two)
    {
        VectorTransform vt1 = vector_one.GetComponent<VectorTransform>();
        VectorTransform vt2 = vector_two.GetComponent<VectorTransform>();
        if(vt1 && vt2)
        {
            Vector3 vector1 = vt1.getVector();
            Vector3 vector2 = vt2.getVector();

            float scalarProductValue = Vector3.Dot(vector1, vector2);
            float normV1 = Vector3.Magnitude(vector1);

            /* The normal is important because it is necessary for the placement of the area rectangle. */
            Vector3 vectorProductDirection = Vector3.Cross(vector1, vector2);
            Quaternion normalToArea = Quaternion.FromToRotation(Vector3.up, vectorProductDirection);

            /* Planes are squares by default, scaling is therefore needed to give the rectangle its correct dimensions. The length is the norm
            of the first vector and the height is the norm of the second vector times the angle between both angles. */
            Vector3 areaScale = new Vector3(normV1, 0.001f, scalarProductValue / normV1);

            // instanciate in world
            GameObject area = Instantiate(_scalarPlane, vt1.positionP1 + vt1.coordinateSystem.transform.position, normalToArea);
            area.transform.localScale = areaScale;

            PlaneLocation pl = area.GetComponentInChildren<PlaneLocation>();
            if (pl)
            {
                pl._vectorLocation = PlaneLocation.Location.TopR;
            }
            deselectProductTools();
        }
    }

    // Method called when clicking on dot Product Tool
    // Manage to use or not the tool depending on current selected object
    public void dotProduct()
    {
        if (tg)
        {
            tg.deselectAllTools();
        }

        if (selectionManager)
        {
            GameObject selectedobject = selectionManager.GetComponent<ObjectSelect>()?.getSelectedObject();
            VectorTransform vt = selectedobject.GetComponent<VectorTransform>();
            if (vt)
            {
                selectedVector = selectedobject;
                usingDotProduct = true;        
            }
        }
    }

    // Method called when clicking on dot Product Tool
    // Manage to use or not the tool depending on current selected object
    public void crossProduct()
    {
        if (tg)
        {
            tg.deselectAllTools();
        }
        GameObject selectionManager = GameObject.Find("SelectionManager");
        if (selectionManager)
        {
            GameObject selectedObject = selectionManager.GetComponent<ObjectSelect>()?.getSelectedObject();
            Debug.Log("selected : " + selectedObject);
            VectorTransform vt = selectedObject.GetComponent<VectorTransform>();
            if (vt)
            {
                selectedVector = selectedObject;
                usingCrossProduct = true;
            }
        }
    }

    public GameObject getSelectedVector()
    {
        return selectedVector;
    }

    public bool isUsingDot()
    {
        return usingDotProduct;
    }

    public bool isUsingCross()
    {
        return usingCrossProduct;
    }

    // Turn all boolean concerning cross and dot product
    public void deselectProductTools()
    {
        usingCrossProduct = false;
        usingDotProduct = false;
    }
}
