using System.Collections;
using System.Timers;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    public PastryPet enemyPet;
    [SerializeField]
    public PastryPet ownedPet;
    [SerializeField]
    public Slider petSlider;
    [SerializeField]
    public Slider enemySlider;
    [SerializeField]
    public TextMeshPro battleText;
    [SerializeField]
    public AudioSource battleImpact;
    [SerializeField]
    public AudioSource music;
    [SerializeField]
    public AudioResource normalBI;
    [SerializeField]
    public AudioResource superBI;

    private GameObject teamObject;
    private PastryPetTeam team;

    public IEnumerator RunTurn()
    {
        if (ownedPet.isAttacking)
        {
            if (ownedPet.GetSpeed() >= enemyPet.GetSpeed())
            {
                enemyPet.OnGetAttacked(ownedPet);
                if (CheckSuperEffective(ownedPet)) yield return new WaitForSeconds(2f);
                if (CheckCriticalHit(ownedPet)) yield return new WaitForSeconds(2f);
                battleText.text = $"{enemyPet.GetName()} took {enemyPet.damageToTake} damage!";

                yield return new WaitForSeconds(2f);

                ownedPet.OnGetAttacked(enemyPet);
                if (CheckSuperEffective(enemyPet)) yield return new WaitForSeconds(2f);
                if (CheckCriticalHit(enemyPet)) yield return new WaitForSeconds(2f);
                battleText.text = $"{ownedPet.GetName()} took {ownedPet.damageToTake} damage!";
            }
            else
            {
                ownedPet.OnGetAttacked(enemyPet);
                if (CheckSuperEffective(enemyPet)) yield return new WaitForSeconds(2f);
                if (CheckCriticalHit(enemyPet)) yield return new WaitForSeconds(2f);
                battleText.text = $"{ownedPet.GetName()} took {ownedPet.damageToTake} damage!";

                yield return new WaitForSeconds(2f);

                enemyPet.OnGetAttacked(ownedPet);
                if (CheckSuperEffective(ownedPet)) yield return new WaitForSeconds(2f);
                if (CheckCriticalHit(ownedPet)) yield return new WaitForSeconds(2f);
                battleText.text = $"{enemyPet.GetName()} took {enemyPet.damageToTake} damage!";
            }

            ownedPet.isAttacking = false;
        }
        else
        {
            ownedPet.OnGetAttacked(enemyPet);

            if (ownedPet.hasDodged)
            {
                battleText.text = $"{ownedPet.GetName()} has successfully dodged the attack!";
                ownedPet.hasDodged = false;
            }
            else if (!ownedPet.hasDodged && !ownedPet.hasDefended)
            {
                if (CheckSuperEffective(enemyPet)) yield return new WaitForSeconds(2f);
                if (CheckCriticalHit(enemyPet)) yield return new WaitForSeconds(2f);
                battleText.text = $"{ownedPet.GetName()} failed to dodge the attack and took {ownedPet.damageToTake} damage!";
            }

            if (ownedPet.hasDefended)
            {
                if (CheckSuperEffective(enemyPet)) yield return new WaitForSeconds(2f);
                if (CheckCriticalHit(enemyPet)) yield return new WaitForSeconds(2f);
                battleText.text = $"{ownedPet.GetName()} took {ownedPet.damageToTake} damage!";
                ownedPet.hasDefended = false;
            }

        }

        ownedPet.damageToTake = 0;
        enemyPet.damageToTake = 0;
    }

    private bool CheckSuperEffective(PastryPet pet)
    {
        Debug.Log($"{pet.GetName()}.hitSuperEffective is {pet.hitSuperEffective}");

        if (pet.hitSuperEffective)
        {
            battleText.text = $"{pet.GetName()} landed a super effective hit!";
            pet.hitSuperEffective = false;
            battleImpact.resource = superBI;
            battleImpact.Play();
            return true;
        }

        battleImpact.resource = normalBI;
        battleImpact.Play();
        return false;
    }

    private bool CheckCriticalHit(PastryPet pet)
    {
        if (pet.hitCritical)
        {
            battleText.text = $"{pet.GetName()} landed a critical hit!";
            pet.hitCritical = false;
            return true;
        }

        return false;
    }

    private void Awake()
    {
        music.Play();

        teamObject = new GameObject("PastryPetTeam");
        team = teamObject.AddComponent<PastryPetTeam>();
        team.LoadMembers();

        if (team.GetMember1 != null)
        {
            ownedPet = team.GetMember1;
        }

        enemyPet.SetName("Cookiedile");
        enemyPet.SetSpecies(PastryPet.Species.Cookiedile);
        enemyPet.SetType(PastryPet.Type.Pyro);
        enemyPet.SetLevel(5);

        enemyPet.AssignWeakTo();
        enemyPet.AssignBaseStats();

        petSlider.maxValue = ownedPet.GetHealth();
        enemySlider.maxValue = enemyPet.GetHealth();
    }

    public void Start()
    {

    }

    public void Update()
    {
        petSlider.value = ownedPet.GetHealth();
        enemySlider.value = enemyPet.GetHealth();
    }
}
