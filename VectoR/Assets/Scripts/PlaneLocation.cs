using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneLocation : MonoBehaviour
{
    // Location where the normal vector appear on the plan
    public enum Location { Middle, TopR, TopL, BottomR, BottomL }
    public Location _vectorLocation;

    // Base local position when the object is instanciate
    public Vector3 baseLocalPos;


    // Start is called before the first frame update
    void Start()
    {
        // Getting the base local position
        baseLocalPos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        setTransformFromVector();
    }

    /*
     * Calcul the offset depennding on Location type
     */
    private Vector3 calculOffset()
    {
        Vector3 positionOffset = Vector3.zero;
        float planScaleX = gameObject.transform.localScale.x;
        float planScaleZ = gameObject.transform.localScale.z;

        switch (_vectorLocation)
        {
            case Location.TopR:
                positionOffset = new Vector3(-planScaleX / 2, 0, -planScaleZ / 2);
                break;
            case Location.TopL:
                positionOffset = new Vector3(planScaleX / 2, 0, -planScaleZ / 2);
                break;
            case Location.BottomR:
                positionOffset = new Vector3(-planScaleX / 2, 0, planScaleZ / 2);
                break;
            case Location.BottomL:
                positionOffset = new Vector3(planScaleX / 2, 0, planScaleZ / 2);
                break;
        }

        return positionOffset;
    }

    private void setTransformFromVector()
    {

        // SET POSITION
        gameObject.transform.localPosition = baseLocalPos - calculOffset();

        // SET ORIENTATION - does not seems usefull
        // Vector3 directionVect = gameObject.transform.up;
        // Quaternion rotationPlan = Quaternion.FromToRotation(Vector3.up, directionVect);
        // transform.rotation = rotationPlan;

    }
}
