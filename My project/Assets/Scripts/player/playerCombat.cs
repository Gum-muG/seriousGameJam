using UnityEngine;

public class playerCombat : MonoBehaviour
{
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;

        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
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