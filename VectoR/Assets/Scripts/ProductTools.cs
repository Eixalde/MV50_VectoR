using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTools : MonoBehaviour
{
    // Prefab to represent the dot product area
    public GameObject _scalarPlane;

    /* Function called when two vectors are selected and the trigger for vectorial product is on. It needs the two gameobjects corresponding
    to the vectors. It creates a 3D Vector corresponding to the vectorial product of the two first vectors. */
    public void OnVectorProductTrigger (GameObject vector_one, GameObject vector_two)
    {
        VectorTransform vtVector1 = vector_one.GetComponent<VectorTransform>();
        Vector3 vector1 = (Vector3)(vtVector1?.getVector());
        Vector3 vector2 = (Vector3)(vector_two.GetComponent<VectorTransform>()?.getVector());

        Vector3 vectorProductDirection = Vector3.Cross(vector1, vector2);

        VectorTool vt = this.GetComponent<VectorTool>();
        if (vt)
        {
            vt.createVectorFrom2points(vector_one.GetComponent<VectorTransform>()?.CoordinateSystem, vtVector1.positionP1, vtVector1.positionP1 + vectorProductDirection);
        }
    }


    /* Function called when two vectors are selected and the trigger for scalar product is on. It needs the two gameobjects corresponding
    to the vectors. It creates a rectangle that corresponds to a specific area, that is geometrically related to both vectors. */
    public void OnScalarProductTrigger (GameObject vector_one, GameObject vector_two)
    {
        VectorTransform vtVector1 = vector_one.GetComponent<VectorTransform>();
        Vector3 vector1 = (Vector3)(vtVector1?.getVector());
        Vector3 vector2 = (Vector3)(vector_two.GetComponent<VectorTransform>()?.getVector());

        float scalarProductValue = Vector3.Dot(vector1, vector2);
        float normV1 = Vector3.Magnitude(vector1);
        // float normV2 = Vector3.Magnitude(vector_two.transform.forward);

        /* The normal is important because it is necessary for the placement of the area rectangle. */
        Vector3 vectorProductDirection = Vector3.Cross(vector1, vector2);
        Quaternion normalToArea = Quaternion.FromToRotation(Vector3.up, vectorProductDirection);

        /* Planes are squares by default, scaling is therefore needed to give the rectangle its correct dimensions. The length is the norm
        of the first vector and the height is the norm of the second vector times the angle between both angles. */
        Vector3 areaScale = new Vector3(normV1, 0.001f, scalarProductValue/normV1);

        GameObject area = Instantiate(_scalarPlane, vtVector1.positionP1, normalToArea);
        area.transform.localScale = areaScale;
        PlaneLocation pl = area.GetComponentInChildren<PlaneLocation>();
        if (pl)
        {
            pl._vectorLocation = PlaneLocation.Location.TopR;
        }
            
    }
}
