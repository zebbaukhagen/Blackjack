//////////////////////////////////////////////
//Assignment/Lab/Project: BlackJack
//Name: Zebulun Baukhagen
//Section: 2023SP.SGD.213.2172
//Instructor: Brian Sowers
//Date: 2/25/2023
/////////////////////////////////////////////

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] GameObject humanPlayArea; // two invisible areas that dynamically align the cards
    [SerializeField] GameObject computerPlayArea;
    [SerializeField] GameObject cardPrefab; // card object to spawn in
    [SerializeField] GameObject endPanel;
    [SerializeField] Button hitButton; // need these in order to set them non-interactable
    [SerializeField] Button holdButton;
    [SerializeField] TextMeshProUGUI yourTotalNumber; // players hand total
    [SerializeField] TextMeshProUGUI endMessageField;

    #endregion

    #region Card Sprites
    // a bunch of lists in order to sort our sprites
    [SerializeField] List<Sprite> aces;
    [SerializeField] List<Sprite> twos;
    [SerializeField] List<Sprite> threes;
    [SerializeField] List<Sprite> fours;
    [SerializeField] List<Sprite> fives;
    [SerializeField] List<Sprite> sixes;
    [SerializeField] List<Sprite> sevens;
    [SerializeField] List<Sprite> eights;
    [SerializeField] List<Sprite> nines;
    [SerializeField] List<Sprite> tens;

    #endregion

    private List<Card> deck = new(); // represents the deck of cards

    // each player, computerPlayer inherits from player class
    Player humanPlayer = new();
    ComputerPlayer computerPlayer = new();

    GameObject firstComputerCard; // hold the first computer card so that we can make it visible later

    GameResult gameResult;
    string endMessage;

    // Start is called before the first frame update
    void Start()
    {
        PopulateDeck();
        GameSetup(); // deal two cards and check for blackjacks
    }

    private void PopulateDeck()
    {
        // adds a Card of each value to the deck, four times
        // the third nested loop referencing k adds 4 Cards of value 10
        for (int j = 0; j < 4; j++)
        {
            for (int i = 2; i < 12; i++)
            {
                if (i == 10)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        deck.Add(new Card(i));
                    }
                }
                else
                {
                    deck.Add(new Card(i));
                }
            }
        }
    }

    public Card DrawRandomCardFromDeck()
    {
        // get random card from the deck, return and remove it
        Card choice = deck[Random.Range(0, deck.Count)];
        deck.Remove(choice);
        return choice;
    }

    public void OnClickHitButton()
    {
        // give player a card, if their hand is size 5, their turn is over
        // also check for bust
        PlayCard(humanPlayer);
        if (humanPlayer.Hand.Count == 5)
        {
            ComputerTurn();
        }
        else
        {
            CheckForBust(humanPlayer);
        }
    }

    public void OnClickHoldButton()
    {
        // end player turn
        ComputerTurn();
    }

    private void DisableButtons()
    {
        // make hit and hold buttons non-interactable
        hitButton.interactable = false;
        holdButton.interactable = false;
    }

    private void ComputerTurn()
    {
        // see if the computer wants more cards, if they do keep giving cards until they don't
        DisableButtons();
        bool computerWantsMore = computerPlayer.DoesComputerWantAnotherCard();
        while (computerWantsMore)
        {
            PlayCard(computerPlayer);
            computerWantsMore = computerPlayer.DoesComputerWantAnotherCard();
        }
        EndGame();
    }

    private void PlayFirstComputerCard()
    {
        // play the first computer card specially so that it doesn't get a sprite assigned
        Card cardToPlay = DrawRandomCardFromDeck();
        GameObject newCard;
        computerPlayer.Hit(cardToPlay);
        newCard = Instantiate(cardPrefab, computerPlayArea.transform);
        firstComputerCard = newCard;
    }

    private void PlayCard(Player player)
    {
        // get random card
        // if player calling this method is human, give humanPlayer a hit and instantiate to their play area
        // do the same, but for computer
        // assign a sprite to the card and update the UI
        Card cardToPlay = DrawRandomCardFromDeck();
        GameObject newCard;
        if (player == humanPlayer)
        {
            humanPlayer.Hit(cardToPlay);
            newCard = Instantiate(cardPrefab, humanPlayArea.transform);
        }
        else
        {
            computerPlayer.Hit(cardToPlay);
            newCard = Instantiate(cardPrefab, computerPlayArea.transform);
        }
        AssignSprite(newCard, cardToPlay.Value);
        UpdateUI();
    }

    private void AssignSprite(GameObject cardToAssign, int cardValue)
    {
        // figure out which list of sprites to draw from and then assign to the card that is passed into this method
        List<Sprite> listToAssignFrom = new();
        if (cardValue == 10)
        {
            listToAssignFrom = tens;
        }
        else if (cardValue == 9)
        {
            listToAssignFrom = nines;
        }
        else if (cardValue == 8)
        {
            listToAssignFrom = eights;
        }
        else if (cardValue == 7)
        {
            listToAssignFrom = sevens;
        }
        else if (cardValue == 6)
        {
            listToAssignFrom = sixes;
        }
        else if (cardValue == 5)
        {
            listToAssignFrom = fives;
        }
        else if (cardValue == 4)
        {
            listToAssignFrom = fours;
        }
        else if (cardValue == 3)
        {
            listToAssignFrom = threes;
        }
        else if (cardValue == 2)
        {
            listToAssignFrom = twos;
        }
        else if (cardValue == 11 || cardValue == 1)
        {
            listToAssignFrom = aces;
        }
        int randomIndex = Random.Range(0, listToAssignFrom.Count);
        cardToAssign.GetComponent<UnityEngine.UI.Image>().sprite = listToAssignFrom[randomIndex];
    }

    private void GameSetup()
    {
        // deal each player two cards, one face down for computer
        // check for blackjacks
        PlayFirstComputerCard();
        PlayCard(computerPlayer);
        PlayCard(humanPlayer);
        PlayCard(humanPlayer);
        if (humanPlayer.CheckForBlackJack() || computerPlayer.CheckForBlackJack())
        {
            EndGame();
        }
    }

    private void CheckForBust(Player player)
    {
        // check if the player has busted, end game if so
        if (player.IsBusted == true)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // execute end game, determine the winner, assign sprite to the first computer card
        // assign end game message, and then activate the end panel
        DisableButtons();
        DetermineWinner();
        AssignSprite(firstComputerCard, computerPlayer.Hand[0].Value);
        if (gameResult == GameResult.HumanWon)
        {
            endMessage = "You won! Play Again?";
        }
        else
        {
            endMessage = "The house won. Play again?";
        }
        endMessageField.text = endMessage;
        endPanel.SetActive(true);
    }

    private void DetermineWinner()
    {
        // determine winner
        if (computerPlayer.IsBusted)
        {
            gameResult = GameResult.HumanWon;
        }
        else if (humanPlayer.IsBusted)
        {
            gameResult = GameResult.ComputerWon;
        }
        else if (computerPlayer.HandValue >= humanPlayer.HandValue)
        {
            gameResult = GameResult.ComputerWon;
        }
        else
        {
            gameResult = GameResult.HumanWon;
        }
    }

    private void UpdateUI()
    {
        yourTotalNumber.text = humanPlayer.HandValue.ToString();
    }

}

public enum GameResult
{
    HumanWon,
    ComputerWon,
}