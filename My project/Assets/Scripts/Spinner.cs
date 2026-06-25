using UnityEngine;

public class Spinner : MonoBehaviour
{
    private int playerLayer;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            GameManager.instance.playerHealth.Heal(10);
            HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1));
    }
}
