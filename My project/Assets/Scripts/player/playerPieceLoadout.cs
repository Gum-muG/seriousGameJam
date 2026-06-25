using UnityEngine;

public class playerPieceLoadout : MonoBehaviour
{
    public pieceBlueprint face;
    public pieceBlueprint ring;
    public pieceBlueprint wheel;
    public pieceBlueprint track;
    public pieceBlueprint tip;

    public void Equip(pieceBlueprint part)
    {
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
    }
}