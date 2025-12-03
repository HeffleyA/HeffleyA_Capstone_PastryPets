using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleNPC : MonoBehaviour
{
    [SerializeField]
    public GameObject player;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(this.transform.position.x - player.transform.position.x) <= 0.05f)
        {
            if (controls.Player.Interact.IsPressed())
            {
                OnInteract();
            }
        }
    }

    private void OnInteract()
    {
        controls.Player.Disable();
        SceneManager.LoadScene("BattleScene");
    }
}
