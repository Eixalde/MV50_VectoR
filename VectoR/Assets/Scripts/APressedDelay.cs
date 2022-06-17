using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class APressedDelay : MonoBehaviour
{
    private bool AhasBeenPressed = false;
    private bool AhasBeenReleased = false;
    private bool AReleasable = false;
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
        }else if (Abutton.IsPressed())
        {
            AhasBeenReleased = false;
        }


    }

    IEnumerator ADelay()
    {
        AReleasable = true;
        yield return new WaitForSeconds(0.5f);
        AhasBeenReleased = true;
        AReleasable = false;
    }

    

    public bool isApressed()
    {
        if(!AhasBeenPressed)
        {
            if(Abutton.IsPressed())
                AhasBeenPressed = true;
            return Abutton.IsPressed();
        }
        return false;
    }

    public bool isAreleased()
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

    public bool canAbePressed()
    {
        return !AhasBeenPressed;
    }

    public bool canAbeReleased()
    {
        return AReleasable;
    }
}
