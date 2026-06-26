using System.Collections.Generic;
using UnityEngine;

public class playerPieceLoadout : MonoBehaviour
{
    public ownedPiece face;
    public ownedPiece ring;
    public ownedPiece wheel;
    public ownedPiece track;
    public ownedPiece tip;
    public int selectedFaceSlot = 0;

    public int maxFacePieces = 3;
    public List<ownedPiece> faceInventory = new List<ownedPiece>();

    private pieceBlueprint pendingFaceBlueprint;
    private playerAbilityLoadout abilities;

    private void Awake()
    {
        abilities = GetComponent<playerAbilityLoadout>();
    }

    private void Start()
    {
        equipFaceFromInventory(0);
    }

    public void equip(ownedPiece part)
    {
        if (part == null || part.blueprint == null)
            return;

        switch (part.blueprint.pieceType)
        {
            case beybladePiece.face:
                face = part;
                break;

            case beybladePiece.ring:
                ring = part;
                break;

            case beybladePiece.wheel:
                wheel = part;
                break;

            case beybladePiece.track:
                track = part;
                break;

            case beybladePiece.tip:
                tip = part;
                break;
        }

        updateAbility();
    }

    public bool addOrUpgradeFacePiece(pieceBlueprint blueprint)
    {
        if (blueprint == null)
            return false;

        if (blueprint.pieceType != beybladePiece.face)
            return false;

        foreach (ownedPiece piece in faceInventory)
        {
            if (piece.blueprint == blueprint)
            {
                piece.upgrade();
                return true;
            }
        }

        if (faceInventory.Count < maxFacePieces)
        {
            ownedPiece newPiece = new ownedPiece();
            newPiece.blueprint = blueprint;
            newPiece.level = 1;

            faceInventory.Add(newPiece);
            return true;
        }

        pendingFaceBlueprint = blueprint;
        return false;
    }

    public void replaceFacePiece(int inventoryIndex)
    {
        if (pendingFaceBlueprint == null)
            return;

        if (inventoryIndex < 0 || inventoryIndex >= faceInventory.Count)
            return;

        ownedPiece newPiece = new ownedPiece();
        newPiece.blueprint = pendingFaceBlueprint;
        newPiece.level = 1;

        bool replacingEquippedFace = faceInventory[inventoryIndex] == face;

        faceInventory[inventoryIndex] = newPiece;

        if (replacingEquippedFace)
        {
            face = newPiece;
            updateAbility();
        }

        pendingFaceBlueprint = null;
    }

    public void equipFaceFromInventory(int inventoryIndex)
    {
        if (inventoryIndex < 0 || inventoryIndex >= maxFacePieces)
            return;

        selectedFaceSlot = inventoryIndex;

        if (inventoryIndex >= faceInventory.Count)
        {
            face = null;
            updateAbility();
            return;
        }

    face = faceInventory[inventoryIndex];
    updateAbility();
}

    public void unequip(beybladePiece pieceType)
    {
        switch (pieceType)
        {
            case beybladePiece.face:
                face = null;
                break;

            case beybladePiece.ring:
                ring = null;
                break;

            case beybladePiece.wheel:
                wheel = null;
                break;

            case beybladePiece.track:
                track = null;
                break;

            case beybladePiece.tip:
                tip = null;
                break;
        }

        updateAbility();
    }

    private void updateAbility()
    {
        if (abilities == null)
            return;

        abilities.setAbility(face);
    }
}