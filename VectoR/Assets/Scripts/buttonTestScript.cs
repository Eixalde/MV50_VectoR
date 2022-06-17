using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonTestScript : MonoBehaviour
{
    public GameObject coordinateSystem;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Test method of creation of a vector with given coordinates
    public void test2pointVector()
    {
        VectorTool vt = GetComponent<VectorTool>();
        vt.createVectorFrom2CoordinateSystemPoints(coordinateSystem, new Vector3(0, 0, 0), new Vector3(1, 2, 1));
    }

    // Test method of creation of a vector from nothing
    public void testnopointVector()
    {
        VectorTool vt = GetComponent<VectorTool>();
        vt.createFromNothing(coordinateSystem);
    }
}
