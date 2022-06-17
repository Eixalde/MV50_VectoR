using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanTool : MonoBehaviour
{
    // Prefab to instanciate
    public GameObject _3DPlan;

    // Boolean concerning le pacement of the normal vector points
    private bool placingP1;
    private bool placingP2;

    // Temp normal vector starting and ending points
    private Vector3 tempP1;
    private Vector3 tempP2;

    // Temporary Coordinate System
    public GameObject tempCoordinateSystem;

    private GameObject selectedVector;
    private bool creatingPlane;
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

        // Creation of a Plane
        if (placingP1)
        {
            if (aPressed.isApressed())
            {
                // Using right hand coordinates
                tempP1 = GameObject.Find("RightHand Controller").transform.position;
                placingP1 = false;
                placingP2 = true;

            }
        }
        else if (placingP2)
        {
            if (aPressed.isApressed())
            {
                // Using right hand coordinates
                tempP2 = GameObject.Find("RightHand Controller").transform.position; ;
                placingP2 = false;
                Vector3 temp = tempP2 - tempP1;
                createPlanWithWorldVector(temp, tempP1, tempCoordinateSystem);
            }
        }
    }

    /*
     * Method start the creation of a plan using its normal vector
     */
    public void createPlanFromNothing(GameObject coordinate)
    {
        placingP1 = true;
        tempCoordinateSystem = coordinate;
        creatingPlane = false;
    }

    /*
     * Create a 3DPlane with _3DVector as normal vector
     */
    public void createPlanWith3DVector(GameObject _3Dvector)
    {
        //Debug.Log("creating 1 vector plan");
        Transform transform = new GameObject().transform;
        GameObject plan = Instantiate(_3DPlan, transform.position, transform.rotation);
        PlanTransform pt = plan.GetComponent<PlanTransform>();
        if(pt)
        {
            
            VectorTransform vtPlan = pt._vector3D.GetComponent<VectorTransform>();
            VectorTransform vtVect = _3Dvector.GetComponent<VectorTransform>();

            vtPlan.positionP1 = vtVect.positionP1;
            vtPlan.positionP2 = vtVect.positionP2;
            vtPlan.coordinateSystem = tempCoordinateSystem;
            pt._vector3D.SetActive(true);

            GrabbableBehavior gbPlan = plan.GetComponent<GrabbableBehavior>();
            if (gbPlan)
            {
                gbPlan._coordSystem = tempCoordinateSystem;
                Debug.Log("text pos ? " + GameObject.Find("Positions"));
                TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                gbPlan.positions = positionText;

            }
            GrabbableBehavior gbVect = vtPlan.GetComponent<GrabbableBehavior>();
            if (gbVect)
            {
                gbVect._coordSystem = tempCoordinateSystem;
                Debug.Log("text pos ? " + GameObject.Find("Positions"));
                TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                gbVect.positions = positionText;
            }
        }
        ObjectSelect os = GameObject.Find("SelectionManager").GetComponent<ObjectSelect>();
        os.select(plan);

        creatingPlane = false;
    }

    public void createPlanWithWorldVector(Vector3 vector, Vector3 point, GameObject coordinateSystem)
    {
        Vector3 coordPoint = point - coordinateSystem.transform.position;
        createPlanWithCoordinateVector(vector, coordPoint, coordinateSystem);
    }

    /* 
     * Create a plan using a Vector, a Point and a Coordinate System
     */
    public void createPlanWithCoordinateVector(Vector3 vector, Vector3 point, GameObject coordinateSystem)
    {
        //Debug.Log("creating 1 vector plan");
        Transform transform = new GameObject().transform;
        GameObject plan = Instantiate(_3DPlan, transform.position, transform.rotation);
        PlanTransform pt = plan.GetComponent<PlanTransform>();
        if(pt)
        {
            
            VectorTransform vt = pt._vector3D.GetComponent<VectorTransform>();
            if (vt)
            {
                // Setting point from world pos to coord pos
                vt.coordinateSystem = coordinateSystem;
                vt.positionP1 = point;
                vt.positionP2 = point + vector;

                GrabbableBehavior gb = plan.GetComponent<GrabbableBehavior>();
                if (gb)
                {
                    gb._coordSystem = coordinateSystem;
                    Debug.Log("text pos ? " + GameObject.Find("Positions"));
                    TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                    gb.positions = positionText;
                    Debug.Log("for each ? " + pt.gameObject.GetComponentsInChildren<GrabbableBehavior>().Length);
                    foreach (GrabbableBehavior grabbable in pt.gameObject.GetComponentsInChildren<GrabbableBehavior>())
                    {
                        grabbable.positions = positionText;
                    }
                }
                GrabbableBehavior gbVect = vt.GetComponent<GrabbableBehavior>();
                if (gbVect)
                {
                    gbVect._coordSystem = coordinateSystem;
                    Debug.Log("text pos ? " + GameObject.Find("Positions"));
                    TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                    gbVect.positions = positionText;
                }
            }      
        }
        ObjectSelect os = GameObject.Find("SelectionManager").GetComponent<ObjectSelect>();
        os.select(plan);
        creatingPlane = false;
    }
    /*
     * Create a plan using two non colinear vectors, a Point and a Coordonate System
     */
    public void createPlanWithTwoCoordinateVector(Vector3 vector1, Vector3 vector2, Vector3 point, GameObject coordinateSystem)
    {
        //Debug.Log("creating 2 vector plan");
        Vector3 vector = Vector3.Cross(vector1, vector2);
        createPlanWithCoordinateVector(vector, point, coordinateSystem);
        creatingPlane = false;
    }

    public void planTool()
    {
        if (tg)
        {
            tg.deselectAllTools();
        }
        GameObject selectionManager = GameObject.Find("SelectionManager");
        if (selectionManager)
        {
            GameObject selectedObject = selectionManager.GetComponent<ObjectSelect>()?.getSelectedObject();
            Debug.Log("selected : " + selectedObject);
            VectorTransform vt = selectedObject.GetComponent<VectorTransform>();
            if (vt)
            {
                selectedVector = selectedObject;
                creatingPlane = true;
            }
            else
            {
                createPlanFromNothing(tempCoordinateSystem);
            }
        }
    }

    public GameObject getSelectedVector()
    {
        return selectedVector;
    }

    public bool isCreatingPlane()
    {
        return creatingPlane;
    }

    public void deselectPlaneTool()
    {
        creatingPlane = false;
        placingP1 = false;
        placingP2 = false;
}
}
