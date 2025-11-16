using UnityEngine;
using TMPro;

public class HandIdleUIReporter : MonoBehaviour
{
    [Header("Referencia al detector de manos quietas")]
    public HandIdleDetector handIdleDetector;

    [Header("UI donde se mostrará el conteo")]
    public TextMeshProUGUI idleCounterText;

    void Update()
    {
        if (handIdleDetector == null || idleCounterText == null)
            return;

        idleCounterText.text = "Eventos de manos quietas: " + handIdleDetector.idleEventsCount;
    }
}
