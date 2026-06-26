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
            modifier += pieces.face.getAttackModifier();

        if (pieces.ring != null)
            modifier += pieces.ring.getAttackModifier();

        if (pieces.wheel != null)
            modifier += pieces.wheel.getAttackModifier();

        if (pieces.track != null)
            modifier += pieces.track.getAttackModifier();

        if (pieces.tip != null)
            modifier += pieces.tip.getAttackModifier();

        return modifier;
    }

    public float getDefenseModifier()
    {
        float modifier = 1f;

        if (pieces.face != null)
            modifier += pieces.face.getDefenseModifier();

        if (pieces.ring != null)
            modifier += pieces.ring.getDefenseModifier();

        if (pieces.wheel != null)
            modifier += pieces.wheel.getDefenseModifier();

        if (pieces.track != null)
            modifier += pieces.track.getDefenseModifier();

        if (pieces.tip != null)
            modifier += pieces.tip.getDefenseModifier();

        return modifier;
    }

    public float getSpeedModifier()
{
    float modifier = 1f;

    if (pieces.face != null) modifier += pieces.face.getSpeedModifier();
    if (pieces.ring != null) modifier += pieces.ring.getSpeedModifier();
    if (pieces.wheel != null) modifier += pieces.wheel.getSpeedModifier();
    if (pieces.track != null) modifier += pieces.track.getSpeedModifier();
    if (pieces.tip != null) modifier += pieces.tip.getSpeedModifier();

    return modifier;
}

public float getHealthModifier()
{
    float modifier = 1f;

    if (pieces.face != null) modifier += pieces.face.getHealthModifier();
    if (pieces.ring != null) modifier += pieces.ring.getHealthModifier();
    if (pieces.wheel != null) modifier += pieces.wheel.getHealthModifier();
    if (pieces.track != null) modifier += pieces.track.getHealthModifier();
    if (pieces.tip != null) modifier += pieces.tip.getHealthModifier();

    return modifier;
}
}