using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelRewardManager : MonoBehaviour
{
    public static levelRewardManager instance;

    public pieceBlueprint[] possibleFaceRewards;
    public rewardUI rewardUI;

    private playerPieceLoadout playerLoadout;

    private int enemiesAlive;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerLoadout = FindAnyObjectByType<playerPieceLoadout>();
    }

    public void registerEnemy()
    {
        enemiesAlive++;
    }

    public void enemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
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

        if (playerLoadout == null)
        {
            playerLoadout = FindAnyObjectByType<playerPieceLoadout>();
        }

        if (playerLoadout == null)
        {
            Debug.LogError("No playerPieceLoadout found.");
            return;
        }

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

        if (GameManager.instance != null)
        {
            GameManager.instance.advanceLevel();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void loadRandomScene()
    {
        Time.timeScale = 1f;

        if (GameManager.instance != null)
        {
            GameManager.instance.advanceLevel();
        }

        SceneManager.LoadScene("New Scene 1");
    }
}

