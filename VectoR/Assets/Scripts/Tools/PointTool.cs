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



    // Start is called before the first frame update
    void Start()
    {
        aPressed = GetComponent<APressedDelay>();
    }

    // Update is called once per frame
    void Update()
    {
        InputAction Abutton = inputActions.FindActionMap("XRI RightHand").FindAction("A_Button");
        Debug.Log("Placing point : " + placingPoint  + "A pressed : " + aPressed.debugIsAPressed());
        if (placingPoint)
        {
            if (aPressed.isApressed())
            {
                // Converting mouse position to 3D coordinates
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
        // Commented for debug purpose
        PointTransform pt = _3DPoint.GetComponent<PointTransform>();
        if (pt)
        {
            pt.coordinateSystem = coordinateSystem;
            pt.position = position;
        }
        
    }

    // Create a point withour coordinates
    public void createPointFromNothing(GameObject coordinateSystem)
    {
        tempCoordinateSystem = coordinateSystem;
        placingPoint = true;
    }

    public void pointTool()
    {
        createPointFromNothing(tempCoordinateSystem);
    }
}


