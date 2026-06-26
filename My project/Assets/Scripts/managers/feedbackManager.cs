
using UnityEngine;

public class feedbackManager : MonoBehaviour
{
    public static feedbackManager instance;

    public void ShowAbilityUsed(string abilityName)
    {
        Debug.Log("used " + abilityName);
    }
}