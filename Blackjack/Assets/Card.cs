//////////////////////////////////////////////
//Assignment/Lab/Project: BlackJack
//Name: Zebulun Baukhagen
//Section: 2023SP.SGD.213.2172
//Instructor: Brian Sowers
//Date: 2/25/2023
/////////////////////////////////////////////
public class Card
{
    public int scoreValue;

    public Card(int startingValue)
    {
        scoreValue = startingValue;
    }

    public int Value
    {
        get => scoreValue;
        set => scoreValue = value;
    }

    public void ConvertToLowAce()
    {
        // change card value to 1 if called
        scoreValue = 1;
    }

}
