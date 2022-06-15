using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Mouse controller to detect mouse inputs
// Requires a collider to detect mouse click
public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody is necessary to detect mouse click
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.isKinematic = true;
        }
    }

    // Offset between the mouse and the game object center
    private Vector3 mOffset;

    // Main object (this, or parent) who manage selection and movement
    public GameObject _mainObject = null;

    // true if mouse left button is down
    private bool _isMouseDown = false;

    // Z coordinate calculate from 2D mouse cursor position
    private float mZCoord;

    // Detects when mouse left button is down
    void OnMouseDown()
    {
        _isMouseDown = true;
        mZCoord = Camera.main.WorldToScreenPoint(
            transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = transform.position - GetMouseAsWorldPoint();
    }

    // Detects when mouse left button is down
    private void OnMouseUp()
    {
        // Mouse clicked detection
        if (_isMouseDown)
        {
            GameObject selectionManager = GameObject.Find("SelectionManager");
            _isMouseDown = false;
            // Select a 3DVector object
            if (_mainObject.GetComponent<VectorTransform>() != null)
            {
                Debug.Log("CALL FUNCTION SELECT POINT");
                Debug.Log("Object selected : " + gameObject.transform.parent.gameObject.name);

                _mainObject.GetComponent<VectorTransform>().Select(gameObject);

                if (selectionManager != null)
                {
                    if (_mainObject.transform.parent.gameObject.name == "3DVector")
                        selectionManager.GetComponent<ObjectSelect>().select(_mainObject.transform.parent.gameObject);
                    else
                        selectionManager.GetComponent<ObjectSelect>().select(_mainObject);
                }
            }
            // Select a CoordinateSystem object
            else if (_mainObject.GetComponent<CoordinateSystemTransform>() != null)
            {
                _mainObject.GetComponent<CoordinateSystemTransform>().Select(true);
                if (selectionManager != null)
                {
                    selectionManager.GetComponent<ObjectSelect>().select(_mainObject);
                }
            }
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    void OnMouseDrag()
    {
        // Set position of a 3DVector object
        if (_mainObject.GetComponent<VectorTransform>() != null)
        {
            _mainObject.GetComponent<VectorTransform>().setPosition(GetMouseAsWorldPoint() + mOffset, gameObject.name);
        }
        // Set position of a CoordinateSystem object
        else if (_mainObject.GetComponent<CoordinateSystemTransform>() != null)
        {
            _mainObject.GetComponent<CoordinateSystemTransform>().setPosition(GetMouseAsWorldPoint() + mOffset);
        }
        else
        {
            transform.position = GetMouseAsWorldPoint() + mOffset;
        }
    }
}