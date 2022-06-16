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
        CheckSelection();
        setTransformFromVector();
    }
    public void Select(bool select)
    {
        _vector3D.SetActive(true);
        GetComponent<Outline>().enabled = select;
    }

    private void CheckSelection()
    {
        GameObject selectionManager = GameObject.Find("SelectionManager");
        if (selectionManager == null)
            return;

        if (selectionManager.GetComponent<ObjectSelect>().getSelectedObject().name != _vector3D.name && selectionManager.GetComponent<ObjectSelect>().getSelectedObject().name != gameObject.name)
        {
            Select(false);
            _vector3D.SetActive(false);
        }
            
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
        VectorTransform vt = _vector3D.GetComponent<VectorTransform>();
        if (vt)
        {
            Vector3 positionVect = vt.getPositionP1();
            Vector3 pos = positionVect;
            GameObject coordinateSystem = vt.CoordinateSystem;

            if (coordinateSystem != null)
            {
                pos += coordinateSystem.transform.position;
            }
            transform.position = pos;

        }


        //_plan.transform.localPosition = -getOffset();

        // SET ORIENTATION
        Vector3 directionVect = _vector3D.GetComponent<VectorTransform>().getVectorDirection();
        Quaternion rotationPlan = Quaternion.FromToRotation(Vector3.up, directionVect);
        transform.rotation = rotationPlan;

    }
}