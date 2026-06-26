using UnityEngine;

public class lava : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        player playerRef = other.gameObject.GetComponentInParent<player>();
        playerCombat combat = playerRef.GetComponent<playerCombat>();
        combat.Kill();
    }
}
