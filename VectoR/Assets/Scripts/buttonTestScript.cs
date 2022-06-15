using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonTestScript : MonoBehaviour
{
    public GameObject coordinateSystem;
    public GameObject tm;


    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("ToolManager");
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
}
