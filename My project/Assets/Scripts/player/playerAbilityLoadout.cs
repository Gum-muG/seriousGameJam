using UnityEngine;

public class playerAbilityLoadout : MonoBehaviour
{
    public Ability currentAbility;

    private float cooldownTimer;
    private float maxCooldown;
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

        if (feedbackManager.instance != null)
            feedbackManager.instance.ShowAbilityUsed(currentAbility.abilityName);

        maxCooldown = currentAbility.abilityCooldown;
        cooldownTimer = maxCooldown;
    }

    public float getCooldownRemaining()
    {
        return cooldownTimer;
    }

    public float getCooldownProgress()
    {
        if (maxCooldown <= 0f)
            return 1f;

        return 1f - (cooldownTimer / maxCooldown);
    }
}