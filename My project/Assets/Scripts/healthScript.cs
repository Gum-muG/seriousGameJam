using UnityEngine;

public class HealthComponent
{
    int health;
    int maxHealth;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
    public HealthComponent(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }
    public void Damage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        if (health < 0)
        {
            health = 0;
        }
    }
    public void Heal(int heal)
    {
        if (health < maxHealth)
        {
            health += heal;
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

