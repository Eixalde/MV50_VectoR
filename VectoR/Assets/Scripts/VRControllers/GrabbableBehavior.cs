using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum GrabType { None, Free, Snap };

[RequireComponent(typeof(Rigidbody))]
/*
 * Class managing interaction between user inputs and the other scripts
 */
public class GrabbableBehavior : MonoBehaviour
{
    private bool isHeld = false;
    private bool hover;

    public GrabType grabType = GrabType.Free;
    public TextMesh positions;

    // Coordinate system of the object
    public GameObject _coordSystem;
    // 
    public InputActionAsset inputActions;

    // Selection manager
    private GameObject selectionManager;
    // Tool Manager
    private GameObject toolManager;

    InputAction Abutton;

    // Start is called before the first frame update
    void Start()
    {
        selectionManager = GameObject.Find("SelectionManager");
        toolManager = GameObject.Find("ToolManager");
        Abutton = inputActions?.FindActionMap("XRI RightHand")?.FindAction("A_Button");
        Debug.Log("Abutton" + Abutton);
    }

    // Update is called once per frame
    void Update()
    {
        // Using tools
        ObjectSelect os = selectionManager?.GetComponent<ObjectSelect>();
        
        if (toolManager)
        {
            VectorTool vt = toolManager.GetComponent<VectorTool>();
            APressedDelay aPressed = toolManager.GetComponent<APressedDelay>();
            ProductTools pts = toolManager.GetComponent<ProductTools>();
            PlaneTool pt = toolManager.GetComponent<PlaneTool>();

            // Vector Tool
            if (vt.isCreatingVector())
            {
                GameObject selectedPoint = vt.getSelectedPoint();
                if (_mainObject.GetComponent<PointTransform>() != null && selectedPoint != null && gameObject != selectedPoint)
                {
                    PointTransform pt1 = selectedPoint.GetComponent<PointTransform>();
                    if(hover)
                    {
                        if (os)
                        {
                            PointTransform pt2 = os.getSelectedObject().GetComponent<PointTransform>();
                            if (pt1 != null && pt2 != null)
                            {
                                vt.createVectorFrom2CoordinateSystemPoints(pt1.coordinateSystem, pt1.position, pt2.position);
                            }
                        }
                    }
                    else if (aPressed.isArelease())
                    {
                        vt.createFromNothing(vt.coordinateSystem);
                    }
                }
            }
            // Dot Product
            else if (pts.isUsingDot())
            {
                GameObject selectedVector = pts.getSelectedVector();
                if (_mainObject.GetComponent<VectorTransform>() != null && selectedVector != null && gameObject != selectedVector)
                {
                    VectorTransform vt1 = selectedVector.GetComponent<VectorTransform>();
                    if (hover)
                    {
                        VectorTransform vt2 = os.getSelectedObject()?.GetComponent<VectorTransform>();
                        if (vt1 != null && vt2 != null)
                        {
                            pts.dotProductFromVectors(vt1.gameObject, vt2.gameObject);
                        }
                    }
                    
                }
            }
            // Cross product
            else if (pts.isUsingCross())
            {
                GameObject selectedVector = pts.getSelectedVector();
                if (_mainObject.GetComponent<VectorTransform>() != null && selectedVector != null && gameObject != selectedVector)
                {
                    VectorTransform vt1 = selectedVector.GetComponent<VectorTransform>();
                    if(hover)
                    {
                        VectorTransform vt2 = os.getSelectedObject()?.GetComponent<VectorTransform>();
                        if (vt1 != null && vt2 != null)
                        {
                            pts.crossProductFromVector(vt1.gameObject, vt2.gameObject);
                        }
                    }

                }
            }
            // Plan tool
            else if (pt.isCreatingPlane())
            {
                GameObject selectedVector = pt.getSelectedVector();

                if (_mainObject.GetComponent<VectorTransform>() != null && selectedVector != null && gameObject != selectedVector)
                {
                    VectorTransform vt1 = selectedVector.GetComponent<VectorTransform>();
                    if (hover)
                    {
                        VectorTransform vt2 = os.getSelectedObject()?.GetComponent<VectorTransform>();
                        if (vt1 != null && vt2 != null)
                        {
                            pt.createPlanWithTwoCoordinateVector(vt1.getVector(), vt2.getVector(), vt1.positionP1, vt1.coordinateSystem);
                        }
                    }
                    else if (aPressed.isArelease())
                    {
                        pt.createPlanWith3DVector(selectedVector);
                    }
                }           
            }
        }

        if (isHeld)
        {
            printPositions();
            OnGrab();
        }
    }
 
    // Method called when a object is hovered
    public void HoverEnter()
    {

        if (Abutton.IsPressed())
        {
            hover = true;
            OnSelected();
            printPositions();
        }
    }

    // Method called when a object is exited
    public void HoverExited()
    {
        hover = false;
    }


    public void isHeldChange()
    {
        isHeld = !isHeld;
        if (isHeld)
            OnSelected();
    }


    private void printPositions()
    {
        Vector3 relativePosition = transform.position;
        if (_coordSystem != null)
            relativePosition -= _coordSystem.transform.position;
        if(positions)
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

    // Called when the object is selected
    public void OnSelected()
    {
            if (_mainObject.GetComponent<PointTransform>())
            {
                _mainObject.GetComponent<PointTransform>().Select(true);
                if (selectionManager)
                {
                    selectionManager.GetComponent<ObjectSelect>().select(_mainObject);
                }
            }

            // Select a 3DVector object
            if (_mainObject.GetComponent<VectorTransform>() != null)
            {
                _mainObject.GetComponent<VectorTransform>().Select(gameObject);

                if (selectionManager != null)
                {
                    if (_mainObject.transform.parent?.gameObject.GetComponent<VectorTransform>() != null)
                    {
                        selectionManager.GetComponent<ObjectSelect>().select(_mainObject.transform.parent.gameObject);
                    }
                    else
                        selectionManager.GetComponent<ObjectSelect>().select(_mainObject);
                }
            }
            // Select a CoordinateSystem object
            else if (_mainObject.GetComponent<CoordinateSystemTransform>())
            {
                _mainObject.GetComponent<CoordinateSystemTransform>().Select(true);
                if (selectionManager)
                {
                    selectionManager.GetComponent<ObjectSelect>().select(_mainObject);
                }
            }
            // Select a Plan object
            else if (_mainObject.GetComponent<PlanTransform>())
            {
                _mainObject.GetComponent<PlanTransform>().Select(true);

                if (selectionManager)
                {
                    selectionManager.GetComponent<ObjectSelect>().select(_mainObject);
                }
            }

    }


    public void OnGrab()
    {
        Vector3 position = GameObject.Find("RightHand Controller").transform.position;

        // Set position of a 3DVector object
        if (_mainObject.GetComponent<PointTransform>())
        {
            _mainObject.GetComponent<PointTransform>().setPosition(position);
        }
        else if (_mainObject.GetComponent<VectorTransform>() != null)
        {
            
            _mainObject.GetComponent<VectorTransform>().setPosition(position, gameObject.name);
        }
        // Set position of a CoordinateSystem object
        else if (_mainObject.GetComponent<CoordinateSystemTransform>() != null)
        {
            _mainObject.GetComponent<CoordinateSystemTransform>().setPosition(position);
        }
        // Set position of a Plan object
        else if (_mainObject.GetComponent<PlanTransform>())
        {
            _mainObject.GetComponent<PlanTransform>()._vector3D.GetComponent<VectorTransform>().setPosition(position, gameObject.name);
        }
        else
        {
            transform.position = position;
        }
    }
}
