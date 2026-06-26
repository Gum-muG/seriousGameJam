using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class rewardUI : MonoBehaviour
{
    public GameObject rewardPanel;

    public Button option1Button;
    public Button option2Button;
    public Button option3Button;

    public TMP_Text option1Text;
    public TMP_Text option2Text;
    public TMP_Text option3Text;

    private pieceBlueprint reward1;
    private pieceBlueprint reward2;
    private pieceBlueprint reward3;

    public void Show(pieceBlueprint option1, pieceBlueprint option2, pieceBlueprint option3)
    {
        reward1 = option1;
        reward2 = option2;
        reward3 = option3;

        option1Text.text = reward1.partName;
        option2Text.text = reward2.partName;
        option3Text.text = reward3.partName;

        rewardPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();
        option3Button.onClick.RemoveAllListeners();

        option1Button.onClick.AddListener(() => Choose(reward1));
        option2Button.onClick.AddListener(() => Choose(reward2));
        option3Button.onClick.AddListener(() => Choose(reward3));
    }

    private void Choose(pieceBlueprint chosenPiece)
    {
        levelRewardManager.instance.chooseReward(chosenPiece);

        rewardPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}