using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTool : MonoBehaviour
{
    private bool firstPointPlaced;
    // Vector prefab to instanciate
    public GameObject _3DVector;
    private bool creatingVector;

    // Boolean concerning le pacement of the vector points
    private bool placingP1 = false;
    private bool placingP2 = false;

    // Temporary startPoint position
    private Vector3 tempP1;
    // Temporary endPoint position
    private Vector3 tempP2;
    // Temporary coordinate system
    public GameObject tempCoorDystem;


    private GameObject selectedPoint;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Creation of a Vector
        if(placingP1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Converting mouse position to 3D coordinates
                tempP1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                placingP1 = false;
                placingP2 = true;

            }
        }else if(placingP2)
        {
            if(Input.GetMouseButtonDown(0))
            {
                // Converting mouse position to 3D coordinates
                tempP2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                placingP2 = false;
                createVectorFrom2points(tempCoorDystem, tempP1, tempP2);
            }
        }
    }

    // Create a vector based on its startPoint p1 and endPoint p2
    public void createVectorFrom2points(GameObject coordinateSystem, Vector3 p1, Vector3 p2)
    {
        Debug.Log("instanciate");
        Transform transform = new GameObject().transform;
        GameObject vector = Instantiate(_3DVector, transform.position, transform.rotation);
        VectorTransform vt = vector.GetComponent<VectorTransform>();
        if (vt)
        {
            vt.CoordinateSystem = coordinateSystem;
            vt.positionP1 = p1;
            vt.positionP2 = p2;
        }
        creatingVector = false;
    }

    // Create a Vector without points
    public void createFromNothing(GameObject coordinateSystem)
    {
        tempCoorDystem = coordinateSystem;
        placingP1 = true;
    }

    public void vectorTool()
    {
        GameObject selectionManager = GameObject.Find("SelectionManager");
        if (selectionManager)
        {
            GameObject selectedobject = selectionManager.GetComponent<ObjectSelect>()?.getSelectedObject();
            PointTransform pt = selectedobject.GetComponent<PointTransform>();
            if (pt)
            {
                selectedPoint = selectedobject;
                Debug.Log("????");

                creatingVector = true;
            }
            else
            {
                createFromNothing(tempCoorDystem);
            }

        }
    }

    public GameObject getSelectedPoint()
    {
        return selectedPoint;
    }

    public bool isCreatingVector()
    {
        return creatingVector;
    }
}
