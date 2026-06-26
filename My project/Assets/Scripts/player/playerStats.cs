using UnityEngine;

public class playerStats : MonoBehaviour
{
    private playerPieceLoadout pieces;

    private void Awake()
    {
        pieces = GetComponent<playerPieceLoadout>();
    }

    public float getAttackModifier()
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

    public float getDefenseModifier()
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

    public float getSpeedModifier()
{
    float modifier = 1f;

    if (pieces.face != null) modifier += pieces.face.speedModifier;
    if (pieces.ring != null) modifier += pieces.ring.speedModifier;
    if (pieces.wheel != null) modifier += pieces.wheel.speedModifier;
    if (pieces.track != null) modifier += pieces.track.speedModifier;
    if (pieces.tip != null) modifier += pieces.tip.speedModifier;

    return modifier;
}

public float getHealthModifier()
{
    float modifier = 1f;

    if (pieces.face != null) modifier += pieces.face.healthModifier;
    if (pieces.ring != null) modifier += pieces.ring.healthModifier;
    if (pieces.wheel != null) modifier += pieces.wheel.healthModifier;
    if (pieces.track != null) modifier += pieces.track.healthModifier;
    if (pieces.tip != null) modifier += pieces.tip.healthModifier;

    return modifier;
}
}