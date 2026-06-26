using UnityEngine;

public class playerPieceLoadout : MonoBehaviour
{
    public pieceBlueprint face;
    public pieceBlueprint ring;
    public pieceBlueprint wheel;
    public pieceBlueprint track;
    public pieceBlueprint tip;

    private playerAbilityLoadout abilities;

    private void Start()
    {
        updateAbility();
    }

    private void Awake()
    {
        abilities = GetComponent<playerAbilityLoadout>();
    }

    public void equip(pieceBlueprint part)
    {
        if (part == null)
            return;

        switch (part.pieceType)
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

        if (face != null)
            abilities.setAbility(face.grantedAbility);
        else
            abilities.setAbility(null);
    }
}