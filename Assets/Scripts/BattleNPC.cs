using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleNPC : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject panel1;
    [SerializeField] 
    public GameObject panel2;
    [SerializeField]
    public Button yesButton;
    [SerializeField]
    public Button noButton;
    [SerializeField]
    public Button okayButton;

    private PlayerControls controls;
    private PastryPetTeam team;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Enable();

        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);
        okayButton.onClick.AddListener(OnOkayButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(this.transform.position.x - player.transform.position.x) <= 0.65f)
        {
            if (controls.Player.Interact.IsPressed())
            {
                OnInteract();
            }
        }
    }

    private void OnInteract()
    {
        team = new PastryPetTeam();
        team.LoadMembers();

        panel1.SetActive(true);
    }

    private void OnYesButtonClick()
    {
        panel1.SetActive(false);

        if (team.GetMember1 == null)
        {
            panel2.SetActive(true);
        }
        else
        {
            controls.Player.Disable();
            SceneManager.LoadScene("BattleScene");
        }
    }

    private void OnNoButtonClick()
    {
        panel1.SetActive(false);
    }

    private void OnOkayButtonClick()
    {
        panel2.SetActive(false);
    }
}
