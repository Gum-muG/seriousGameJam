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
        if (blueprint == null){
        return 0f;
        }
        return blueprint.attackModifier + blueprint.upgradeModifier * (level - 1);
    }

    public float getDefenseModifier()
    {   
        if (blueprint == null){
        return 0f;
        }
        return blueprint.defenseModifier + blueprint.upgradeModifier * (level - 1);
    }

    public float getSpeedModifier()
    {
        if (blueprint == null){
        return 0f;
        }
        return blueprint.speedModifier + blueprint.upgradeModifier * (level - 1);
    }

    public float getHealthModifier()
    {
        if (blueprint == null){
        return 0f;
        }
        return blueprint.healthModifier + blueprint.upgradeModifier * (level - 1);
    }

    public Ability getGrantedAbility()
    {   
        if (blueprint == null){
        return null;
        }
        return blueprint.grantedAbility;
    }
}