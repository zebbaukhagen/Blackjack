//////////////////////////////////////////////
//Assignment/Lab/Project: BlackJack
//Name: Zebulun Baukhagen
//Section: 2023SP.SGD.213.2172
//Instructor: Brian Sowers
//Date: 2/25/2023
/////////////////////////////////////////////

using System.Collections.Generic;
public class Player
{
    List<Card> hand;
    List<Card> highAces;
    int handValue;
    bool isBusted = false;

    public Player()
    {
        // construct with new lists
        hand = new();
        highAces = new();
    }

    #region Properties
    public List<Card> Hand
    {
        get => hand;
    }

    public int HandValue
    {
        get => handValue;
        set => handValue = value;
    }

    public bool IsBusted
    {
        get => isBusted;
    }

    #endregion

    public void Hit(Card card)
    {
        // take the given card and add it to the hand
        // if the card is an ace, keep track of it in the highAces list
        // calculate the handValue
        hand.Add(card);
        if (card.Value == 11)
        {
            highAces.Add(card);
        }
        handValue = CalculateHandValue();
    }

    public int CalculateHandValue()
    {
        // go through all cards in the hand, return the sum
        // if sum exceeds 21, check if there are highAces and then convert them to lowAces
        // if sum exceeds 21 and there are no highAces to covert, isBusted is true
        int total = 0;
        foreach (Card card in hand)
        {
            total += card.Value;
        }
        if (total > 21 && highAces.Count > 0)
        {
            highAces[0].ConvertToLowAce();
            highAces.RemoveAt(0);
            total -= 10;
        }
        else if (total > 21 && highAces.Count == 0)
        {
            isBusted = true;
        }

        return total;
    }

    public bool CheckForBlackJack()
    {
        // check for BlackJack
        if (handValue == 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
