using System.IO;
using UnityEngine;

public class PastryPetTeam
{
    private PastryPet member1;
    private PastryPet member2;
    private PastryPet member3;

    public PastryPet GetMember1 { get { return member1; } }
    public PastryPet GetMember2 { get { return member2; } }
    public PastryPet GetMember3 { get { return member3; } }

    public void SetMember1(PastryPet member) 
    { 
        member1 = new PastryPet();
        member1 = member; 
    }
    public void SetMember2(PastryPet member) 
    { 
        member2 = new PastryPet();
        member2 = member; 
    }
    public void SetMember3(PastryPet member) 
    { 
        member3 = new PastryPet();
        member3 = member; 
    }

    private string filePath = "Assets/SaveData/TeamData.txt";

    private string GetAllMembersInfo()
    {
        string baseInfo = "";
        string member1Info = "";
        string member2Info = "";
        string member3Info = "";

        if (member1 != null)
        {
            member1Info = $"{member1.GetName()}" +
                $"\n{member1.GetSpecies()}" +
                $"\n{member1.GetType()}" +
                $"\n{member1.GetWeakTo()}" +
                $"\n{member1.GetMaxHealth()}" +
                $"\n{member1.GetHealth()}" +
                $"\n{member1.GetAttack()}" +
                $"\n{member1.GetDefense()}" +
                $"\n{member1.GetSpeed()}" +
                $"\n{member1.GetLevel()}";

            baseInfo = member1Info;
        }

        if (member2 != null)
        {
            member2Info = $"\n{member2.GetName()}" +
                $"\n{member2.GetSpecies()}" +
                $"\n{member2.GetType()}" +
                $"\n{member2.GetWeakTo()}" +
                $"\n{member2.GetMaxHealth()}" +
                $"\n{member2.GetHealth()}" +
                $"\n{member2.GetAttack()}" +
                $"\n{member2.GetDefense()}" +
                $"\n{member2.GetSpeed()}" +
                $"\n{member2.GetLevel()}";

            baseInfo = member1Info + member2Info;
        }

        if (member3 != null)
        {
            member3Info = $"\n{member3.GetName()}" +
                $"\n{member3.GetSpecies()}" +
                $"\n{member3.GetType()}" +
                $"\n{member3.GetWeakTo()}" +
                $"\n{member3.GetMaxHealth()}" +
                $"\n{member3.GetHealth()}" +
                $"\n{member3.GetAttack()}" +
                $"\n{member3.GetDefense()}" +
                $"\n{member3.GetSpeed()}" +
                $"\n{member3.GetLevel()}";

            baseInfo = member1Info + member2Info + member3Info;
        }

        return baseInfo;
    }

    public void SaveMembers()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        File.WriteAllText(filePath, GetAllMembersInfo());
    }

    public void LoadMembers()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length >= 10)
        {
            member1 = new PastryPet();
            member1.SetName(lines[0]);
            member1.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[1]));
            member1.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[2]));
            member1.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[3]));
            member1.SetMaxHealth(System.Int32.Parse(lines[4]));
            member1.SetHealth(System.Int32.Parse(lines[5]));
            member1.SetAttack(System.Int32.Parse(lines[6]));
            member1.SetDefense(System.Int32.Parse(lines[7]));
            member1.SetSpeed(System.Int32.Parse(lines[8]));
            member1.SetLevel(System.Int32.Parse(lines[9]));

            if (lines.Length >= 20)
            {
                member2 = new PastryPet();
                member2.SetName(lines[10]);
                member2.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[11]));
                member2.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[12]));
                member2.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[13]));
                member2.SetMaxHealth(System.Int32.Parse(lines[14]));
                member2.SetHealth(System.Int32.Parse(lines[15]));
                member2.SetAttack(System.Int32.Parse(lines[16]));
                member2.SetDefense(System.Int32.Parse(lines[17]));
                member2.SetSpeed(System.Int32.Parse(lines[18]));
                member2.SetLevel(System.Int32.Parse(lines[19]));
            }

            if (lines.Length >= 30)
            {
                member3 = new PastryPet();
                member3.SetName(lines[20]);
                member3.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[21]));
                member3.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[22]));
                member3.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[23]));
                member3.SetMaxHealth(System.Int32.Parse(lines[24]));
                member3.SetHealth(System.Int32.Parse(lines[25]));
                member3.SetAttack(System.Int32.Parse(lines[26]));
                member3.SetDefense(System.Int32.Parse(lines[27]));
                member3.SetSpeed(System.Int32.Parse(lines[28]));
                member3.SetLevel(System.Int32.Parse(lines[29]));
            }
        }
    }
}
