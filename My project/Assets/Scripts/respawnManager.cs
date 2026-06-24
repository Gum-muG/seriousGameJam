using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class respawnManager : MonoBehaviour
{

    [SerializeField] private GameObject respawnScreenUI;
    [SerializeField] private Button respawnButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private GameObject player;
    private Vector3 startingPos;


    public static respawnManager instance {get; private set;}

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    
    public void triggerRespawnScreen(Vector3 startingPosition)
    {
        Time.timeScale = 0f;
        startingPos = startingPosition;
        respawnScreenUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        respawnButton.onClick.AddListener(respawnPlayer);
        exitGameButton.onClick.AddListener(exitGame);
    }

    private void respawnPlayer()
    {
        Time.timeScale = 1f;
        player.transform.position = startingPos;
        GameManager.instance.playerHealth.Health = 20;
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnScreenUI.SetActive(false);
    }

    private void exitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
