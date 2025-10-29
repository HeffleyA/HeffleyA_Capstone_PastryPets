using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    public PastryPet enemyPet;

    [SerializeField]
    public PastryPet ownedPet;

    public void OnTakeTurn()
    {
        if (ownedPet.Speed >= enemyPet.Speed && ownedPet.isAttacking)
        {
            ownedPet.OnAttack(enemyPet);
            ownedPet.isAttacking = false;
        }

        enemyPet.OnAttack(ownedPet);

        if (ownedPet.isAttacking)
        {
            ownedPet.OnAttack(enemyPet);
            ownedPet.isAttacking = false;
        }

        if (ownedPet.isDefending)
        {
            ownedPet.OnDefend();
            ownedPet.isDefending = false;
        }

        if (ownedPet.isDodging)
        {
            ownedPet.OnDodge();
            ownedPet.isDodging = false;
        }

        ownedPet.OnTakeDamage();
        enemyPet.OnTakeDamage();

        Debug.Log($"{ownedPet.Name} took {ownedPet.damageToTake} damage and has {ownedPet.Health} health remaining!");
        Debug.Log($"{enemyPet.Name} took {enemyPet.damageToTake} damage and has {enemyPet.Health} health remaining!");
        ownedPet.damageToTake = 0;
        enemyPet.damageToTake = 0;
    }
}
