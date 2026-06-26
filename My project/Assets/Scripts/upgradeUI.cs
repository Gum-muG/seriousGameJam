using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class upgradeUI : MonoBehaviour
{
    public GameObject upgradeCanvas;

    public Button ringButton;
    public Button wheelButton;
    public Button trackButton;
    public Button tipButton;

    public TMP_Text ringText;
    public TMP_Text wheelText;
    public TMP_Text trackText;
    public TMP_Text tipText;

    public int upgradeCost = 10;

    private playerPieceLoadout playerLoadout;
    private playerCurrency currency;

    private void Start()
    {
        playerLoadout = FindAnyObjectByType<playerPieceLoadout>();
        currency = FindAnyObjectByType<playerCurrency>();

        upgradeCanvas.SetActive(false);

        ringButton.onClick.AddListener(() => upgradePiece(playerLoadout.ring));
        wheelButton.onClick.AddListener(() => upgradePiece(playerLoadout.wheel));
        trackButton.onClick.AddListener(() => upgradePiece(playerLoadout.track));
        tipButton.onClick.AddListener(() => upgradePiece(playerLoadout.tip));
    }

    public void Show()
    {
        updateText();
        upgradeCanvas.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        upgradeCanvas.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    private void upgradePiece(ownedPiece piece)
    {
        if (piece == null || piece.blueprint == null)
            return;

        if (piece.level >= piece.maxLevel)
        {
            Debug.Log("Piece is already max level.");
            return;
        }

        if (currency.spendCoins(upgradeCost) == false)
        {
            Debug.Log("Not enough coins.");
            return;
        }

        piece.upgrade();
        updateText();
    }

    private void updateText()
    {
        setButtonText(playerLoadout.ring, ringText, "Ring");
        setButtonText(playerLoadout.wheel, wheelText, "Wheel");
        setButtonText(playerLoadout.track, trackText, "Track");
        setButtonText(playerLoadout.tip, tipText, "Tip");
    }

    private void setButtonText(ownedPiece piece, TMP_Text textBox, string slotName)
    {
        if (piece == null || piece.blueprint == null)
        {
            textBox.text = slotName + ": Empty";
            return;
        }

        textBox.text = piece.blueprint.partName + " Lv." + piece.level + " - Cost: " + upgradeCost;
    }
}
