using TMPro;
using UnityEngine;

public class AlienLaneIndicator : MonoBehaviour
{
    public TextMeshPro textPro;
    
    public void SetDifficulty(int dif)
    {
        textPro.text = dif.ToString();
    }
}