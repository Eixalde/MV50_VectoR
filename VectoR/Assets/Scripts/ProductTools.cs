using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTools : MonoBehaviour
{
    public GameObject _scalarPlane;
    private GameObject tempCoorSystem;
    /* Function called when two vectors are selected and the trigger for vectorial product is on. It needs the two gameobjects corresponding
    to the vectors. It creates a 3D Vector corresponding to the vectorial product of the two first vectors. */
    public void OnVectorProductTrigger (GameObject vector_one, GameObject vector_two)
    {
        Vector3 vectorProductDirection = Vector3.Cross(vector_one.transform.forward, vector_two.transform.forward);

        VectorTool vt = this.GetComponent<VectorTool>();
        vt.createVectorFrom2points(tempCoorSystem, vector_one.transform.position, vector_one.transform.position + vectorProductDirection);
    }

    /* Smaller function used to get only the quaternion of the normal vector. Essentially used for the placement of the plane in the Scalar Product bit. */
    public Quaternion RotationVectorProduct (GameObject vector_one, GameObject vector_two)
    {
        Vector3 vectorProductDirection = Vector3.Cross(vector_one.transform.forward, vector_two.transform.forward);
        Quaternion vectorProductRotation = Quaternion.FromToRotation(Vector3.up, vectorProductDirection);

        return vectorProductRotation;
    }

    /* Function called when two vectors are selected and the trigger for scalar product is on. It needs the two gameobjects corresponding
    to the vectors. It creates a rectangle that corresponds to a specific area, that is geometrically related to both vectors. */
    public void OnScalarProductTrigger (GameObject vector_one, GameObject vector_two)
    {
        float scalarProductValue = Vector3.Dot(vector_one.transform.forward, vector_two.transform.forward);
        float normV1 = Vector3.Magnitude(vector_one.transform.forward);
        float normV2 = Vector3.Magnitude(vector_two.transform.forward);

        /* The normal is important because it is necessary for the placement of the area rectangle. */
        Quaternion normalToArea = RotationVectorProduct(vector_one, vector_two);

        VectorTransform vt = vector_one.GetComponent<VectorTransform>();

        /* Planes are squares by default, scaling is therefore needed to give the rectangle its correct dimensions. The length is the norm
        of the first vector and the height is the norm of the second vector times the angle between both angles. */
        Vector3 areaScale = new Vector3(normV1, 0, scalarProductValue/normV1);

        GameObject area = Instantiate(_scalarPlane, vt.positionP1, normalToArea);
        area.transform.localScale = areaScale;
    }
}
