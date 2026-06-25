using UnityEngine;

[CreateAssetMenu(fileName = "Beyblade Part", menuName = "Equipment/Bey Part")]
public class pieceBlueprint : ScriptableObject
{
    public string partName;

    public beybladePiece pieceType;

    public float attackModifier;
    public float defenseModifier;
    public float staminaModifier;

    public passiveEffect passiveEffect;
    public float passiveValue;
}