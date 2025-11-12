using System.IO;
using UnityEngine;

public class PastryPetTeam : MonoBehaviour
{
    private PastryPet member1;
    private PastryPet member2;
    private PastryPet member3;

    private string filePath = "C:\\Users\\aheffley\\Neumont\\Fall 2025\\Quarter\\PRO390-Capstone\\Projects\\Capstone-PastryPets\\Assets\\SaveData\\SavaData.txt";

    private void Awake()
    {
        LoadMembers();
    }

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
                $"\n{member1.GetHealth()}" +
                $"\n{member1.GetAttack()}" +
                $"\n{member1.GetDefense()}" +
                $"\n{member1.GetSpeed()}";

            baseInfo.Insert(0, member1Info);
        }

        if (member2 != null)
        {
            member2Info = $"{member2.GetName()}" +
                $"\n{member2.GetSpecies()}" +
                $"\n{member2.GetType()}" +
                $"\n{member2.GetWeakTo()}" +
                $"\n{member2.GetHealth()}" +
                $"\n{member2.GetAttack()}" +
                $"\n{member2.GetDefense()}" +
                $"\n{member2.GetSpeed()}";

            baseInfo.Insert(member1Info.Length + 1, member2Info);
        }

        if (member3 != null)
        {
            member3Info = $"{member3.GetName()}" +
                $"\n{member3.GetSpecies()}" +
                $"\n{member3.GetType()}" +
                $"\n{member3.GetWeakTo()}" +
                $"\n{member3.GetHealth()}" +
                $"\n{member3.GetAttack()}" +
                $"\n{member3.GetDefense()}" +
                $"\n{member3.GetSpeed()}";

            baseInfo.Insert(member2Info.Length + 1, member3Info);
        }

        return baseInfo;
    }

    private void SaveMembers()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        File.WriteAllText(filePath, GetAllMembersInfo());
    }

    private void LoadMembers()
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length >= 8)
        {
            member1.SetName(lines[0]);
            member1.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[1]));
            member1.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[2]));
            member1.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[3]));
            member1.SetHealth(System.Int32.Parse(lines[4]));
            member1.SetAttack(System.Int32.Parse(lines[5]));
            member1.SetDefense(System.Int32.Parse(lines[6]));
            member1.SetSpeed(System.Int32.Parse(lines[7]));

            if (lines.Length >= 16)
            {
                member1.SetName(lines[8]);
                member1.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[9]));
                member1.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[10]));
                member1.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[11]));
                member1.SetHealth(System.Int32.Parse(lines[12]));
                member1.SetAttack(System.Int32.Parse(lines[13]));
                member1.SetDefense(System.Int32.Parse(lines[14]));
                member1.SetSpeed(System.Int32.Parse(lines[15]));
            }

            if (lines.Length >= 24)
            {
                member1.SetName(lines[16]);
                member1.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[17]));
                member1.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[18]));
                member1.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[19]));
                member1.SetHealth(System.Int32.Parse(lines[20]));
                member1.SetAttack(System.Int32.Parse(lines[21]));
                member1.SetDefense(System.Int32.Parse(lines[22]));
                member1.SetSpeed(System.Int32.Parse(lines[23]));
            }
        }
    }
}
