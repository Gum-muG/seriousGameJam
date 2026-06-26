
using UnityEngine;

public class feedbackManager : MonoBehaviour
{
    public static feedbackManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowAbilityUsed(string abilityName)
    {
        Debug.Log("used " + abilityName);
    }
}