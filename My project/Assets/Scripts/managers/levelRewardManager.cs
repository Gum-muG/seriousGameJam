using UnityEngine;

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
        enemy[] enemies = FindObjectsByType<enemy>();

        if (enemies.Length - 1 <= 0)
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
        ownedPiece newPiece = new ownedPiece();
        newPiece.blueprint = chosenPiece;
        newPiece.level = 1;

        playerLoadout.equip(newPiece);
    }
}