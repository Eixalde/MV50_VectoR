using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* 
 * Class used to know when A is press and release only one time
 * ex : Abutton.IsPressed() returns true
 *      isApress is called and returns true
 *      isApress is called a second time, it returns false
 */

public class APressedDelay : MonoBehaviour
{
    // Indicate if A has been pressed
    private bool AhasBeenPressed = false;
    // Indicate if A has been realeased
    private bool AhasBeenReleased = false;
    // Indicate if A can be released
    // True when A is no longer pressed from less than the releaseTime
    private bool AReleasable = false;
    // Time during wich A is releasable 
    private float releaseTime = 0.5f;

    // Variables used to know if A is currently pressed
    public InputActionAsset inputActions;
    private InputAction Abutton;


    // Start is called before the first frame update
    void Start()
    {
        Abutton = inputActions.FindActionMap("XRI RightHand").FindAction("A_Button");
    }

    // Update is called once per frame
    void Update()
    {

        if (!Abutton.IsPressed())
        {
            AhasBeenPressed = false;
            if (AhasBeenReleased == false)
            {
                StartCoroutine(ADelay());
            }
        }
        else
        {
            AhasBeenReleased = false;
        }


    }
    
    // Allow to turn AReleasable to false after a certain releaseTime
    IEnumerator ADelay()
    {
        AReleasable = true;
        yield return new WaitForSeconds(releaseTime);
        AhasBeenReleased = true;
        AReleasable = false;
    }



    // Check if A is press
    public bool isApress()
    {
        if(!AhasBeenPressed)
        {
            if(Abutton.IsPressed())
                AhasBeenPressed = true;
            return Abutton.IsPressed();
        }
        return false;
    }

    // Check if A is release
    public bool isArelease()
    {
        if (!AhasBeenReleased)
        {
            if (AReleasable)
            {
                AhasBeenReleased = true;
                AReleasable = false;
                return !Abutton.IsPressed();
            }    
        }
        return false;
    }

    // Debug method to check if A can be press
    public bool canAbePress()
    {
        return !AhasBeenPressed;
    }
    // Debug method to check if A can be release
    public bool canAbeRelease()
    {
        return AReleasable;
    }
}
