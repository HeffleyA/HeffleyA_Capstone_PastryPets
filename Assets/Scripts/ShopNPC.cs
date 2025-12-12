using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject shopPanel;
    [SerializeField]
    public TMP_Dropdown shopDropdown;
    [SerializeField]
    public TMP_InputField shopInput;
    [SerializeField]
    public Button submitButton;
    [SerializeField]
    public Button cancelButton;

    private Inventory inventory;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        inventory = new Inventory();

        submitButton.onClick.AddListener(OnSubmitButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void Update()
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
        inventory.LoadItems();
        shopPanel.SetActive(true);
    }

    private void OnSubmitButtonClicked()
    {
        string itemString = "";
        string itemAmount = shopInput.text;
        int itemInt = System.Int32.Parse(itemAmount);
        
        for (int i = 0; i < inventory.items.Length - 1; i++)
        {
            if (inventory.items[i].GetItemType() == Item.ItemType.Money) continue;
            else if (i == shopDropdown.value + 1)
            {
                itemString = inventory.items[i].GetItemType().ToString();
            }
        }

        inventory.GainItem(itemString, itemInt);

        shopPanel.SetActive(false);

        controls.Player.Disable();
    }

    private void OnCancelButtonClicked()
    {
        shopPanel.SetActive(false);

        controls.Player.Disable();
    }
}
