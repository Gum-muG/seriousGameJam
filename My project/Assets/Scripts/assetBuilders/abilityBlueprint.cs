using UnityEngine;

[CreateAssetMenu(fileName = "ability", menuName = "Abilities")]

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public float abilityCooldown;
    public abilityTypes abilityType;
    
    public virtual void useAbility(GameObject user) { }
    public virtual void useAbility(GameObject user, GameObject target) { }

    public virtual void onHit(GameObject user, GameObject target) { }

    public virtual void applyBuff(GameObject user) { }

    public virtual void applyDebuff(GameObject user, GameObject target) { }

    public virtual void useAbility(GameObject user, int abilityLevel) { }

    public virtual void useAbility(GameObject user, GameObject target, int abilityLevel) { }
}