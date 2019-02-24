using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass : MonoBehaviour
{
    //GameObject GameController;
    private bool flipped;
    private bool isMatched;
    // Start is called before the first frame update
    void Start()
    {
        //GameController = GameObject.FindGameObjectWithTag("GameController");
        flipped = false;
        isMatched = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void flipCard()
    {
        if(flipped == true)
        {
            flipped = false;
        }
        else if(flipped == false)
        {
            flipped = true;
        }
        //dostuff
    }

    public bool getIsMatched()
    {
        return isMatched;
    }

    public bool getFlipped()
    {
        return flipped;
    }
}
