using UnityEngine;

[CreateAssetMenu(fileName = "wobbleAOE", menuName = "Abilities/wobbleAOE")]
public class wobbleAOE : Ability
{
    [SerializeField] public float radius = 5f;
    [SerializeField] public int wobbleDamage = 2;
    public LayerMask enemyLayer;

    public override void useAbility(GameObject user)
    {
        Collider[] enemiesInRadius = Physics.OverlapSphere(user.transform.position, radius, enemyLayer);

        foreach (Collider enemy in enemiesInRadius)
        {

            applyDebuff(user, enemy.gameObject);
        }       
    }

    public override void applyDebuff(GameObject user, GameObject target)
    {
        enemy enemyTarget = target.GetComponentInParent<enemy>();

        if (enemyTarget == null)
            return;

        enemyTarget.applyWobble(10f, 1);
    }

}