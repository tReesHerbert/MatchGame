using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController controller = null;

    [Header("Grid Layout Variables")]
    public Transform gridTopCorner = null;
    public float xOffset = 0f;
    public float yOffset = 0f;
    public int rowLength = 1;

    private GameObject offset = null;
    private GameObject cardParent = null;
    private Vector3 offsetPoint = Vector3.zero;

    [Header("Card Data Variables")]
    [SerializeField] private GameObject[] cardTypes = null;
    [SerializeField] private GameObject[] cards = null;
    private GameObject card1 = null;
    private GameObject card2 = null;

    [Header("GUI References")]
    [SerializeField] private GameObject endGameMenu = null;
    [SerializeField] private TextMeshProUGUI tryCounter = null;
    [SerializeField] private TextMeshProUGUI endFlipperCounter = null;

    [Header("Game State Variables")]
    private int matchCounter = 0;
    private bool gameWon = false;

    void Awake()
    {
        endGameMenu.SetActive(false);

        // Setting up the Singleton Pattern
        if (controller == null)
            controller = this;
        else
            GameObject.Destroy(gameObject);

        // Loading In all cards
        cardTypes = Resources.LoadAll<GameObject>("Cards") as GameObject[];
        Debug.Log("Finished loading " + cardTypes.Length.ToString() + " cards.");

        offset = new GameObject { name = "Offset" };
        offset.transform.position = gridTopCorner.position;
        offsetPoint = new Vector3(offset.transform.position.x, offset.transform.position.y, offset.transform.position.z);
        
        if(cardTypes != null) {
            cardParent = new GameObject("CardParent");
            cardParent.transform.position = Vector3.zero;

            int counter = 0;
            cards = new GameObject[cardTypes.Length * 2];

            for (int i = 0; i < cardTypes.Length; i++) {
                cards[counter++] = Instantiate(cardTypes[i], gridTopCorner.position, Quaternion.identity, cardParent.transform);
                cards[counter++] = Instantiate(cardTypes[i], gridTopCorner.position, Quaternion.identity, cardParent.transform);
            }
        }

        ShuffleCards();
        PlaceCards();
    }

    void Update()
    {
        if (card1 == null || card2 == null) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 50)) {
                    CardClass hitCard = hit.transform.gameObject.GetComponent<CardClass>();
                    if (!hitCard.GetRevealed() && !hitCard.IsClicked()) {
                        hitCard.GetComponent<CardClass>().FlipCard();
                        SetCard(hitCard.gameObject);
                    }
                }
            }
        }
    }

    public void CheckForCardMatch()
    {
        Debug.Log("Checking Cards");

        if (card1 != null && card1.GetComponent<CardClass>().GetRevealed() && card2 != null && card2.GetComponent<CardClass>().GetRevealed()) {
            if (card1.tag == card2.tag) {
                MatchRemove();
                tryCounter.text = (++matchCounter).ToString();
                card1 = null;
                card2 = null;

                CheckWin();
            } else {
                ResetCards();
                tryCounter.text = (++matchCounter).ToString();
                card1 = null;
                card2 = null;
            }
        }
    }

    public void SetCard(GameObject card)
    {
        if (card1 == null) {
            card1 = card;
        } else if (card2 == null) {
            card2 = card;
        }
    }

    private void MatchRemove()
    {
        if (card1 != null & card2 != null) {
            card1.SetActive(false);
            card2.SetActive(false);
        }
    }

    private void ResetCards()
    {
        if (card1 != null) {
            card1.GetComponent<CardClass>().FlipCard();
        }

        if (card2 != null) {
            card2.GetComponent<CardClass>().FlipCard();
        }
    }
    
    private void SetOffset()
    {
        if (offset.transform.position.x >= gridTopCorner.position.x + (xOffset * (rowLength-1))) {
            offsetPoint.x = gridTopCorner.position.x;
            offsetPoint.y = offsetPoint.y - yOffset;
            offset.transform.position = offsetPoint;
        } else {
            offsetPoint.x += xOffset;
            offset.transform.position = offsetPoint;
        }
    }

    private void ShuffleCards()
    {
        GameObject temp = null;
        for(int i = 0; i < cardTypes.Length*2; i++) {
            int random = Random.Range(0, cards.Length);
            temp = cards[random];
            cards[random] = cards[i];
            cards[i] = temp;  
        }
    }

    private void PlaceCards()
    {
        for(int i = 0; i < cardTypes.Length*2; i++) {
            cards[i].transform.position = offset.transform.position;
            SetOffset();
        }
    }

    private void CheckWin()
    {
        for(int i = 0; i < cardTypes.Length*2; i++) {
            if (cards[i].activeInHierarchy == true)
                return;
        }

        gameWon = true;
        endGameMenu.SetActive(true);
        endFlipperCounter.text = "Number of Tries: " + tryCounter.text;
    }

    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void ExitGame() { Application.Quit(); }
}
