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
    //[SerializeField]
    //public PastryPet enemyPet;
    //[SerializeField]
    //public PastryPet ownedPet;
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

    public GameObject teamObject;
    public PastryPetTeam team;

    private GameObject ownedObject;
    public PastryPet ownedPet;
    private GameObject enemyObject;
    private PastryPet enemyPet;

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

        enemyObject = new GameObject("PastryPet");
        enemyPet = enemyObject.AddComponent<PastryPet>();
        enemyPet.SetName("Cookiedile");
        enemyPet.SetSpecies(PastryPet.Species.Cookiedile);
        enemyPet.SetType(PastryPet.Type.Pyro);
        enemyPet.SetLevel(5);

        enemyPet.AssignWeakTo();
        enemyPet.AssignBaseStats();
        //string enemySpritePath = $"C:\\Users\\aheffley\\Neumont\\Fall 2025\\Quarter\\PRO390-Capstone\\Projects\\Capstone-PastryPets\\Assets\\Sprites\\PastryPets\\{enemyPet.GetSpecies()}\\{enemyPet.GetSpecies()}{enemyPet.GetType()}_1";
        //Sprite enemySprite = Resources.Load<Sprite>(enemySpritePath);
        //if (enemySprite != null )
        //{
        //    //enemyPet.SetSprite(enemySprite);
        //}
        //else
        //{
        //    Debug.LogWarning($"Sprite not found at path: {enemySpritePath}");
        //}

        teamObject = new GameObject("PastryPetTeam");
        team = teamObject.AddComponent<PastryPetTeam>();
        team.LoadMembers();

        ownedObject = new GameObject("PastryPet");
        ownedPet = ownedObject.AddComponent<PastryPet>();

        if (team.GetMember1 != null)
        {
            ownedPet = team.GetMember1;
            //Sprite[] ownedSprites = Resources.LoadAll<Sprite>($"{ownedPet.GetSpecies()}{ownedPet.GetType()}");
            //Sprite ownedTarget = System.Array.Find(ownedSprites, s => s.name == $"{ownedPet.GetSpecies()}{ownedPet.GetType()}_0");
            //ownedPet.spriteRenderer.sprite = ownedTarget;
        }

        petSlider.maxValue = ownedPet.GetMaxHealth();
        enemySlider.maxValue = enemyPet.GetHealth();
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

        //string spritePath = $"C:\\Users\\aheffley\\Neumont\\Fall 2025\\Quarter\\PRO390-Capstone\\Projects\\Capstone-PastryPets\\Assets\\Sprites\\PastryPets\\{ownedPet.GetSpecies()}\\{ownedPet.GetSpecies()} {ownedPet.GetType()}_0";
        //Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
        //if (loadedSprite != null)
        //{
        //    //ownedPet.SetSprite(loadedSprite);
        //}
        //else
        //{
        //    Debug.LogWarning($"Sprite not found at path: {spritePath}");
        //}
    }

    public void Update()
    {
        int damageTaken = ownedPet.GetMaxHealth() - ownedPet.GetHealth();
        petSlider.value = ownedPet.GetMaxHealth() - damageTaken;
        enemySlider.value = enemyPet.GetHealth();
    }
}
