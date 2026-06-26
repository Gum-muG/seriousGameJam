using UnityEngine;

[System.Serializable]
public class ownedPiece
{
    public pieceBlueprint blueprint;

    public int level = 1;
    public int maxLevel = 5;

  public bool IsPassiveUnlocked()
  {
      return level >= 3;
  }

    public void upgrade()
    {
        if (level >= maxLevel)
            return;

        level++;
    }

    public float getAttackModifier()
    {
        return blueprint.attackModifier + blueprint.upgradeModifier * (level - 1);
    }

    public float getDefenseModifier()
    {
        return blueprint.defenseModifier + blueprint.upgradeModifier * (level - 1);
    }

    public float getSpeedModifier()
    {
        return blueprint.speedModifier + blueprint.upgradeModifier * (level - 1);
    }

    public float getHealthModifier()
    {
        return blueprint.healthModifier + blueprint.upgradeModifier * (level - 1);
    }

    public Ability getGrantedAbility()
    {
        return blueprint.grantedAbility;
    }
}