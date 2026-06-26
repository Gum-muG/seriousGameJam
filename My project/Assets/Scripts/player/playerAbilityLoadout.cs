using UnityEngine;

public class playerAbilityLoadout : MonoBehaviour
{
    public Ability currentAbility;

    private float cooldownTimer;
    private ownedPiece equippedFacePiece;

    private void Update()
    {
        cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);
    }

    public void setAbility(ownedPiece facePiece)
    {
        equippedFacePiece = facePiece;

        if (facePiece != null && facePiece.blueprint != null)
            currentAbility = facePiece.getGrantedAbility();
        else
            currentAbility = null;

        cooldownTimer = 0f;
    }

    public bool canUseAbility()
    {
        return currentAbility != null && cooldownTimer <= 0f;
    }

    public void useAbility()
    {
        if (!canUseAbility())
            return;

        int abilityLevel = 1;

        if (equippedFacePiece != null)
            abilityLevel = equippedFacePiece.level;

        currentAbility.useAbility(gameObject, abilityLevel);
        
        feedbackManager.instance.ShowAbilityUsed(currentAbility.abilityName);

        cooldownTimer = currentAbility.abilityCooldown;
    }

    public void useAbility(GameObject target)
    {
        if (!canUseAbility())
            return;

        int abilityLevel = 1;

        if (equippedFacePiece != null)
            abilityLevel = equippedFacePiece.level;

        currentAbility.useAbility(gameObject, target, abilityLevel);

        feedbackManager.instance.ShowAbilityUsed(currentAbility.abilityName);

        cooldownTimer = currentAbility.abilityCooldown;
    }

    public float getCooldownRemaining()
    {
        return cooldownTimer;
    }

    public float getCooldownProgress()
    {
        if (currentAbility == null || currentAbility.abilityCooldown <= 0f)
            return 1f;

        return 1f - (cooldownTimer / currentAbility.abilityCooldown);
    }
}