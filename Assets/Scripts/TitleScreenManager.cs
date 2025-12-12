using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField]
    public Button newGameButton;
    [SerializeField]
    public Button continueButton;
    [SerializeField]
    public Button exitButton;

    private PastryPetTeam team;
    private Inventory inventory;

    private string filePath;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        team = new PastryPetTeam();
        team.LoadMembers();

        inventory = new Inventory();
        inventory.LoadItems();

        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnNewGameButtonClicked()
    {
        if (team.GetMember1 != null)
        {
            team.SetMember1(null);
        }

        if (team.GetMember2 != null)
        {
            team.SetMember2(null);
        }

        if (team.GetMember3 != null)
        {
            team.SetMember3(null);
        }

        team.SaveMembers();

        foreach (var item in inventory.items)
        {
            if (item == null)
                continue; // prevents crash

            if (item.GetItemType() == Item.ItemType.Money)
                item.SetAmountOwned(500);
            else
                item.SetAmountOwned(1);
        }
        inventory.SaveItems();

        controls.Disable();

        SceneManager.LoadScene("SampleScene");
    }

    private void OnContinueButtonClicked()
    {
        controls.Disable();

        if (inventory.items[0] == null || inventory.items[0].GetAmountOwned() <= 0)
        {
            foreach (var item in inventory.items)
            {
                if (item == null)
                    continue; // prevents crash

                if (item.GetItemType() == Item.ItemType.Money)
                    item.SetAmountOwned(500);
                else
                    item.SetAmountOwned(1);
            }
        }
        inventory.SaveItems();

        SceneManager.LoadScene("SampleScene");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (controls.Player.QuitGame.IsPressed())
        {
            Application.Quit();
        }
    }
}
