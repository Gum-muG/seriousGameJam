using UnityEngine;
using UnityEngine.SceneManagement;

public class levelRewardManager : MonoBehaviour
{
    public static levelRewardManager instance;

    public pieceBlueprint[] possibleFaceRewards;
    public rewardUI rewardUI;

    private playerPieceLoadout playerLoadout;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerLoadout = FindAnyObjectByType<playerPieceLoadout>();
    }

    public void checkLevelCleared()
    {
        enemy[] enemies = FindObjectsByType<enemy>(FindObjectsSortMode.None);

        if (enemies.Length <= 1)
        {
            showRewards();
        }
    }

    private void showRewards()
    {
        pieceBlueprint reward1 = possibleFaceRewards[Random.Range(0, possibleFaceRewards.Length)];
        pieceBlueprint reward2 = possibleFaceRewards[Random.Range(0, possibleFaceRewards.Length)];
        pieceBlueprint reward3 = possibleFaceRewards[Random.Range(0, possibleFaceRewards.Length)];

        rewardUI.Show(reward1, reward2, reward3);
    }

    public void chooseReward(pieceBlueprint chosenPiece)
    {
        if (chosenPiece == null)
            return;

        if (chosenPiece.pieceType != beybladePiece.face)
            return;

        if (playerLoadout.addOrUpgradeFacePiece(chosenPiece) == false)
        {
            Debug.Log("Face inventory full. Open replacement UI here.");
            return;
        }

        reloadCurrentScene();
    }

    private void reloadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}