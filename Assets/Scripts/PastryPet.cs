using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PastryPet : MonoBehaviour
{
    System.Random random = new System.Random();

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
    private Species species = Species.Default;
    private Type type = Type.Basic;
    private Type weakTo = Type.Basic;
    private int health;
    private int attack;
    private int defense;
    private int speed;
    private int level;

    private float exp;
    private float expToLvl;
    public int damageToTake = 0;

    public bool isAttacking = false;
    public bool isDefending = false;
    public bool isDodging = false;
    public bool hitCritical;
    public bool hitSuperEffective;
    public bool hasDodged = false;
    public bool hasDefended = false;

    public string GetName() { return name; }
    public Species GetSpecies() { return species; }
    public Type GetType() { return type; }
    public Type GetWeakTo() { return weakTo; }
    public int GetHealth() { return health; }
    public int GetAttack() { return attack; }
    public int GetDefense() { return defense; }
    public int GetSpeed() { return speed; }
    public int GetLevel() { return level; }

    public void SetName(string value) { name = value; }
    public void SetSpecies (Species value) { species = value; }
    public void SetType(Type value) { type = value; }
    public void SetWeakTo(Type value) { weakTo = value; }
    public void SetHealth(int value) { health = value; }
    public void SetAttack(int value) { attack = value; }
    public void SetDefense(int value) { defense = value; }
    public void SetSpeed(int value) { speed = value; }
    public void SetLevel(int value) { level = value; }

    void OnLevelUp()
    {
        level++;

        health = (int)(health + ((health * 0.1)) / 2);
        attack = (int)(attack + ((attack * 0.1)) / 2);
        defense = (int)(defense + ((defense * 0.1)) / 2);
        speed = (int)(speed + ((speed * 0.1)) / 2);

        expToLvl = level * random.Next(3, 5);
    }

    public void OnGainExp(PastryPet opp)
    {
        exp += (float)(opp.GetLevel() * 2);

        float remainingExp = expToLvl - exp;

        if (remainingExp <= 0)
        {
            OnLevelUp();
            exp = remainingExp * -1;
        }
    }

    public void OnGetAttacked(PastryPet opp)
    {
        damageToTake += (int)(opp.GetAttack() * 0.1);

        if (opp.type == this.weakTo)
        {
            damageToTake *= (int)1.5;
            opp.hitSuperEffective = true;
        }

        if (random.Next(16) == 5)
        {
            damageToTake *= (int)1.5;
            opp.hitCritical = true;
        }
        
        if (this.isDefending)
        {
            damageToTake = damageToTake / 2;
            hasDefended = true;
            isDefending = false;
        }

        if (this.isDodging)
        {
            if (random.Next(11) == 5)
            {
                damageToTake = 0;
                hasDodged = true;
            }
            else
            {
                hasDodged = false;
            }

            isDodging = false;
        }

        this.SetHealth(GetHealth() - damageToTake);
    }

    public void OnKnockedOut()
    {

    }

    public void AssignWeakTo()
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
    }

    public void AssignBaseStats()
    {
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