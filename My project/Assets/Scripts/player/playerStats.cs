using UnityEngine;

public class playerStats : MonoBehaviour
{
    private playerPieceLoadout pieces;

    private void Awake()
    {
        pieces = GetComponent<playerPieceLoadout>();
    }

    public float GetAttackModifier()
    {
        float modifier = .1f;

        if (pieces.face != null)
            modifier += pieces.face.attackModifier;

        if (pieces.ring != null)
            modifier += pieces.ring.attackModifier;

        if (pieces.wheel != null)
            modifier += pieces.wheel.attackModifier;

        if (pieces.track != null)
            modifier += pieces.track.attackModifier;

        if (pieces.tip != null)
            modifier += pieces.tip.attackModifier;

        return modifier;
    }

    public float GetDefenseModifier()
    {
        float modifier = 1f;

        if (pieces.face != null)
            modifier += pieces.face.defenseModifier;

        if (pieces.ring != null)
            modifier += pieces.ring.defenseModifier;

        if (pieces.wheel != null)
            modifier += pieces.wheel.defenseModifier;

        if (pieces.track != null)
            modifier += pieces.track.defenseModifier;

        if (pieces.tip != null)
            modifier += pieces.tip.defenseModifier;

        return modifier;
    }
}