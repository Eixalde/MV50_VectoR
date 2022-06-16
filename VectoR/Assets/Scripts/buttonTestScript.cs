using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonTestScript : MonoBehaviour
{
    public GameObject coordinateSystem;
    private GameObject tm;
    public GameObject vector4plan;    
    public GameObject vector4plan1;    
    public GameObject vector4plan2;


    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("ToolManager");
        Debug.Log(tm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Test method of creation of a vector with given coordinates
    public void test2pointVector()
    {
        VectorTool vt = tm.GetComponent<VectorTool>();
        vt.createVectorFrom2points(coordinateSystem, new Vector3(0, 0, 0), new Vector3(1, 2, 1));
    }

    // Test method of creation of a vector from nothing
    public void testnopointVector()
    {
        VectorTool vt = tm.GetComponent<VectorTool>();
        vt.createFromNothing(coordinateSystem);
    }
    
    public void test1vectPlan()
    {

        PlanTool pt = tm.GetComponent<PlanTool>();
        pt.createPlanWith3DVector(vector4plan);
    }
    public void test2vectPlan()
    {

        PlanTool pt = tm.GetComponent<PlanTool>();
        Vector3 vect1 = vector4plan1.GetComponent<VectorTransform>().getVector();
        Vector3 vect2 = vector4plan2.GetComponent<VectorTransform>().getVector();
        Vector3 point = vector4plan1.GetComponent<VectorTransform>().positionP1;
        GameObject cs = vector4plan1.GetComponent<VectorTransform>().CoordinateSystem;
        pt.createPlanWithTwoVector(vect1, vect2, point, coordinateSystem);
    }

    public void testnothingplan()
    {
        PlanTool pt = tm.GetComponent<PlanTool>();
        pt.createPlanFromNothing(coordinateSystem);
    }

    public void crosstest()
    {
        ProductTools pt = tm.GetComponent<ProductTools>();
        pt.OnVectorProductTrigger(vector4plan1, vector4plan2);
    }
    
    public void dottest()
    {
        ProductTools pt = tm.GetComponent<ProductTools>();
        pt.OnScalarProductTrigger(vector4plan1, vector4plan2);
    }
}
