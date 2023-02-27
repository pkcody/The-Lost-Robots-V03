using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TutorialScriptPickUp : MonoBehaviour
{
    private PlayerControls playerInput;

    //tutorial stuff
    public GameObject TutorialScript;
    public bool ts_pickedUp;
    public bool tutorialON = true;

    private CharHoldItem _charHoldItem;
    public Transform itemToHold;

    private void OnEnable()
    {
        playerInput.Enable();
        //BeginGame();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Awake()
    {
        playerInput = new PlayerControls();
        _charHoldItem = GetComponent<CharHoldItem>();

    }

    public void OnPickUpTutorialScript(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (ts_pickedUp)
            {
                _charHoldItem.HoldPlease(itemToHold);
            }
            
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<Collider>().tag == "TutorialScript")
        {
            ts_pickedUp = false;
            //TutorialScript.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        //print(collision.name);
        if (collision.GetComponent<Collider>().tag == "TutorialScript")
        {
            TutorialScript = collision.gameObject;
            ts_pickedUp = true;
            itemToHold = TutorialScript.transform;
            
        }

    }

}
