using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTool : MonoBehaviour
{
    // Point prefab to instanciate
    public GameObject _3DPoint;

    private bool placingPoint = false;

    // Temporary coordinate system
    private GameObject tempCoordinateSystem;
    // Temporary point position
    private Vector3 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(placingPoint)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Converting mouse position to 3D coordinates
                tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        // PointTransform pt = _3DPoint.GetComponent<PointTransform>();
        // pt.CoordinateSystem = coordinateSystem;
        // pt.position = position;
    }

    // Create a point withour coordinates
    public void createPointFromNothing(GameObject coordinateSystem)
    {
        tempCoordinateSystem = coordinateSystem;
        placingPoint = true;
    }
}


