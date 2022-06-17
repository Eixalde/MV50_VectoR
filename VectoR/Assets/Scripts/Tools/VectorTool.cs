using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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

    public InputActionAsset inputActions;

    private APressedDelay aPressed;
    private ToolGestion tg;

    // Start is called before the first frame update
    void Start()
    {
        tg = GetComponent<ToolGestion>();
        aPressed = GetComponent<APressedDelay>();
    }

    // Update is called once per frame
    void Update()
    {
        InputAction Abutton = inputActions.FindActionMap("XRI RightHand").FindAction("A_Button");

        // Creation of a Vector
        if (placingP1)
        {
            if (aPressed.isApressed())
            {
                // Getting rightHandPosition
                tempP1 = GameObject.Find("RightHand Controller").transform.position;
                placingP1 = false;
                placingP2 = true;

            }
        }else if(placingP2)
        {
            if(aPressed.isApressed())
            {
                // Getting rightHandPosition
                tempP2 = GameObject.Find("RightHand Controller").transform.position;
                placingP2 = false;
                createVectorFrom2WorldPoints(tempCoorDystem, tempP1, tempP2);
            }
        }
    }

    public void createVectorFrom2WorldPoints(GameObject coordinateSystem, Vector3 p1, Vector3 p2)
    {
        Debug.Log("wesh ? " + coordinateSystem);
        if (coordinateSystem)
        {
            Vector3 posOffset = coordinateSystem.transform.position;
            Vector3 p1Coord = p1 - posOffset;
            Vector3 p2Coord = p2 - posOffset;
            createVectorFrom2CoordinateSystemPoints(coordinateSystem, p1Coord, p2Coord);
        }
    }


    // Create a vector based on its startPoint p1 and endPoint p2
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
                gb._coordSystem = coordinateSystem;
                Debug.Log("text pos ? " + GameObject.Find("Positions"));
                TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                gb.positions = positionText;
                Debug.Log("for each ? " + vt.gameObject.GetComponentsInChildren<GrabbableBehavior>().Length);
                foreach(GrabbableBehavior grabbable in vt.gameObject.GetComponentsInChildren<GrabbableBehavior>())
                {
                    Debug.Log("grabbble : " + grabbable.positions);
                    Debug.Log("positionText " + positionText);

                    grabbable.positions = positionText;
                }
            }
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
        if (tg)
        {
            tg.deselectAllTools();
            Debug.Log("deselect All");
        }
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

    public void deselectVectorTool()
    {
        creatingVector = false;
        placingP1 = false;
        placingP2 = false;
    }
}
