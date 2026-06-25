using UnityEngine;

public class playerCombat : MonoBehaviour
{
    private player playerController;
    private playerStats stats;

    private Vector3 startingPosition;

    private void Awake()
    {
        playerController = GetComponent<player>();
        stats = GetComponent<playerStats>();

        startingPosition = transform.position;
    }

    private void Start()
    {
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }

    public int GetCollisionDamage()
    {
        float damage = playerController.SpinSpeed;

        float attackMultiplier = stats.GetAttackModifier();

        damage *= attackMultiplier;

        return Mathf.RoundToInt(damage / 100f);
    }

    public void Damage(int damageAmount)
    {
        GameManager.instance.playerHealth.Damage(damageAmount);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);

        if (GameManager.instance.playerHealth.Health <= 0)
        {
            RespawnPlayer();
        }
    }

    public void Heal(int healAmount)
    {
        GameManager.instance.playerHealth.Heal(healAmount);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }

    private void RespawnPlayer()
    {
        respawnManager.instance.triggerRespawnScreen(startingPosition);
    }
}