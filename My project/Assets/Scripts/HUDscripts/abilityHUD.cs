using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class abilityHUD : MonoBehaviour
{
    public TMP_Text slot1Text;
    public TMP_Text slot2Text;
    public TMP_Text slot3Text;

    public Image slot1Background;
    public Image slot2Background;
    public Image slot3Background;

    public Color normalColor = Color.white;
    public Color equippedColor = Color.yellow;

    private playerPieceLoadout playerLoadout;
    private playerAbilityLoadout abilityLoadout;

    private void Start()
    {
        playerLoadout = FindAnyObjectByType<playerPieceLoadout>();
        abilityLoadout = FindAnyObjectByType<playerAbilityLoadout>();
        updateAbilityDisplay();
    }

    private void Update()
    {
        if (playerLoadout == null)
            playerLoadout = FindAnyObjectByType<playerPieceLoadout>();

        if (abilityLoadout == null)
            abilityLoadout = FindAnyObjectByType<playerAbilityLoadout>();

        updateAbilityDisplay();
    }

    private void updateAbilityDisplay()
    {
        setSlotText(0, slot1Text, "1");
        setSlotText(1, slot2Text, "2");
        setSlotText(2, slot3Text, "3");

        updateEquippedHighlight();
    }

    private void setSlotText(int inventoryIndex, TMP_Text textBox, string hotkey)
    {
        if (playerLoadout == null || textBox == null)
            return;

        if (inventoryIndex >= playerLoadout.faceInventory.Count)
        {
            textBox.text = "[" + hotkey + "] Empty";
            return;
        }

        ownedPiece piece = playerLoadout.faceInventory[inventoryIndex];

        if (piece == null || piece.blueprint == null)
        {
            textBox.text = "[" + hotkey + "] Empty";
            return;
        }

        string abilityName = "No Ability";

        if (piece.blueprint.grantedAbility != null)
        {
            abilityName = piece.blueprint.grantedAbility.abilityName;
        }

        string text = "[" + hotkey + "] " + abilityName + " Lv." + piece.level;

        if (playerLoadout.selectedFaceSlot == inventoryIndex && abilityLoadout != null)
        {
            float cooldown = abilityLoadout.getCooldownRemaining();

            if (cooldown > 0f)
            {
                text += " (" + cooldown.ToString("F1") + "s)";
            }
        }

        textBox.text = text;
    }

    private void updateEquippedHighlight()
    {
        slot1Background.color = normalColor;
        slot2Background.color = normalColor;
        slot3Background.color = normalColor;

        switch (playerLoadout.selectedFaceSlot)
        {
            case 0:
                slot1Background.color = equippedColor;
                break;

            case 1:
                slot2Background.color = equippedColor;
                break;

            case 2:
                slot3Background.color = equippedColor;
                break;
        }
    }
}