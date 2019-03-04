using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass : MonoBehaviour
{
    // Card state Variables
    private bool revealed = false;
    private bool isMatched = false;
    private bool turning = false;
    private bool clicked = false;

    private Animator anim = null;

    public void SetHidden() { revealed = false; turning = false; }
    public void SetRevealed() {
        Debug.Log("Set Revealed");
        // Setting State Variables
        revealed = true;
        turning = false;

        // Telling the Manager to check for a win
        GameController.controller.CheckForCardMatch();
    }

    public bool IsClicked() { return clicked; }
    public bool GetIsMatched() { return isMatched; }
    public bool GetRevealed() { return (revealed && !turning); }

    public void Awake()
    {
        // Getting the Anim Ref
        anim = GetComponent<Animator>();
    }

    public void FlipCard() {
        if (!turning) {
            if (!revealed) {
                anim.SetTrigger("Reveal");
                turning = clicked = true;
            } else {
                anim.SetTrigger("Hide");
                turning = true;
                clicked = false;
            }
        }
    }

    
}
