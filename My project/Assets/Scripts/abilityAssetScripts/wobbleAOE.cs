using UnityEngine;

[CreateAssetMenu(fileName = "wobbleAOE", menuName = "Abilities/Wobble AOE")]
public class wobbleAOE : Ability
{
    [SerializeField] private float baseRadius = 5f;
    [SerializeField] private float radiusPerLevel = 0.5f;

    [SerializeField] private float baseDuration = 10f;
    [SerializeField] private float durationPerLevel = 2f;

    [SerializeField] private int baseDamage = 1;
    [SerializeField] private int damagePerLevel = 1;

    [SerializeField] private LayerMask enemyLayer;

    public override void useAbility(GameObject user, int abilityLevel)
    {
        float radius = baseRadius + radiusPerLevel * (abilityLevel - 1);
        float duration = baseDuration + durationPerLevel * (abilityLevel - 1);
        int damage = baseDamage + damagePerLevel * (abilityLevel - 1);

        Collider[] enemiesInRadius = Physics.OverlapSphere(
            user.transform.position,
            radius,
            enemyLayer);

        foreach (Collider enemyCollider in enemiesInRadius)
        {
            applyDebuff(user, enemyCollider.gameObject, duration, damage);
        }
    }

    private void applyDebuff(GameObject user, GameObject target, float duration, int damage)
    {
        enemy enemyTarget = target.GetComponentInParent<enemy>();

        if (enemyTarget == null)
            return;

        enemyTarget.applyWobble(duration, damage);
    }
}