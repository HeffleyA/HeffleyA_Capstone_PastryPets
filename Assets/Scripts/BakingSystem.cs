using System.Collections;
using UnityEngine;

public class BakingSystem
{
    private Inventory inventory = new Inventory();

    private PastryPetTeam team = new PastryPetTeam();

    public PastryPet BakePastryPet(string core, string flavor)
    {
        inventory.LoadItems();
        team.LoadMembers();

        PastryPet pet = new PastryPet();

        inventory.UseItem("Baking_Kit", 1);

        if (core.Contains("Core"))
        {
            inventory.UseItem(core, 1);
            switch (core)
            {
                case "Cookie_Core":
                    pet.SetSpecies(PastryPet.Species.Cookiedile);
                    break;
                case "Creampuff_Core":
                    pet.SetSpecies(PastryPet.Species.Puppuff);
                    break;
                case "Cupcake_Core":
                    pet.SetSpecies(PastryPet.Species.Cupcat);
                    break;
                case "Bonbon_Core":
                    pet.SetSpecies(PastryPet.Species.Bonbonny);
                    break;
                case "Muffin_Core":
                    pet.SetSpecies(PastryPet.Species.Moofin);
                    break;
                default:
                    break;
            }
        }

        pet.SetName(pet.GetSpecies().ToString());
        
        inventory.UseItem(flavor, 1);
        switch (flavor)
        {
            case "Vanilla":
                pet.SetType(PastryPet.Type.Basic);
                break;
            case "Cinnamon":
                pet.SetType(PastryPet.Type.Pyro);
                break;
            case "Sparkling_Water":
                pet.SetType(PastryPet.Type.Aqua);
                break;
            case "Nutmeg":
                pet.SetType(PastryPet.Type.Gaia);
                break;
            case "Himalayan_Salt":
                pet.SetType(PastryPet.Type.Terra);
                break;
            case "Lemon_Juice":
                pet.SetType(PastryPet.Type.Spark);
                break;
            case "Wasabi":
                pet.SetType(PastryPet.Type.Toxic);
                break;
            case "Rolled_Oats":
                pet.SetType(PastryPet.Type.Metallic);
                break;
            case "Whipped_Cream":
                pet.SetType(PastryPet.Type.Aero);
                break;
            case "Frosting":
                pet.SetType(PastryPet.Type.Arcane);
                break;
            default:
                break;
        }

        pet.SetLevel(1);
        pet.AssignWeakTo();
        pet.AssignBaseStats();

        if (team.GetMember1 == null)
        {
            team.SetMember1(pet);
        }
        else if (team.GetMember2 == null)
        {
            team.SetMember2(pet);
        }
        else if (team.GetMember3 == null)
        {
            team.SetMember3(pet);
        }
        else if (team.GetMember1 != null && team.GetMember2 != null && team.GetMember3 != null)
        {
            int member1Health = team.GetMember1.GetHealth();
            int member2Health = team.GetMember2.GetHealth();
            int member3Health = team.GetMember3.GetHealth();

            if (member1Health <= member2Health && member1Health <= member3Health)
            {
                team.SetMember1(pet);
            }
            else if (member2Health <= member1Health && member2Health <= member3Health)
            {
                team.SetMember2(pet);
            }
            else if (member3Health <= member1Health && member3Health <= member2Health)
            {
                team.SetMember3(pet);
            }
            else
            {
                team.SetMember1(pet);
            }
        }

        team.SaveMembers();

        return pet;
    }
}
