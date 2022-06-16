using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum GrabType { None, Free, Snap };

[RequireComponent(typeof(Rigidbody))]
public class GrabbableBehavior : MonoBehaviour
{
    private Rigidbody rigidbody;
    private GameObject grabber;
    private bool wasKinematic;
    private bool isHeld = false;
    private bool hover;

    public GrabType grabType = GrabType.Free;
    public TextMesh positions;

    public GameObject _coordSystem = null;
    public InputActionAsset inputActions;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        wasKinematic = rigidbody.isKinematic;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("hover" + hover);
        // Using tools
        GameObject selectionManager = GameObject.Find("SelectionManager");
        GameObject toolManager = GameObject.Find("ToolManager");
        ObjectSelect os = selectionManager?.GetComponent<ObjectSelect>();
        
        if (toolManager)
        {

            VectorTool vt = toolManager.GetComponent<VectorTool>();
            Debug.Log("tool manager + ceat vect ? : " + vt.isCreatingVector());
            ProductTools pts = toolManager.GetComponent<ProductTools>();
            PlanTool pt = toolManager.GetComponent<PlanTool>();
            // Vector Tool

            if (vt.isCreatingVector())
            {
                Debug.Log("try creat vect");
                GameObject selectedPoint = vt.getSelectedPoint();
                if (_mainObject.GetComponent<PointTransform>() != null && selectedPoint != null && gameObject != selectedPoint)
                {
                    Debug.Log("try creat vect");

                    PointTransform pt1 = selectedPoint.GetComponent<PointTransform>();
                    if(hover)
                    {
                        if (os)
                        {
                            PointTransform pt2 = os.getSelectedObject().GetComponent<PointTransform>();
                            if (pt1 != null && pt2 != null)
                            {
                                vt.createVectorFrom2points(pt1.coordinateSystem, pt1.position, pt2.position);
                            }
                        }
                    }
                    else
                    {
                        vt.createFromNothing(vt.tempCoorDystem);
                    }
                }
            }
            // Dot Product
            else if (pts.isUsingDot())
            {
                Debug.Log("try dot");
                GameObject selectedVector = pts.getSelectedVector();
                if (_mainObject.GetComponent<VectorTransform>() != null && selectedVector != null && gameObject != selectedVector)
                {
                    VectorTransform vt1 = selectedVector.GetComponent<VectorTransform>();
                    if (hover)
                    {
                        // NORMALEMENT c'est changé
                        VectorTransform vt2 = os.getSelectedObject()?.GetComponent<VectorTransform>();
                        Debug.Log("vt1 : " + vt1 + " vt2 : " + vt2);
                        if (vt1 != null && vt2 != null)
                        {
                            pts.OnScalarProductTrigger(vt1.gameObject, vt2.gameObject);
                        }
                    }
                    
                }
            }
            else if (pts.isUsingCross())
            {
                Debug.Log("try cross");
                GameObject selectedVector = pts.getSelectedVector();
                if (_mainObject.GetComponent<VectorTransform>() != null && selectedVector != null && gameObject != selectedVector)
                {
                    VectorTransform vt1 = selectedVector.GetComponent<VectorTransform>();
                    if(hover)
                    {
                        // A CHANGER POUR VR
                        VectorTransform vt2 = os.getSelectedObject()?.GetComponent<VectorTransform>();
                        Debug.Log("vt1 : " + vt1 + " vt2 : " + vt2);
                        if (vt1 != null && vt2 != null)
                        {
                            pts.OnVectorProductTrigger(vt1.gameObject, vt2.gameObject);
                        }
                    }

                }
            }
            else if (pt.isCreatingPlane())
            {
                Debug.Log("1");
                GameObject selectedVector = pt.getSelectedVector();
                if (_mainObject.GetComponent<VectorTransform>() != null && selectedVector != null && _mainObject.GetComponent<VectorTransform>()?.gameObject != selectedVector)
                {
                    Debug.Log("2");
                    VectorTransform vt1 = selectedVector.GetComponent<VectorTransform>();
                    if (hover)
                    {
                        // A CHANGER POUR VR
                        VectorTransform vt2 = os.getSelectedObject()?.GetComponent<VectorTransform>();
                        Debug.Log("vt1 : " + vt1 + " vt2 : " + vt2);
                        if (vt1 != null && vt2 != null)
                        {
                            Debug.Log("3");
                            pt.createPlanWithTwoVector(vt1.getVector(), vt2.getVector(), vt1.positionP1, vt1.CoordinateSystem);
                        }
                    }
                    
                }
                else
                {
                    Debug.Log("4");
                    pt.createPlanWith3DVector(selectedVector);
                }
            }

        }

        if (isHeld)
        {
            printPositions();
            OnGrab();
        }




    }
 
    public void JustSelected()
    {
        
        //Debug.Log("Name : "+gameObject.name);
        //Debug.Log("Appuie sur A");
        InputAction Abutton = inputActions.FindActionMap("XRI RightHand").FindAction("A_Button");

        if (Abutton.IsPressed())
        {
            hover = true;
            //Debug.Log("Name : " + gameObject.name);
            OnSelected();
            printPositions();
        }
    }

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
            // Mouse clicked detection
            GameObject selectionManager = GameObject.Find("SelectionManager");
            Debug.Log("main pbject : " + _mainObject);
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
                //Debug.Log("CALL FUNCTION SELECT POINT");
                Debug.Log("Object selected : " + gameObject.transform.parent?.gameObject.name);

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
        //Debug.Log(" CURRENT GRAB OBJECT : " + gameObject.name + "\n OBJECT TRANSFORM: " + gameObject.transform.position);
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
