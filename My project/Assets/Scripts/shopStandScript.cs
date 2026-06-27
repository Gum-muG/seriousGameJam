using TMPro;
using UnityEngine;

public class shopStandScript : MonoBehaviour
{
    [Header("Shop")]
    public beybladePiece shopPieceType;
    public pieceBlueprint[] allPieces;
    public int price = 50;

    [Header("Selling")]
    public int baseSellPrice = 25;
    public int sellPricePerLevel = 15;

    [Header("Display")]
    public TMP_Text pieceNameText;
    public TMP_Text buyPriceText;
    public TMP_Text sellPriceText;

    private pieceBlueprint offeredPiece;
    private playerPieceLoadout playerLoadout;
    private playerCurrency currency;

    private bool playerInRange;
    private bool soldOut;

    private void Start()
    {
        playerLoadout = FindAnyObjectByType<playerPieceLoadout>();
        currency = FindAnyObjectByType<playerCurrency>();

        rollNewPiece();
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            buyPiece();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            sellEquippedPiece();
        }

        updateDisplay();
    }

    private void rollNewPiece()
    {
        offeredPiece = getRandomPiece();

        if (offeredPiece == null)
        {
            soldOut = true;
        }

        updateDisplay();
    }

    private void updateDisplay()
    {
        ownedPiece equippedPiece = getEquippedOwnedPiece();
        int currentSellPrice = getSellPrice(equippedPiece);

    // Piece Name
        if (soldOut || offeredPiece == null)
        pieceNameText.text = "Sold Out";
        else
            pieceNameText.text = offeredPiece.partName;

    // Buy Price
        if (soldOut || offeredPiece == null)
        {
            buyPriceText.text = "";
        }
        else
        {
            buyPriceText.text =
            "Buy: " + price +
            "\n[F]";
    }

    // Sell Price
    if (equippedPiece == null || equippedPiece.blueprint == null)
    {
        sellPriceText.text =
            "Nothing Equipped\n[G]";
    }
    else
    {
        sellPriceText.text =
            "Sell: " + currentSellPrice +
            "\n[G]";
    }
}

    private pieceBlueprint getRandomPiece()
    {
        pieceBlueprint equippedPiece = getEquippedBlueprint();

        pieceBlueprint[] validPieces = System.Array.FindAll(
            allPieces,
            piece =>
                piece != null &&
                piece.pieceType == shopPieceType &&
                piece != equippedPiece
        );

        if (validPieces.Length == 0)
            return null;

        return validPieces[Random.Range(0, validPieces.Length)];
    }

    private pieceBlueprint getEquippedBlueprint()
    {
        ownedPiece equippedPiece = getEquippedOwnedPiece();

        if (equippedPiece == null)
            return null;

        return equippedPiece.blueprint;
    }

    private ownedPiece getEquippedOwnedPiece()
    {
        if (playerLoadout == null)
            return null;

        switch (shopPieceType)
        {
            case beybladePiece.ring:
                return playerLoadout.ring;

            case beybladePiece.wheel:
                return playerLoadout.wheel;

            case beybladePiece.track:
                return playerLoadout.track;

            case beybladePiece.tip:
                return playerLoadout.tip;
        }

        return null;
    }

    private int getSellPrice(ownedPiece piece)
    {
        if (piece == null)
            return 0;

        return baseSellPrice + (piece.level - 1) * sellPricePerLevel;
    }

    private void buyPiece()
    {
        if (soldOut || offeredPiece == null)
            return;

        if (currency == null || !currency.spendCoins(price))
        {
            Debug.Log("Not enough coins.");
            return;
        }

        ownedPiece newPiece = new ownedPiece();
        newPiece.blueprint = offeredPiece;
        newPiece.level = 1;

        playerLoadout.equip(newPiece);

        Debug.Log("Purchased " + offeredPiece.partName);

        soldOut = true;
        offeredPiece = null;

        updateDisplay();
    }

    private void sellEquippedPiece()
    {
        ownedPiece equippedPiece = getEquippedOwnedPiece();

        if (equippedPiece == null || equippedPiece.blueprint == null)
        {
            Debug.Log("Nothing equipped to sell.");
            return;
        }

        int coinsEarned = getSellPrice(equippedPiece);

        if (currency != null)
        {
            currency.addCoins(coinsEarned);
        }

        Debug.Log("Sold " + equippedPiece.blueprint.partName + " Lv." + equippedPiece.level + " for " + coinsEarned);

        playerLoadout.unequip(shopPieceType);

        updateDisplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        updateDisplay();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
    }
}