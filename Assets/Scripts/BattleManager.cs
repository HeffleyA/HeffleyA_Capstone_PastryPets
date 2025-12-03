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
    [SerializeField]
    public Sprite[] sprites;
    [SerializeField]
    public SpriteRenderer ownedRender;
    [SerializeField]
    public SpriteRenderer enemyRender;

    public PastryPetTeam team = new PastryPetTeam();

    public PastryPet ownedPet = new PastryPet();
    private PastryPet enemyPet = new PastryPet();

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

        team.SaveMembers();
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

        enemyPet.SetName("Cookiedile");
        enemyPet.SetSpecies(PastryPet.Species.Cookiedile);
        enemyPet.SetType(PastryPet.Type.Pyro);
        enemyPet.SetLevel(5);

        enemyPet.AssignWeakTo();
        enemyPet.AssignBaseStats();
        SetSprite($"{enemyPet.GetSpecies()}{enemyPet.GetType()}_1", enemyPet, enemyRender);

        team.LoadMembers();


        if (team.GetMember1 != null)
        {
            ownedPet = team.GetMember1;
            SetSprite($"{ownedPet.GetSpecies()}{ownedPet.GetType()}_0", ownedPet, ownedRender);
        }

        petSlider.maxValue = ownedPet.GetMaxHealth();
        enemySlider.maxValue = enemyPet.GetHealth();
    }

    private void SetSprite(string spriteName, PastryPet pet, SpriteRenderer renderer)
    {
        Sprite s = null;
        Debug.Log(spriteName);

        if (pet == ownedPet)
        {
           s = System.Array.Find(sprites, x => x.name == spriteName);
        }
        else if (pet == enemyPet)
        {
           s = System.Array.Find(sprites, x => x.name == spriteName);
        }
        else
        {
            Debug.Log("Pet not found!");
        }

        if (s == null)
        {
            Debug.LogError($"Sprite '{spriteName}' not found!");
        }
        else
        {
            Debug.Log($"Found sprite {s.name}");
        }

        renderer.sprite = s;
    }

    public void SwitchMember()
    {
        Debug.Log($"SwitchMember is getting run");

        if (ownedPet == team.GetMember1 && team.GetMember2 != null)
        {
            ownedPet = team.GetMember2;
        }
        else if (ownedPet == team.GetMember2 && team.GetMember3 != null)
        {
            ownedPet = team.GetMember3;
        }
        else if (ownedPet == team.GetMember2 && team.GetMember3 == null)
        {
            ownedPet = team.GetMember1;
        }
        else if (ownedPet == team.GetMember3 && team.GetMember1 != null)
        {
            ownedPet = team.GetMember1;
        }

        SetSprite($"{ownedPet.GetSpecies()}{ownedPet.GetType()}_0", ownedPet, ownedRender);
    }

    public void Update()
    {
        int damageTaken = ownedPet.GetMaxHealth() - ownedPet.GetHealth();
        petSlider.value = ownedPet.GetMaxHealth() - damageTaken;
        enemySlider.value = enemyPet.GetHealth();
    }
}
