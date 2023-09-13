//////////////////////////////////////////////
//Assignment/Lab/Project: BlackJack
//Name: Zebulun Baukhagen
//Section: 2023SP.SGD.213.2172
//Instructor: Brian Sowers
//Date: 2/25/2023
/////////////////////////////////////////////
public class ComputerPlayer : Player
{
    public bool DoesComputerWantAnotherCard()
    {
        // determine if the computerPlayer wants another card
        bool result;
        if (HandValue < 17)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }
}
