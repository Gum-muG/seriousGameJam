using UnityEngine;

public class upgradeStation : MonoBehaviour
{
    public int upgradeCost = 10;

    public upgradeUI upgradeUI;

    private playerPieceLoadout playerLoadout;
    private playerCurrency currency;

    private bool playerInRange;

    private void Start()
    {
        playerLoadout = FindAnyObjectByType<playerPieceLoadout>();
        currency = FindAnyObjectByType<playerCurrency>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            upgradeUI.Show();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered: " + other.name);
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press F to upgrade.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void upgradeRing()
    {
        upgradePiece(playerLoadout.ring);
    }

    public void upgradeWheel()
    {
        upgradePiece(playerLoadout.wheel);
    }

    public void upgradeTrack()
    {
        upgradePiece(playerLoadout.track);
    }

    public void upgradeTip()
    {
        upgradePiece(playerLoadout.tip);
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

        if (!currency.spendCoins(upgradeCost))
        {
            Debug.Log("Not enough coins.");
            return;
        }

        piece.upgrade();
        Debug.Log(piece.blueprint.partName + " upgraded to level " + piece.level);
    }
}