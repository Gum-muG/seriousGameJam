using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public HealthComponent playerHealth = new HealthComponent(20, 20);
    void Awake()
    {
        
    }
}
