using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject inventoryPanel;
    [SerializeField]
    public TextMeshProUGUI itemText;
    [SerializeField]
    public TextMeshProUGUI amountText;
    [SerializeField]
    public AudioSource music;

    private PlayerControls controls;
    private Vector2 movement;
    private Rigidbody2D rb;

    public float moveSpeed = 1.0f;

    private Inventory inventory;
    public Animator animator;

    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();

        inventory = new Inventory();

        music.Play();
    }

    private void Update()
    {
        if (controls.Player.Move.IsInProgress())
        {
            animator.SetBool("IsWalking", true);

            if (movement.x > 0)
            {
                animator.SetBool("IsFacingSide", true);
                animator.SetBool("IsFacingUp", false);
                animator.SetBool("IsFacingDown", false);
                
                transform.localScale = new Vector3(5, 5, 5);
            }
            else if (movement.x < 0)
            {
                animator.SetBool("IsFacingSide", true);
                animator.SetBool("IsFacingUp", false);
                animator.SetBool("IsFacingDown", false);

                transform.localScale = new Vector3(-5, 5, 5);
            }
            else if (movement.y > 0)
            {
                animator.SetBool("IsFacingUp", true);
                animator.SetBool("IsFacingDown", false);
                animator.SetBool("IsFacingSide", false);
            }
            else if (movement.y < 0)
            {
                animator.SetBool("IsFacingDown", true);
                animator.SetBool("IsFacingUp", false);
                animator.SetBool("IsFacingSide", false);
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (controls.Player.OpenInventory.triggered)
        {
            DisplayInventory();
        }

        if (controls.Player.QuitGame.IsPressed())
        {
            Application.Quit();
        }
    }

    private void DisplayInventory()
    {
        if (!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);

            inventory.LoadItems();
            string itemString = "";
            string amountString = "";

            foreach (var item in inventory.items)
            {
                string currentItem = "";
                string currentAmount = "";

                switch (item.GetItemType().ToString())
                {
                    case "Money":
                        currentItem = "Money";
                        break;
                    case "Baking_Kit":
                        currentItem = "Baking Kit";
                        break;
                    case "Cookie_Core":
                        currentItem = "Cookie Core";
                        break;
                    case "Creampuff_Core":
                        currentItem = "Creampuff Core";
                        break;
                    case "Cupcake_Core":
                        currentItem = "Cupcake Core";
                        break;
                    case "Bonbon_Core":
                        currentItem = "Bonbon Core";
                        break;
                    case "Muffin_Core":
                        currentItem = "Muffin Core";
                        break;
                    case "Vanilla":
                        currentItem = "Vanilla";
                        break;
                    case "Cinnamon":
                        currentItem = "Cinnamon";
                        break;
                    case "Sparkling_Water":
                        currentItem = "Sparkling Water";
                        break;
                    case "Nutmeg":
                        currentItem = "Nutmeg";
                        break;
                    case "Himalayan_Salt":
                        currentItem = "Himalayan Salt";
                        break;
                    case "Lemon_Juice":
                        currentItem = "Lemon Juice";
                        break;
                    case "Wasabi":
                        currentItem = "Wasabi";
                        break;
                    case "Rolled_Oats":
                        currentItem = "Rolled Oats";
                        break;
                    case "Whipped_Cream":
                        currentItem = "Whipped Cream";
                        break;
                    case "Frosting":
                        currentItem = "Frosting";
                        break;
                    default:
                        break;
                }

                currentAmount = item.GetAmountOwned().ToString();

                if (item != inventory.items[inventory.items.Length - 1])
                {
                    Debug.Log(currentItem);
                    itemString += $"{currentItem}\n";
                    Debug.Log(itemString);
                    amountString += $"x{currentAmount}\n";
                }
                else
                {
                    itemString += currentItem;
                    amountString += $"x{currentAmount}";
                }
            }

            itemText.text = itemString;
            amountText.text = amountString;
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = Vector2.zero;
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
