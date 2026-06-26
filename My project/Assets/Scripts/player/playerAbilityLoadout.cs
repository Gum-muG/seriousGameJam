using UnityEngine;

public class playerAbilityLoadout : MonoBehaviour
{
    public Ability currentAbility;

    private float cooldownTimer;

    private void Update()
    {
        cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);
    }

    public void setAbility(Ability newAbility)
    {
        currentAbility = newAbility;
    }

    public bool canUseAbility()
    {
        return currentAbility != null && cooldownTimer <= 0f;
    }

    public void useAbility()
    {
        if (!canUseAbility())
            return;

        currentAbility.useAbility(gameObject);
        cooldownTimer = currentAbility.abilityCooldown;
    }
    public void useAbility(GameObject target)
    {
        if (!canUseAbility())
            return;

        currentAbility.useAbility(gameObject, target);

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