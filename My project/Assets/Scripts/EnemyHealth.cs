using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void setHealth(int health)
    {
        text.text = health.ToString();
    }
}
