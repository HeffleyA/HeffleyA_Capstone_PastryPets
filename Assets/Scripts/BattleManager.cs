using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private int turn = 0;
    private PastryPet first = null;
    private PastryPet second = null;

    void TakeTurn(PastryPet pet, PastryPet opp)
    {
        if (pet.Speed >= opp.Speed)
        {
            first = pet;
            second = opp;
        }
        else
        {
            first = opp;
            second = pet;
        }

        do
        {
            if (turn % 2 == 0)
            {
                second.TakeDamage(first);
            }
            else
            {
                first.TakeDamage(second);
            }
        }
        while (first.Health > 0 || second.Health > 0);

    }

    void OnBattleWin(PastryPet pet)
    {
        
    }
}
