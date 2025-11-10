using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleNPC : MonoBehaviour
{
    [SerializeField]
    public GameObject player;

    private PlayerControls controls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new PlayerControls();
        controls.Player.Enable();

        Debug.Log(transform.position.x);
        Debug.Log(player.transform.position.x);
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
