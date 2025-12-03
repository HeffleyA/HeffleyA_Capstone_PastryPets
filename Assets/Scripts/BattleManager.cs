using System.Collections;
using System.Timers;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    System.Random random = new System.Random();

    private Inventory inventory = new Inventory();

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

        if (enemyPet.GetHealth() <= 0)
        {
            ownedPet.SetExp(ownedPet.GetExp() + (enemyPet.GetLevel() * 2));
            ownedPet.SetExpToLevel(ownedPet.GetExpToLevel() - ownedPet.GetExp());
            if (ownedPet.GetExpToLevel() >= ownedPet.GetExp())
            {
                ownedPet.OnLevelUp(ownedPet.GetExpToLevel() - ownedPet.GetExp());
            }
           
            team.SaveMembers();

            SceneManager.LoadScene("SampleScene");
        }

        if (team.GetMember1.GetKnockedOut() == true && 
            (team.GetMember2 == null || team.GetMember2.GetKnockedOut() == true)
            && (team.GetMember3 == null || team.GetMember3.GetKnockedOut() == true))
        {

            foreach (var item in inventory.items)
            {
                if (item.GetItemType() == Item.ItemType.Money)
                {
                    if (item.GetAmountOwned() >= (enemyPet.GetLevel() * 10))
                    {
                        item.SetAmountOwned(item.GetAmountOwned() - (enemyPet.GetLevel() * 10));
                    }
                }
            }

            team.GetMember1.SetHealth(team.GetMember1.GetMaxHealth());
            team.GetMember1.SetKnockedOut(false);
            if (team.GetMember2 != null)
            {
                team.GetMember2.SetHealth(team.GetMember2.GetMaxHealth());
                team.GetMember2.SetKnockedOut(false);
            }
            if (team.GetMember3 != null)
            {
                team.GetMember3.SetHealth(team.GetMember3.GetMaxHealth());
                team.GetMember3.SetKnockedOut(false);
            }

            team.SaveMembers();

            SceneManager.LoadScene("SampleScene");
        }

        if (ownedPet == team.GetMember1 && ownedPet.GetHealth() <= 0)
        {
            team.GetMember1.SetKnockedOut(true);
            SwitchMember();
        }

        if (team.GetMember2 != null && ownedPet == team.GetMember2 && ownedPet.GetHealth() <= 0)
        {
            team.GetMember2.SetKnockedOut(true);
            SwitchMember();
        }

        if (team.GetMember3 != null && ownedPet == team.GetMember3 && ownedPet.GetHealth() <= 0)
        {
            team.GetMember3.SetKnockedOut(true);
            SwitchMember();
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

        enemyPet.SetType(PastryPet.Type.Pyro);
        enemyPet.SetLevel(5);

        switch (random.Next(5))
        {
            case 0:
                enemyPet.SetSpecies(PastryPet.Species.Cookiedile);
                break;
            case 1:
                enemyPet.SetSpecies(PastryPet.Species.Puppuff);
                break;
            case 2:
                enemyPet.SetSpecies(PastryPet.Species.Cupcat);
                break;
            case 3:
                enemyPet.SetSpecies(PastryPet.Species.Bonbonny);
                break;
            case 4:
                enemyPet.SetSpecies(PastryPet.Species.Moofin);
                break;
            default:
                break;
        }
        enemyPet.SetName(enemyPet.GetSpecies().ToString());

        switch(random.Next(10))
        {
            case 0:
                enemyPet.SetType(PastryPet.Type.Basic);
                break;
            case 1:
                enemyPet.SetType(PastryPet.Type.Pyro);
                break;
            case 2:
                enemyPet.SetType(PastryPet.Type.Aqua);
                break;
            case 3:
                enemyPet.SetType(PastryPet.Type.Gaia);
                break;
            case 4:
                enemyPet.SetType(PastryPet.Type.Terra);
                break;
            case 5:
                enemyPet.SetType(PastryPet.Type.Spark);
                break;
            case 6:
                enemyPet.SetType(PastryPet.Type.Toxic);
                break;
            case 7:
                enemyPet.SetType(PastryPet.Type.Metallic);
                break;
            case 8:
                enemyPet.SetType(PastryPet.Type.Aero);
                break;
            case 9:
                enemyPet.SetType(PastryPet.Type.Arcane);
                break;
            default:
                break;
        }

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

        if (ownedPet == team.GetMember1 && team.GetMember2 != null && team.GetMember2.GetKnockedOut() == false)
        {
            ownedPet = team.GetMember2;
        }
        else if (ownedPet == team.GetMember2 && team.GetMember3 != null && team.GetMember3.GetKnockedOut() == false)
        {
            ownedPet = team.GetMember3;
        }
        else if (ownedPet == team.GetMember2 && team.GetMember3 == null && team.GetMember1.GetKnockedOut() == false)
        {
            ownedPet = team.GetMember1;
        }
        else if (ownedPet == team.GetMember3 && team.GetMember1 != null && team.GetMember1.GetKnockedOut() == false)
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
