using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrabType { None, Free, Snap };

[RequireComponent(typeof(Rigidbody))]
public class GrabbableBehavior : MonoBehaviour
{
    private Rigidbody rigidbody;
    private GameObject grabber;
    private bool wasKinematic;
    private bool isHeld = false;
    public GrabType grabType = GrabType.Free;
    public TextMesh positions;

    public GameObject _coordSystem = null;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        wasKinematic = rigidbody.isKinematic;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHeld)
        {
            Debug.Log("isHeld");
            printPositions();
            OnGrab();
        }
    }
    public void isHeldChange()
    {
        isHeld = !isHeld;
        if (isHeld)
            OnSelected();
    }
    private void printPositions()
    {
        Debug.Log("PRINT POSITION");
        Vector3 relativePosition = transform.position;
        if (_coordSystem != null)
            relativePosition -= _coordSystem.transform.position;
        positions.text = name + "\nX = " + relativePosition.x + "\nY = " + relativePosition.y + "\nZ = " + relativePosition.z;
    }
    public void addXPosition()
    {
        transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        printPositions();
    }
    public void removeXPosition()
    {
        transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        printPositions();
    }
    public void addYPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        printPositions();
    }
    public void removeYPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        printPositions();
    }
    public void addZPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        printPositions();
    }
    public void removeZPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        printPositions();
    }

    // Main object (this, or parent) who manage selection and movement
    public GameObject _mainObject = null;

    public void OnSelected()
    {

        // SELECT OBJECT
        GameObject selectionManager = GameObject.Find("SelectionManager");
        // Select a 3DVector object
        if (_mainObject.GetComponent<VectorTransform>() != null)
        {
            //Debug.Log("Object selected : " + transform.parent.gameObject.name);

            _mainObject.GetComponent<VectorTransform>().Select(gameObject);

            if (selectionManager != null)
            {
                //if (_mainObject.transform.parent.gameObject.name == "3DVector")
                //    selectionManager.GetComponent<ObjectSelect>().select(_mainObject.transform.parent.gameObject);
                //else
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

    public void OnGrab()
    {
        // MOVE OBJECT
        // Set position of a 3DVector object
        if (_mainObject.GetComponent<VectorTransform>() != null)
        {
            _mainObject.GetComponent<VectorTransform>().setPosition(transform.position, gameObject.name);
        }
        // Set position of a CoordinateSystem object
        else if (_mainObject.GetComponent<CoordinateSystemTransform>() != null)
        {
            _mainObject.GetComponent<CoordinateSystemTransform>().setPosition(transform.position);
        }
        else
        {
            transform.position = transform.position;
        }

    }
}
