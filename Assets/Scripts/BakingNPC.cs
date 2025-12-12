using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BakingNPC : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject firstPanel;
    [SerializeField]
    public GameObject secondPanel;
    [SerializeField]
    public GameObject thirdPanel;
    [SerializeField]
    public GameObject fourthPanel;
    [SerializeField] 
    public GameObject fifthPanel;
    [SerializeField]
    public Button yesButton;
    [SerializeField]
    public Button noButton;
    [SerializeField]
    public Button continueButton;
    [SerializeField]
    public Button submitButton;
    [SerializeField]
    public Button finishButton;
    [SerializeField] 
    public Button okayButton;
    [SerializeField]
    public TMP_Dropdown coreDropdown;
    [SerializeField]
    public TMP_Dropdown specialDropdown;
    [SerializeField]
    public TMP_Text finalText;

    private PlayerControls controls;

    private Inventory inventory = new Inventory();

    private string coreUsed = "";
    private string specialUsed = "";

    private PastryPet pet = new PastryPet();

    private BakingSystem system = new BakingSystem();

    private void Awake()
    {
        controls = new PlayerControls();

        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);
        continueButton.onClick.AddListener(OnContinueButtonClick);
        submitButton.onClick.AddListener(OnSubmitButtonClick);
        finishButton.onClick.AddListener(OnFinishButtonClick);
        okayButton.onClick.AddListener(OnOkayButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(this.transform.position.x - player.transform.position.x) <= 0.65f)
        {
            controls.Player.Enable();
            if (controls.Player.Interact.IsPressed())
            {
                OnInteract();
            }
        }
        else
        {
            controls.Player.Disable();
        }
    }

    private void OnInteract()
    {
        bool validItem = true;

        inventory.LoadItems();

        firstPanel.SetActive(true);

        foreach (var item in inventory.items)
        {
            Debug.Log($"{item.GetItemType().ToString()}: {item.GetAmountOwned()}");

            if (item.GetItemType().ToString() == "Baking_Kit" && item.GetAmountOwned() <= 0)
            {
                validItem = false;
            }
        }

        if (validItem)
        {
            yesButton.interactable = true;
        }
        else
        {
            yesButton.interactable = false;
        }
    }

    private void OnYesButtonClick()
    {
        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
    }
    
    private void OnNoButtonClick()
    {
        firstPanel.SetActive(false);
    }

    private void OnContinueButtonClick()
    {
        bool validItem = true;

        switch (coreDropdown.value)
        {
            case 0:
                coreUsed = "Cookie_Core";
                break;
            case 1:
                coreUsed = "Creampuff_Core";
                break;
            case 2:
                coreUsed = "Cupcake_Core";
                break;
            case 3:
                coreUsed = "Bonbon_Core";
                break;
            case 4:
                coreUsed = "Muffin_Core";
                break;
            default:
                break;
        }

        Debug.Log(coreUsed);

        foreach (var item in inventory.items)
        {
            if (item.GetItemType().ToString() == coreUsed && item.GetAmountOwned() <= 0)
            {
                Debug.Log($"{item.GetItemType().ToString()} found with {item.GetAmountOwned()} left");
                validItem = false;
            }
        }

        Debug.Log($"validItem is {validItem}");

        secondPanel.SetActive(false);

        if (validItem)
        {
            thirdPanel.SetActive(true);
        }
        else
        {
            fifthPanel.SetActive(true);
        }
    }

    private void OnSubmitButtonClick()
    {
        bool validItem = true;

        switch (specialDropdown.value)
        {
            case 0:
                specialUsed = "Vanilla";
                break;
            case 1:
                specialUsed = "Cinnamon";
                break;
            case 2:
                specialUsed = "Sparkling_Water";
                break;
            case 3:
                specialUsed = "Nutmeg";
                break;
            case 4:
                specialUsed = "Himalayan_Salt";
                break;
            case 5:
                specialUsed = "Lemon_Juice";
                break;
            case 6:
                specialUsed = "Wasabi";
                break;
            case 7:
                specialUsed = "Rolled_Oats";
                break;
            case 8:
                specialUsed = "Whipped_Cream";
                break;
            case 9:
                specialUsed = "Frosting";
                break;
            default:
                break;
        }

        foreach (var item in inventory.items)
        {
            if (item.GetItemType().ToString() == specialUsed && item.GetAmountOwned() <= 0)
            {
                validItem = false;
            }
        }

        thirdPanel.SetActive(false);

        if (validItem)
        {
            pet = system.BakePastryPet(coreUsed, specialUsed);

            finalText.text = $"Congrats!\n" +
            $"You have created a {pet.GetType()} type {pet.GetSpecies()} with the following stats!\n" +
            $"Health: {pet.GetMaxHealth()}\n" +
            $"Attack: {pet.GetAttack()}\n" +
            $"Defense: {pet.GetDefense()}\n" +
            $"Speed: {pet.GetSpeed()}";

            fourthPanel.SetActive(true);
        }
        else
        {
            fifthPanel.SetActive(true);
        }
    }

    private void OnFinishButtonClick()
    {
        inventory.LoadItems();
        fourthPanel.SetActive(false);
    }

    private void OnOkayButtonClick()
    {
        fifthPanel.SetActive(false);
    }
}
