using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance {get; private set;}
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public Gradient healthGradient;
    public Image healthFill;

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthGradient.Evaluate(healthSlider.normalizedValue);
    }
    public void SetHealth(int health)
    {
        healthSlider.value = health;
        healthText.text = health.ToString();
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
