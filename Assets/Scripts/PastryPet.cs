
using System;
using UnityEngine;

public class PastryPet : MonoBehaviour
{
    System.Random random = new System.Random();

    enum Species
    {
        Cookiedile,
        Bonbonny,
        Puppuff,
        Cupcat,
        Moofin,
        Default
    }

    enum Type
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
    private float health;
    private float attack;
    private float defense;
    private float speed;
    private int level;

    public float Health;
    public float Attack;
    public float Defense;
    public float Speed;

    private bool critHit = false;

    private PastryPet()
    {
        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
    }

    void OnLevelUp()
    {
        level++;

        health = (float)(health + ((health * 0.1)) / 2);
        attack = (float)(attack + ((attack * 0.1)) / 2);
        defense = (float)(defense + ((defense * 0.1)) / 2);
        speed = (float)(speed + ((speed * 0.1)) / 2);
    }

    public void TakeDamage(PastryPet opp)
    {
        float damage = 0;
        float critChance = (float)(random.Next(0, 10));

        if (opp.type == weakTo)
        {
            damage += (float)((opp.attack * 0.1) * 1.5);
        }
        else
        {
            damage += (float)(opp.attack * 0.1);
        }

        if (critChance == 5)
        {
            damage += (float)(damage * 1.5);
            critHit = true;
        }

        health -= damage;
    }

    void AssignBaseStats()
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