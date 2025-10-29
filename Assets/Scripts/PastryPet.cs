
using System;
using UnityEngine;

public class PastryPet : MonoBehaviour
{
    System.Random random = new System.Random();

    [SerializeField]
    public Sprite sprite;

    public enum Species
    {
        Cookiedile,
        Bonbonny,
        Puppuff,
        Cupcat,
        Moofin,
        Default
    }

    public enum Type
    {
        Basic,
        Pyro,
        Aqua,
        Gaia,
        Terra,
        Spark,
        Toxic,
        Metallic,
        Aero,
        Arcane,
        Default
    }

    private string name;
    public Species species = Species.Default;
    public Type type = Type.Basic;
    public Type weakTo = Type.Basic;
    private float health;
    private float attack;
    private float defense;
    private float speed;
    private float exp;
    private float expToLvl;
    public float damageToTake = 0;
    private int level;

    public bool isAttacking;
    public bool isDefending;
    public bool isDodging;
    public string Name;
    public float Health;
    public float Attack;
    public float Defense;
    public float Speed;
    public int Level;

    private PastryPet()
    {
        name = Name;
        health = Health;
        attack = Attack;
        defense = Defense;
        speed = Speed;
        level = Level;
    }

    void OnLevelUp()
    {
        level++;

        health = (float)(health + ((health * 0.1)) / 2);
        attack = (float)(attack + ((attack * 0.1)) / 2);
        defense = (float)(defense + ((defense * 0.1)) / 2);
        speed = (float)(speed + ((speed * 0.1)) / 2);

        expToLvl = level * random.Next(3, 5);
    }

    public void OnGainExp(PastryPet opp)
    {
        exp += (float)(opp.Level * 2);

        float remainingExp = expToLvl - exp;

        if (remainingExp <= 0)
        {
            OnLevelUp();
            exp = remainingExp * -1;
        }
    }

    public void CalculateDamage(PastryPet opp)
    {
        damageToTake += opp.Attack * 0.1f;

        if (opp.type == weakTo)
        {
            damageToTake *= 1.5f;
            Debug.Log($"{Name} attacked with a super effective hit!");
        }

        if (random.Next(16) == 5)
        {
            damageToTake *= 1.5f;
            Debug.Log($"{Name} attacked with a critical hit!");
        }
    }

    public void OnAttack(PastryPet opp)
    {
        opp.CalculateDamage(this);
    }

    public void OnDefend()
    {
        damageToTake /= 2.0f;
    }

    public void OnDodge()
    {
        if (random.Next(11) == 5)
        {
            damageToTake = 0.0f;
            Debug.Log($"{Name} dodged the attack!");
        }
    }

    public void OnTakeDamage()
    {
        Health -= damageToTake;
    }

    public void OnKnockedOut()
    {

    }

    public void AssignBaseStats()
    {
        switch (type)
        {
            case Type.Basic:
                weakTo = Type.Toxic;
                return;
            case Type.Pyro:
                weakTo = Type.Aqua;
                return;
            case Type.Aqua:
                weakTo = Type.Gaia;
                return;
            case Type.Gaia:
                weakTo = Type.Pyro;
                return;
            case Type.Terra:
                weakTo = Type.Aero;
                return;
            case Type.Spark:
                weakTo = Type.Terra;
                return;
            case Type.Toxic:
                weakTo = Type.Metallic;
                return;
            case Type.Metallic:
                weakTo = Type.Arcane;
                return;
            case Type.Aero:
                weakTo = Type.Spark;
                return;
            case Type.Arcane:
                weakTo = Type.Basic;
                return;
        }

        switch (species)
        {
            case Species.Cookiedile:
                health = random.Next(60, 80);
                attack = random.Next(65, 75);
                defense = random.Next(50, 65);
                speed = random.Next(40, 45);
                return;
            case Species.Bonbonny:
                health = random.Next(45, 65);
                attack = random.Next(50, 60);
                defense = random.Next(45, 60);
                speed = random.Next(60, 65);
                return;
            case Species.Puppuff:
                health = random.Next(55, 75);
                attack = random.Next(60, 70);
                defense = random.Next(55, 70);
                speed = random.Next(55, 60);
                return;
            case Species.Cupcat:
                health = random.Next(50, 70);
                attack = random.Next(55, 65);
                defense = random.Next(60, 75);
                speed = random.Next(50, 55);
                return;
            case Species.Moofin:
                health = random.Next(80, 100);
                attack = random.Next(70, 80);
                defense = random.Next(75, 90);
                speed = random.Next(45, 50);
                return;
        }
    }
}