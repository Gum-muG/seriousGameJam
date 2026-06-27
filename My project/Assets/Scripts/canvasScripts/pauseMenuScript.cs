using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    public GameObject mainPausePanel;
    public GameObject loadoutPanel;

    public Button resumeButton;
    public Button viewLoadoutButton;
    public Button exitGameButton;
    public Button backButton;

    public TMP_Text faceText;
    public TMP_Text ringText;
    public TMP_Text wheelText;
    public TMP_Text trackText;
    public TMP_Text tipText;
    public TMP_Text totalModifiersText;

    private playerPieceLoadout loadout;
    private playerStats stats;
    private bool paused;

    private void Start()
    {
        refreshReferences();

        mainPausePanel.SetActive(false);
        loadoutPanel.SetActive(false);

        resumeButton.onClick.AddListener(resumeGame);
        viewLoadoutButton.onClick.AddListener(showLoadoutPanel);
        exitGameButton.onClick.AddListener(exitGame);
        backButton.onClick.AddListener(showMainPausePanel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                resumeGame();
            else
                pauseGame();
        }
    }

    private void pauseGame()
    {
        paused = true;
        Time.timeScale = 0f;

        showMainPausePanel();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void resumeGame()
    {
        paused = false;
        Time.timeScale = 1f;

        mainPausePanel.SetActive(false);
        loadoutPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void showMainPausePanel()
    {
        mainPausePanel.SetActive(true);
        loadoutPanel.SetActive(false);
    }

    private void showLoadoutPanel()
    {
        refreshReferences();

        if (loadout == null || stats == null)
        {
            Debug.LogError("Pause menu missing player loadout or stats.");
            return;
        }

        updateLoadoutText();

        mainPausePanel.SetActive(false);
        loadoutPanel.SetActive(true);
    }

    private void refreshReferences()
    {
        loadout = FindAnyObjectByType<playerPieceLoadout>();
        stats = FindAnyObjectByType<playerStats>();
    }

    private void updateLoadoutText()
    {
        faceText.text = getPieceDetails("Face", loadout.face);
        ringText.text = getPieceDetails("Ring", loadout.ring);
        wheelText.text = getPieceDetails("Wheel", loadout.wheel);
        trackText.text = getPieceDetails("Track", loadout.track);
        tipText.text = getPieceDetails("Tip", loadout.tip);

        totalModifiersText.text =
            "Total Modifiers\n\n" +
            "Attack: " + stats.getAttackModifier().ToString("F2") + "\n" +
            "Defense: " + stats.getDefenseModifier().ToString("F2") + "\n" +
            "Speed: " + stats.getSpeedModifier().ToString("F2") + "\n" +
            "Health: " + stats.getHealthModifier().ToString("F2");
    }

    private string getPieceDetails(string slotName, ownedPiece piece)
    {
        if (piece == null || piece.blueprint == null)
            return slotName + "\nN/A";

        pieceBlueprint blueprint = piece.blueprint;

        string details =
            slotName + "\n" +
            blueprint.partName + " Lv." + piece.level +
            "\nATK: " + piece.getAttackModifier().ToString("F2") +
            "  DEF: " + piece.getDefenseModifier().ToString("F2") +
            "\nSPD: " + piece.getSpeedModifier().ToString("F2") +
            "  HP: " + piece.getHealthModifier().ToString("F2");

        if (blueprint.grantedAbility != null)
        {
            details += "\nAbility: " + blueprint.grantedAbility.abilityName;
        }

        if (blueprint.passiveEffect != passiveEffect.none)
        {
            details += "\nPassive: " + blueprint.passiveEffect + " " + blueprint.passiveValue;
        }

        return details;
    }

    private void exitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}