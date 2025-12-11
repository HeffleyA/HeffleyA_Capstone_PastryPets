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

    private void Awake()
    {
        team = new PastryPetTeam();
        team.LoadMembers();

        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnNewGameButtonClicked()
    {
        team.SetMember1(null);
        team.SetMember2(null);
        team.SetMember3(null);
        team.SaveMembers();

        SceneManager.LoadScene("SampleScene");
    }

    private void OnContinueButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
