using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class APressedDelay : MonoBehaviour
{
    private bool APressed = false;
    public InputActionAsset inputActions;
    private InputAction Abutton;

    // Start is called before the first frame update
    void Start()
    {
        Abutton = inputActions.FindActionMap("XRI RightHand").FindAction("A_Button");
        APressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressA()
    {
        StartCoroutine(ADelay());
    }

    IEnumerator ADelay()
    {
        APressed = true;
        yield return new WaitForSeconds(0.1f);
        APressed = false;
    }

    public bool isApressed()
    {
        if(!APressed)
        {
            PressA();
            return Abutton.IsPressed();
        }
        return false;
    }

    public bool debugIsAPressed()
    {
        return APressed;
    }
}
