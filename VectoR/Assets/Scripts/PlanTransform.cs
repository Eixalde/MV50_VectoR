using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanTransform : MonoBehaviour
{

    // Location where the normal vector appear on the plan
    public enum Location { Middle, TopR, TopL, BottomR, BottomL }
    public Location _vectorLocation;

    // Plan object
    public GameObject _plan;

    // 3D vector object (prefab)
    public GameObject _vector3D;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        setTransformFromVector();
    }

    private Vector3 getOffset()
    {
        Vector3 positionOffset = Vector3.zero;
        float planScaleX = _plan.transform.localScale.x;
        float planScaleZ = _plan.transform.localScale.z;
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
        Vector3 positionVect = _vector3D.GetComponent<VectorTransform>().getPositionP1();
        transform.position = positionVect;
        _plan.transform.localPosition = -getOffset();

        // SET ORIENTATION
        Vector3 directionVect = _vector3D.GetComponent<VectorTransform>().getVectorDirection();
        Quaternion rotationPlan = Quaternion.FromToRotation(Vector3.up, directionVect);
        transform.rotation = rotationPlan;

    }
}