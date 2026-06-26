using UnityEngine;

public class playerPieceLoadout : MonoBehaviour
{
    public ownedPiece face;
    public ownedPiece ring;
    public ownedPiece wheel;
    public ownedPiece track;
    public ownedPiece tip;

    private playerAbilityLoadout abilities;

    private void Awake()
    {
        abilities = GetComponent<playerAbilityLoadout>();
    }

    private void Start()
    {
        updateAbility();
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