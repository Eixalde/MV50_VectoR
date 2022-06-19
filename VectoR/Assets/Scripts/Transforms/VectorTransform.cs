using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Change the tranform parameters in depends of the Vector attributes
 */
public class VectorTransform : MonoBehaviour
{
    // Position of the fist point using coordinateSystem
    public Vector3 positionP1;
    // Position of the second point using coordinateSystem
    public Vector3 positionP2;
    // Coordinate system used for positioning the vector
    public GameObject coordinateSystem;

    // Model of the axis of the vector 
    public GameObject axis;

    // Model of the arrowHead of the vector 
    public GameObject arrowHead;

    // List of the movable points of the vector
    public List<GameObject> _movablePoints;

    // Material of the object
    public Material pointMaterial;
    public Material selectedPointMaterial;

    // Vector of the game object's position
    private Vector3 _vectorDirection;

    // ID of the selected object of the vector
    private int selectedID = -1;

    // Selection manager
    private GameObject selectionManager;


    private void Start()
    {
        selectionManager = GameObject.Find("SelectionManager");
    }

    // Update is called once per frame
    void Update()
    {
            CheckSelection();
            setTransformFromPoints();
    }

    // Select a part of the object and outline it
    public void Select(GameObject objectSelected)
    {
        if (objectSelected == null)
        {
            GetComponent<Outline>().enabled = false;
            showPoints(false);
            selectedID = -1;
            
            return;
        }

        GetComponent<Outline>().enabled = true;
        if (objectSelected.name == "P1")
        {
            selectedID = 0;
            showSelectedPoint(objectSelected);
        }
        else if (objectSelected.name == "P2")
        {
            selectedID = 1;
            showSelectedPoint(objectSelected);
        }
        else if (objectSelected.name == "PM")
        {
            selectedID = 3;
            showSelectedPoint(objectSelected);
        }
        else if (objectSelected.name == "VectorAxis")
        {
            if (selectedID != -1)
                showSelectedPoint(_movablePoints[2]);
            else
                showPoints(true);
            selectedID = 3;
        }
        else if (objectSelected.name == gameObject.name)
        {
            selectedID = 4;
            showPoints(true);
        }
    }

    // Set position of the selected part of the gameobject
    public void setPosition(Vector3 newPosition, string name)
    {
        float distance = Mathf.Sqrt(
          Mathf.Pow(positionP2.x - positionP1.x, 2)
          + Mathf.Pow(positionP2.y - positionP1.y, 2)
          + Mathf.Pow(positionP2.z - positionP1.z, 2)
          );


        if (selectedID == 0 && name == "P1")
        {
            positionP1 = newPosition - coordinateSystem.transform.position;
        }
        else if (selectedID == 1 && name == "P2")
        {
            if (positionP2 != newPosition - coordinateSystem.transform.position)
            positionP2 = newPosition - coordinateSystem.transform.position;
        }
        else if (selectedID == 3 && (name == "VectorAxis" || name == "PM"))
        {
            Vector3 middlePoint = (positionP1 + positionP2) / 2;
            Vector3 translation = newPosition - middlePoint;
            positionP1 += translation - coordinateSystem.transform.position;
            positionP2 += translation - coordinateSystem.transform.position;
        }
    }

    // Set position of P1 from word position
    public void setPositionP1(Vector3 newPosition)
    {
        positionP1 = newPosition - coordinateSystem.transform.position;
    }

    // Set position of P2 from word position
    public void setPositionP2(Vector3 newPosition)
    {
        positionP2 = newPosition - coordinateSystem.transform.position;
    }


    // Place, rotates and changes the length of the vector
    // using the points P1 and P2 coordinates and the coordinate system
    private void setTransformFromPoints()
    {
        Vector3 pos1 = positionP1;
        Vector3 pos2 = positionP2;

        if (coordinateSystem)
        {
            positionP1 += coordinateSystem.transform.position;
            positionP2 += coordinateSystem.transform.position;
        }


        GameObject P1 = _movablePoints[0];
        GameObject P2 = _movablePoints[1];
        GameObject P3 = _movablePoints[2];

        // Set POSITION 
        // The coordinate system is placed at the center
        // of the Game object
        Vector3 middlePoint = (positionP1 + positionP2) / 2;
        axis.transform.position = middlePoint;

        // Set position of points
        P1.transform.position = positionP1;
        P2.transform.position = positionP2;
        P3.transform.position = middlePoint;

        arrowHead.transform.position = P2.transform.position;

        // Set ROTATION 
        _vectorDirection = new Vector3(
            (positionP2.x - positionP1.x),
            (positionP2.y - positionP1.y),
            (positionP2.z - positionP1.z)
            );

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, _vectorDirection);
        transform.rotation = rotation;

        // Set SCALE 
        // Distance between the points P1 and P2
        float distance = Mathf.Sqrt(
          Mathf.Pow(positionP2.x - positionP1.x, 2)
          + Mathf.Pow(positionP2.y - positionP1.y, 2)
          + Mathf.Pow(positionP2.z - positionP1.z, 2)
          );
        // Set the length of the axis
        axis.transform.localScale = new Vector3(
            axis.transform.localScale.x,
            distance / 2,
            axis.transform.localScale.z
            );

        positionP1 = pos1;
        positionP2 = pos2;
    }

    // Show movable points of the vector
    private void showPoints(bool show)
    {
        foreach (GameObject point in _movablePoints)
        {
            point.SetActive(show);
        }
    }

    // Outline the selected point
    private void showSelectedPoint(GameObject pointToSelect)
    {

        foreach (GameObject currentPoint in _movablePoints)
        {
            if (pointToSelect != null && currentPoint.name == pointToSelect.name)
            {
                currentPoint.GetComponent<Outline>().enabled = true;
                currentPoint.GetComponent<MeshRenderer>().material = selectedPointMaterial;
            }
            else
            {
                currentPoint.GetComponent<Outline>().enabled = false;
                currentPoint.GetComponent<MeshRenderer>().material = pointMaterial;
            }
        }
    }

    // Check if the vector is still selected
    private void CheckSelection()
    {
        if (selectionManager.GetComponent<ObjectSelect>().getSelectedObject() != gameObject)
            Select(null);
    }

    // Return the direction of the vector
    public Vector3 getVectorDirection()
    {
        return _vectorDirection;
    }

    // Return the position of the first point
    public Vector3 getPositionP1()
    {
        return positionP1;
    }

    public Vector3 getVector()
    {
        return positionP2 - positionP1;
    }
}
