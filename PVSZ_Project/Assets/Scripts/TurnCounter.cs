using TMPro;
using UnityEngine;

public class TurnCounter :  MonoBehaviour
{
    public TextMeshProUGUI textPro;
    
    public void Awake()
    {
        textPro.text = "1";
    }
    
    public void IncrementCounter()
    {
        int nextWave = int.Parse(textPro.text) + 1;
        textPro.text = nextWave.ToString();
    }
}