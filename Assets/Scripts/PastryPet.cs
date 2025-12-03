using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PastryPet
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
    private int maxHealth;
    private int health;
    private int attack;
    private int defense;
    private int speed;
    private int level;

    private int exp;
    private int expToLvl;
    private bool knockedOut;
    public int damageToTake = 0;

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

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
    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }
    public int GetAttack() { return attack; }
    public int GetDefense() { return defense; }
    public int GetSpeed() { return speed; }
    public int GetLevel() { return level; }
    public int GetExp() { return exp; }
    public int GetExpToLevel() { return expToLvl; }
    public bool GetKnockedOut() { return knockedOut; }
    

    public void SetName(string value) { name = value; }
    public void SetSpecies (Species value) { species = value; }
    public void SetType(Type value) { type = value; }
    public void SetWeakTo(Type value) { weakTo = value; }
    public void SetMaxHealth(int value) { maxHealth = value; }
    public void SetHealth(int value) { health = value; }
    public void SetAttack(int value) { attack = value; }
    public void SetDefense(int value) { defense = value; }
    public void SetSpeed(int value) { speed = value; }
    public void SetLevel(int value) { level = value; }
    public void SetExp(int value) { exp = value; }
    public void SetExpToLevel(int value) { expToLvl = value; }
    public void SetKnockedOut(bool value) { knockedOut = value; }

    public void OnLevelUp(int remainingExp)
    {
        level++;

        health = (int)(health + ((health * 0.1)) / 2);
        attack = (int)(attack + ((attack * 0.1)) / 2);
        defense = (int)(defense + ((defense * 0.1)) / 2);
        speed = (int)(speed + ((speed * 0.1)) / 2);

        exp = remainingExp;
        expToLvl = level * random.Next(3, 5);
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
        SetExpToLevel(10);
        SetExp(0);

        switch (species)
        {
            case Species.Cookiedile:
                maxHealth = random.Next(60, 80);
                health = maxHealth;
                attack = random.Next(65, 75);
                defense = random.Next(50, 65);
                speed = random.Next(40, 45);
                return;
            case Species.Bonbonny:
                maxHealth = random.Next(45, 65);
                health = maxHealth;
                attack = random.Next(50, 60);
                defense = random.Next(45, 60);
                speed = random.Next(60, 65);
                return;
            case Species.Puppuff:
                maxHealth = random.Next(55, 75);
                health = maxHealth;
                attack = random.Next(60, 70);
                defense = random.Next(55, 70);
                speed = random.Next(55, 60);
                return;
            case Species.Cupcat:
                maxHealth = random.Next(50, 70);
                health = maxHealth;
                attack = random.Next(55, 65);
                defense = random.Next(60, 75);
                speed = random.Next(50, 55);
                return;
            case Species.Moofin:
                maxHealth = random.Next(80, 100);
                health = maxHealth;
                attack = random.Next(70, 80);
                defense = random.Next(75, 90);
                speed = random.Next(45, 50);
                return;
        }
    }
}