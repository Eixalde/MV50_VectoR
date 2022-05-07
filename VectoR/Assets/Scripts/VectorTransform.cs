using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VectorTransform : MonoBehaviour
{
    // Position of the fist point
    public Vector3 positionP1;
    // Position of the second point
    public Vector3 positionP2;

    // The two ends of the vector
    public GameObject P1;
    public GameObject P2;

    // Model of the axis of the vector 
    public GameObject axis;

    // Information to use to position the vector
    // -> true : uses the position of the object in the scene
    // -> false : uses the positions of the points (P1 & P2)
    public bool useWorldPosition;

    // Vector of the game object's position
    private Vector3 vectorDirection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!useWorldPosition)
        {
            transformVectorFromPoints();
        }
    }

    // Place, rotates and changes the length of the vector
    // using the points P1 and P2 coordinates
    private void transformVectorFromPoints()
    {
        // Set position of points
        P1.transform.position = positionP1;
        P2.transform.position = positionP2;

        // Set ROTATION 
        vectorDirection = new Vector3(
            (positionP2.x - positionP1.x),
            (positionP2.y - positionP1.y),
            (positionP2.z - positionP1.z)
            );

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vectorDirection);   
        transform.rotation = rotation;

        // Set POSITION 
        // The coordinate system is placed at the center
        // of the Game object
        Vector3 middlePoint = new Vector3(
            (positionP1.x + positionP2.x) / 2,
            (positionP1.y + positionP2.y) / 2,
            (positionP1.z + positionP2.z) / 2
            );

        axis.transform.position = middlePoint;

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
            distance/2,
            axis.transform.localScale.z
            );
            
    }
}
