using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PointTool : MonoBehaviour
{
    // Point prefab to instanciate
    public GameObject _3DPoint;

    private bool placingPoint = false;

    // Temporary coordinate system
    public GameObject tempCoordinateSystem;
    // Temporary point position
    private Vector3 tempPosition;

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

        if (placingPoint)
        {
            if (aPressed.isApressed())
            {
                // Getting right hand position
                tempPosition = GameObject.Find("RightHand Controller").transform.position;
                placingPoint = false;
                createPoint(tempCoordinateSystem, tempPosition);

            }
        }
    }

    // Create Point based on coordinates and a coordinate system
    public void createPoint(GameObject coordinateSystem, Vector3 position)
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

    // Create a point withour coordinates
    public void createPointFromNothing(GameObject coordinateSystem)
    {
        tempCoordinateSystem = coordinateSystem;
        placingPoint = true;
    }

    public void pointTool()
    {
        if (tg)
        {
            tg.deselectAllTools();
        }
        createPointFromNothing(tempCoordinateSystem);
    }

    public void deselectPointTool()
    {
        placingPoint = false;
    }
}


