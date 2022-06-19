using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Class allowing to create vectors
 */
public class VectorTool : MonoBehaviour
{
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
    // Coordinate system used to create vectors
    public GameObject coordinateSystem;

    // First Point selected when creating a vector based on two points
    private GameObject selectedPoint;

    // RightHandController
    private GameObject rightHandController;

    // Selection manager
    private GameObject selectionManager;

    // A button specific Inputs
    private APressedDelay aPressed;

    // ToogGestion script
    private ToolGestion tg;

    // Start is called before the first frame update
    void Start()
    {
        tg = GetComponent<ToolGestion>();
        aPressed = GetComponent<APressedDelay>();
        rightHandController = GameObject.Find("RightHand Controller");
        selectionManager = GameObject.Find("SelectionManager");
    }

    // Update is called once per frame
    void Update()
    {
        // Creation of a Vector from nothing
        // Placing start point
        if (placingP1)
        {
            if (aPressed.isApress())
            {
                // Getting rightHandPosition
                if(rightHandController)
                {
                    tempP1 = rightHandController.transform.position;
                    placingP1 = false;
                    placingP2 = true;
                }
            }
        }
        // Placing end point
        else if(placingP2)
        {
            if(aPressed.isApress())
            {
                // Getting rightHandPosition
                if(rightHandController)
                {
                    tempP2 = rightHandController.transform.position;
                    placingP2 = false;
                    createVectorFrom2WorldPoints(coordinateSystem, tempP1, tempP2);
                }
            }
        }
    }

   // Create a vector using a coordinate systeme and the world coordinate of two points p1 and p2
    public void createVectorFrom2WorldPoints(GameObject coordinateSystem, Vector3 p1, Vector3 p2)
    {
        if (coordinateSystem)
        {
            Vector3 posOffset = coordinateSystem.transform.position;
            Vector3 p1Coord = p1 - posOffset;
            Vector3 p2Coord = p2 - posOffset;
            createVectorFrom2CoordinateSystemPoints(coordinateSystem, p1Coord, p2Coord);
        }
    }


    //  Create a vector using a coordinate system and the Coordinate System coordinate of two points p1 and p2
    public void createVectorFrom2CoordinateSystemPoints(GameObject coordinateSystem, Vector3 p1, Vector3 p2)
    {
        Debug.Log("instanciate");
        Transform transform = new GameObject().transform;
        GameObject vector = Instantiate(_3DVector, transform.position, transform.rotation);
        VectorTransform vt = vector.GetComponent<VectorTransform>();
        if (vt)
        {
            vt.coordinateSystem = coordinateSystem;
            vt.positionP1 = p1;
            vt.positionP2 = p2;
            GrabbableBehavior gb = vector.GetComponent<GrabbableBehavior>();
            if (gb)
            {
                TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();

                gb._coordSystem = coordinateSystem;
                gb.positions = positionText;

                foreach(GrabbableBehavior grabbable in vt.gameObject.GetComponentsInChildren<GrabbableBehavior>())
                {
                    grabbable.positions = positionText;
                }
            }
        }
        deselectVectorTool();
    }

    // Create a Vector without points
    public void createFromNothing(GameObject coordinateSystem)
    {
        this.coordinateSystem = coordinateSystem;
        placingP1 = true;
    }

    // Method called when clicking on Vector Tool
    // Manage to use the right creation process depending on current selected object
    public void vectorTool()
    {
        if (tg)
        {
            tg.deselectAllTools();
        }


        if (selectionManager)
        {
            GameObject selectedobject = selectionManager.GetComponent<ObjectSelect>()?.getSelectedObject();

            // If a point is currently selected, we start the creation from two point process
            PointTransform pt = selectedobject?.GetComponent<PointTransform>();
            if (pt)
            {
                selectedPoint = selectedobject;
                creatingVector = true;
            }
            // Else we create from nothing
            else
            {
                createFromNothing(coordinateSystem);
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

    // Turn all boolean concerning vector creation to false
    public void deselectVectorTool()
    {
        creatingVector = false;
        placingP1 = false;
        placingP2 = false;
    }
}
