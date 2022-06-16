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
        Debug.Log("gameobject : "+ gameObject.name);

        // Mouse clicked detection
        if (_isMouseDown)
        {
            // Using tools
            GameObject selectionManager = GameObject.Find("SelectionManager");
            GameObject toolManager = GameObject.Find("ToolManager");
            _isMouseDown = false;
            if(toolManager)
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
                    if (_mainObject.GetComponent<PointTransform>()!=null && selectedPoint!=null && gameObject != selectedPoint)
                    {
                        Debug.Log("try creat vect");

                        PointTransform pt1 = selectedPoint.GetComponent<PointTransform>();
                        PointTransform pt2 = gameObject.GetComponent<PointTransform>();
                        if (pt1!=null && pt2!=null)
                        {
                            vt.createVectorFrom2points(pt1.coordinateSystem, pt1.position, pt2.position);
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
                        // A CHANGER POUR VR
                        VectorTransform vt2 = gameObject.GetComponentInParent<VectorTransform>();
                        Debug.Log("vt1 : " + vt1 + " vt2 : " + vt2);
                        if (vt1 != null && vt2 != null)
                        {
                            pts.OnScalarProductTrigger(vt1.gameObject, vt2.gameObject);
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
                        // A CHANGER POUR VR
                        VectorTransform vt2 = gameObject.GetComponentInParent<VectorTransform>();
                        Debug.Log("vt1 : " + vt1 + " vt2 : " + vt2);
                        if (vt1 != null && vt2 != null)
                        {
                            pts.OnVectorProductTrigger(vt1.gameObject, vt2.gameObject);
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
                        // A CHANGER POUR VR
                        VectorTransform vt2 = gameObject.GetComponentInParent<VectorTransform>();
                        Debug.Log("vt1 : " + vt1 + " vt2 : " + vt2);
                        if (vt1 != null && vt2 != null)
                        {
                            Debug.Log("3");
                            pt.createPlanWithTwoVector(vt1.getVector(), vt2.getVector(), vt1.positionP1, vt1.CoordinateSystem);
                        }
                    }
                    else
                    {
                        Debug.Log("4");
                        pt.createPlanWith3DVector(selectedVector);
                    }
                }

            }
           

            if(_mainObject.GetComponent<PointTransform>())
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
                //Debug.Log("Object selected : " + gameObject.transform.parent?.gameObject.name);

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
        if (_mainObject.GetComponent<PointTransform>())
        {
            _mainObject.GetComponent<PointTransform>().setPosition(GetMouseAsWorldPoint() + mOffset);
        }
        else if (_mainObject.GetComponent<VectorTransform>() != null)
        {
            _mainObject.GetComponent<VectorTransform>().setPosition(GetMouseAsWorldPoint() + mOffset, gameObject.name);
        }
        // Set position of a CoordinateSystem object
        else if (_mainObject.GetComponent<CoordinateSystemTransform>() != null)
        {
            _mainObject.GetComponent<CoordinateSystemTransform>().setPosition(GetMouseAsWorldPoint() + mOffset);
        }
        // Set position of a Plan object
        else if (_mainObject.GetComponent<PlanTransform>())
        {
            _mainObject.GetComponent<PlanTransform>()._vector3D.GetComponent<VectorTransform>().setPosition(GetMouseAsWorldPoint() + mOffset, gameObject.name);
        }
        else
        {
            transform.position = GetMouseAsWorldPoint() + mOffset;
        }
    }
}