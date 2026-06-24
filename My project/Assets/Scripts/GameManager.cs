using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public HealthComponent playerHealth = new HealthComponent(20, 20);

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Destroy " + this);
            Destroy(this);
        }
        else
        {
            Debug.Log(this);
            instance = this;
        }
    }
}
