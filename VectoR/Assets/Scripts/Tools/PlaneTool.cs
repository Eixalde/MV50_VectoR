using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Class allowing to create planes
 */
public class PlaneTool : MonoBehaviour
{
    // Plane prefab to instanciate
    public GameObject _3DPlan;

    private bool creatingPlane;

    // Boolean concerning the placement of the normal vector points
    private bool placingP1;
    private bool placingP2;

    // Temp normal vector starting and ending points
    private Vector3 tempP1;
    private Vector3 tempP2;

    // Coordinate system used to create plane
    public GameObject coordinateSystem;

    // First Vector selected when creating a Pane based on Vector(s)
    private GameObject selectedVector;

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
        // Creation of a Plane
        if (placingP1)
        {
            if (aPressed.isApress())
            {
                if(rightHandController)
                {
                    // Using right hand coordinates
                    tempP1 = rightHandController.transform.position;
                    placingP1 = false;
                    placingP2 = true;
                }
            }
        }
        else if (placingP2)
        {
            if (aPressed.isApress())
            {
                if (rightHandController)
                {
                    // Using right hand coordinates
                    tempP2 = rightHandController.transform.position; ;
                    placingP2 = false;
                    Vector3 temp = tempP2 - tempP1;
                    createPlanWithWorldVector(temp, tempP1, coordinateSystem);
                }
            }
        }
    }

    /*
     * Method start the creation of a plan using its normal vector
     */
    public void createPlanFromNothing(GameObject coordinate)
    {
        placingP1 = true;
        coordinateSystem = coordinate;
    }

    /*
     * Create a 3DPlane with _3DVector as normal vector
     */
    public void createPlanWith3DVector(GameObject _3Dvector)
    {
        Transform transform = new GameObject().transform;
        GameObject plan = Instantiate(_3DPlan, transform.position, transform.rotation);
        PlanTransform pt = plan.GetComponent<PlanTransform>();
        if(pt)
        {
            
            VectorTransform vtPlan = pt._vector3D.GetComponent<VectorTransform>();
            VectorTransform vtVect = _3Dvector.GetComponent<VectorTransform>();

            vtPlan.positionP1 = vtVect.positionP1;
            vtPlan.positionP2 = vtVect.positionP2;
            vtPlan.coordinateSystem = coordinateSystem;
            pt._vector3D.SetActive(true);

            GrabbableBehavior gbPlan = plan.GetComponent<GrabbableBehavior>();
            if (gbPlan)
            {
                gbPlan._coordSystem = coordinateSystem;
                TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                gbPlan.positions = positionText;

            }
            GrabbableBehavior gbVect = vtPlan.GetComponent<GrabbableBehavior>();
            if (gbVect)
            {
                gbVect._coordSystem = coordinateSystem;
                TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                gbVect.positions = positionText;
            }
        }
        if(selectionManager)
        {
            ObjectSelect os = selectionManager.GetComponent<ObjectSelect>();
            os.select(plan);
        }
        deselectPlaneTool();
    }

    // Create a Plan with two vectors a coordinate system and World point 
    public void createPlanWithWorldVector(Vector3 vector, Vector3 point, GameObject coordinateSystem)
    {
        Vector3 coordPoint = point - coordinateSystem.transform.position;
        createPlanWithCoordinateVector(vector, coordPoint, coordinateSystem);
    }

    /* 
     * Create a plan using a Vector, a Coordinate systeme Point and a Coordinate System
     */
    public void createPlanWithCoordinateVector(Vector3 vector, Vector3 point, GameObject coordinateSystem)
    {
        Transform transform = new GameObject().transform;
        GameObject plan = Instantiate(_3DPlan, transform.position, transform.rotation);
        PlanTransform pt = plan.GetComponent<PlanTransform>();
        if(pt)
        {
            VectorTransform vt = pt._vector3D.GetComponent<VectorTransform>();
            if (vt)
            {
                vt.coordinateSystem = coordinateSystem;
                vt.positionP1 = point;
                vt.positionP2 = point + vector;
                pt._vector3D.SetActive(true);

                GrabbableBehavior gb = plan.GetComponent<GrabbableBehavior>();
                if (gb)
                {
                    gb._coordSystem = coordinateSystem;
                    TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                    gb.positions = positionText;

                    foreach (GrabbableBehavior grabbable in pt.gameObject.GetComponentsInChildren<GrabbableBehavior>())
                    {
                        grabbable.positions = positionText;
                    }
                }
                GrabbableBehavior gbVect = vt.GetComponent<GrabbableBehavior>();
                if (gbVect)
                {
                    gbVect._coordSystem = coordinateSystem;
                    TextMesh positionText = GameObject.Find("Positions")?.GetComponent<TextMesh>();
                    gbVect.positions = positionText;
                }
            }      
        }

        if(selectionManager)
        {
            ObjectSelect os = selectionManager.GetComponent<ObjectSelect>();
            os.select(plan);
        }
        deselectPlaneTool();
    }

    /*
     * Create a plan using two non colinear vectors, a Coordinate system Point and a Coordinate System
     */
    public void createPlanWithTwoCoordinateVector(Vector3 vector1, Vector3 vector2, Vector3 point, GameObject coordinateSystem)
    {
        //Debug.Log("creating 2 vector plan");
        Vector3 vector = Vector3.Cross(vector1, vector2);
        createPlanWithCoordinateVector(vector, point, coordinateSystem);
        deselectPlaneTool();
    }


    // Method called when clicking on Plane Tool
    // Manage to use the right creation process depending on current selected object
    public void planeTool()
    {
        if (tg)
        {
            tg.deselectAllTools();
        }

        if (selectionManager)
        {
            GameObject selectedObject = selectionManager.GetComponent<ObjectSelect>()?.getSelectedObject();
            VectorTransform vt = selectedObject?.GetComponent<VectorTransform>();
            if (vt)
            {
                selectedVector = selectedObject;
                creatingPlane = true;
            }
            else
            {
                createPlanFromNothing(coordinateSystem);
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

    // Turn all boolean concerning plane creation to false
    public void deselectPlaneTool()
    {
        creatingPlane = false;
        placingP1 = false;
        placingP2 = false;
    }
}
