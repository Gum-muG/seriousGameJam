using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class respawnManager : MonoBehaviour
{
    public static respawnManager instance { get; private set; }

    [SerializeField] private GameObject respawnScreenUI;
    [SerializeField] private Button respawnButton;
    [SerializeField] private Button exitGameButton;

    private GameObject player;
    private Vector3 startingPos;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        respawnButton.onClick.AddListener(respawnPlayer);
        exitGameButton.onClick.AddListener(exitGame);

        player = GameObject.FindGameObjectWithTag("Player");

        respawnScreenUI.SetActive(false);
    }

    public void triggerRespawnScreen(Vector3 startingPosition)
    {
        Time.timeScale = 0f;
        startingPos = startingPosition;

        respawnScreenUI.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void respawnPlayer()
    {
        Time.timeScale = 1f;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player == null)
        {
            Debug.LogError("Respawn failed: Player not found.");
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.transform.position = startingPos;

        if (controller != null)
            controller.enabled = true;

        GameManager.instance.playerHealth.Health = 20;
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawnScreenUI.SetActive(false);
    }

    private void exitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}