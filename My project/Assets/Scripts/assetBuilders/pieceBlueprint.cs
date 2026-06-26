using UnityEngine;

[CreateAssetMenu(fileName = "beybladePart", menuName = "Beyblade Parts")]
public class pieceBlueprint : ScriptableObject
{
    public string partName;

    public beybladePiece pieceType;

    public float attackModifier;
    public float defenseModifier;
    public float speedModifier;
    public float healthModifier;
    public Ability grantedAbility;
    public float upgradeModifier;
    

    public passiveEffect passiveEffect;
    public float passiveValue;
}