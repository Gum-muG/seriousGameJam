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

    public int getOutgoingCollisionDamage()
    {
        float damage = GameManager.instance.playerHealth.Health;

        damage *= stats.getAttackModifier();

        return Mathf.CeilToInt(damage);
    }

    public int getIncomingCollisionDamage()
    {
        float damage = 2f;

        damage /= stats.getDefenseModifier();

        return Mathf.CeilToInt(damage);
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

    public void Kill()
    {
        GameManager.instance.playerHealth.Health = 0;
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        RespawnPlayer();
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