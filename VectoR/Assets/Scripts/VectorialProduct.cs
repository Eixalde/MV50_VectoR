using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorialProduct : MonoBehaviour
{
    private GameObject tempCoorSystem;
    /* Function called when two vectors are selected and the trigger
    for vectorial product is on. It needs the two gameobjects corresponding
    to the vectors. */
    public void OnVectorProductTrigger (GameObject vector_one, GameObject vector_two)
    {
        Vector3 vectorProductDirection = Vector3.Cross(vector_one.transform.forward, vector_two.transform.forward);
        Quaternion vectorProductRotation = Quaternion.FromToRotation(Vector3.up, vectorProductDirection);

        VectorTool vt = this.GetComponent<VectorTool>();
        vt.createVectorFrom2points(tempCoorSystem, vector_one.transform.position, vector_one.transform.position + vectorProductDirection);
    }
}
