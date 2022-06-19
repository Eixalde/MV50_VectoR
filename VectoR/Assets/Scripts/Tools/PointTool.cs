using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/*
 * Class allowing to create points
 */
public class PointTool : MonoBehaviour
{
    // Point prefab to instanciate
    public GameObject _3DPoint;

    // Boolean indication the tool is activated
    private bool placingPoint;

    // Coordinate system used to create points
    public GameObject coordinateSystem;

    // Temporary point position
    private Vector3 tempPosition;

    // A button specific Inputs
    private APressedDelay aPressed;

    // ToogGestion script
    private ToolGestion tg;

    // RightHandController
    private GameObject rightHandController;

    // Selection manager
    private GameObject selectionManager;



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
        // Creation of a point
        if (placingPoint)
        {
            if (aPressed.isApress())
            {
                if(rightHandController)
                {
                    // Getting right hand position
                    tempPosition = rightHandController.transform.position;
                    deselectPointTool();
                    createPointFromWorldPoint(coordinateSystem, tempPosition);
                }
            }
        }
    }

    // Create Point based on World coordinates and a coordinate system
    public void createPointFromWorldPoint(GameObject coordinateSystem, Vector3 position)
    {
        Transform transform = new GameObject().transform;
        GameObject point = Instantiate(_3DPoint, transform.position, transform.rotation);
        Destroy(transform.gameObject);

        PointTransform pt = point.GetComponent<PointTransform>();
        if (pt)
        {
            pt.coordinateSystem = coordinateSystem;
            pt.setPosition(position);
            GrabbableBehavior gb = point.GetComponent<GrabbableBehavior>();
            if(gb)
            {
                gb._coordSystem = coordinateSystem;
                Debug.Log("text pos ? " + GameObject.Find("Positions"));
                gb.positions = GameObject.Find("Positions")?.GetComponent<TextMesh>();
            }
        }

        Debug.Log("final position " + point.transform.position);

    }

    // Create a point without coordinates
    public void createPointFromNothing(GameObject coordinateSystem)
    {
        this.coordinateSystem = coordinateSystem;
        placingPoint = true;
    }

    // Method called when clicking on Point Tool
    public void pointTool()
    {
        if (tg)
        {
            tg.deselectAllTools();
        }

        createPointFromNothing(coordinateSystem);
    }

    // Turn all boolean involving point creation to false
    public void deselectPointTool()
    {
        placingPoint = false;
    }
}


