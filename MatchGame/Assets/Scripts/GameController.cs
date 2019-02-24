using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform gridTopCorner;
    public float xOffset;
    public float yOffset;
    public int rowLength;

    private Vector3 offsetPoint;
    private GameObject offset;
    private GameObject[] cardTypes;
    private GameObject[] cards;
    private bool gameWon;
    private GameObject card1;
    private GameObject card2;

    void Start()
    {
        Debug.Log("started game.");
        cardTypes = Resources.LoadAll<GameObject>("Cards") as GameObject[];
        Debug.Log("loaded cards.");
        Debug.Log("number loaded: " + cardTypes.Length);
        int counter = 0;
        offset = new GameObject();
        offset.transform.position = gridTopCorner.position;
        offsetPoint = new Vector3(offset.transform.position.x, offset.transform.position.y, offset.transform.position.z);
        cards = new GameObject[cardTypes.Length*2];

        if(cardTypes != null)
        {
            Debug.Log("loaded card types, setting up cards.");
            for (int i = 0; i < cardTypes.Length; i++)
            {
                cards[counter++] = Instantiate(cardTypes[i], gridTopCorner.position, Quaternion.identity);
                cards[counter++] = Instantiate(cardTypes[i], gridTopCorner.position, Quaternion.identity);
            }
        }
        shuffleCards();
        placeCards();

        
        card1 = null;
        card2 = null;
        gameWon = false;
    }

    void Update()
    {
        if (gameWon == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 50))
            {
                GameObject hitCard = hit.transform.gameObject;
                if (!hitCard.GetComponent<CardClass>().getFlipped())
                {
                    hitCard.GetComponent<CardClass>().flipCard();
                    setCard(hitCard);
                    Debug.Log("Flipped Card: " + hitCard.tag);
                }
            }
        }

        if(card1 != null && card2 != null)
        {
            if (card1.tag == card2.tag)
            {
                matchRemove();
                card1 = null;
                card2 = null;
            }
            else
            {
                resetCards();
                card1 = null;
                card2 = null;
            }
        }

        checkWin();
    }

    public void setCard(GameObject card)
    {
        if (card1 == null)
        {
            card1 = card;
        }
        else if (card2 == null)
        {
            card2 = card;
        }
        else
        {
            //Debug.Log("This shouldn't have happened.");
        }
    }

    private void matchRemove()
    {
        if (card1 != null & card2 != null)
        {
            card1.SetActive(false);
            card2.SetActive(false);
        }
    }

    private void resetCards()
    {
        if(card1 != null)
        {
            card1.GetComponent<CardClass>().flipCard();
        }
        if(card2 != null)
        {
            card2.GetComponent<CardClass>().flipCard();
        }
    }
    
    private void setOffset()
    {
        if (offset.transform.position.x >= gridTopCorner.position.x + (xOffset * (rowLength-1)))
        {
            //Debug.Log("reached end of row, resetting x");
            offsetPoint.x = gridTopCorner.position.x;
            offsetPoint.y = offsetPoint.y - yOffset;
            offset.transform.position = offsetPoint;
        }
        else
        {
            offsetPoint.x += xOffset;
            offset.transform.position = offsetPoint;
        }
    }

    private void shuffleCards()
    {
        GameObject temp;
        for (int i = 0; i < cardTypes.Length*2; i++)
        {
            int random = Random.Range(0, cards.Length);
            temp = cards[random];
            cards[random] = cards[i];
            cards[i] = temp;  
        }
    }

    private void placeCards()
    {
        for (int i = 0; i < cardTypes.Length*2; i++)
        {
            cards[i].transform.position = offset.transform.position;
            setOffset();
        }
    }

    private void checkWin()
    {
        for (int i = 0; i < cardTypes.Length*2; i++)
        {
            if(cards[i].activeInHierarchy == true)
            {
                return;
            }
        }
        gameWon = true;
    }
}
