using System.IO;
using UnityEngine;

public class PastryPetTeam : MonoBehaviour
{
    private GameObject member1Object;
    private GameObject member2Object;
    private GameObject member3Object;

    private PastryPet member1;
    private PastryPet member2;
    private PastryPet member3;

    public PastryPet GetMember1 { get { return member1; } }
    public PastryPet GetMember2 { get { return member2; } }
    public PastryPet GetMember3 { get { return member3; } }

    public void SetMember1(PastryPet member) { member1 = member; }
    public void SetMember2(PastryPet member) { member2 = member; }
    public void SetMember3(PastryPet member) { member3 = member; }

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
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length >= 9)
        {
            member1Object = new GameObject("PastryPet");
            member1 = member1Object.AddComponent<PastryPet>();
            member1.SetName(lines[0]);
            member1.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[1]));
            member1.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[2]));
            member1.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[3]));
            member1.SetHealth(System.Int32.Parse(lines[4]));
            member1.SetAttack(System.Int32.Parse(lines[5]));
            member1.SetDefense(System.Int32.Parse(lines[6]));
            member1.SetSpeed(System.Int32.Parse(lines[7]));
            member1.SetLevel(System.Int32.Parse(lines[8]));

            if (lines.Length >= 18)
            {
                member2Object = new GameObject("PastryPet");
                member2 = member2Object.AddComponent<PastryPet>();
                member2.SetName(lines[9]);
                member2.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[10]));
                member2.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[11]));
                member2.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[12]));
                member2.SetHealth(System.Int32.Parse(lines[13]));
                member2.SetAttack(System.Int32.Parse(lines[14]));
                member2.SetDefense(System.Int32.Parse(lines[15]));
                member2.SetSpeed(System.Int32.Parse(lines[16]));
                member2.SetLevel(System.Int32.Parse(lines[17]));
            }

            if (lines.Length >= 27)
            {
                member3Object = new GameObject("PastryPet");
                member3 = member3Object.AddComponent<PastryPet>();
                member3.SetName(lines[18]);
                member3.SetSpecies((PastryPet.Species)System.Enum.Parse(typeof(PastryPet.Species), lines[19]));
                member3.SetType((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[20]));
                member3.SetWeakTo((PastryPet.Type)System.Enum.Parse(typeof(PastryPet.Type), lines[21]));
                member3.SetHealth(System.Int32.Parse(lines[22]));
                member3.SetAttack(System.Int32.Parse(lines[23]));
                member3.SetDefense(System.Int32.Parse(lines[24]));
                member3.SetSpeed(System.Int32.Parse(lines[25]));
                member3.SetLevel(System.Int32.Parse(lines[26]));
            }
        }
    }
}
