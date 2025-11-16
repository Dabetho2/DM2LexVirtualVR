using UnityEngine;
using UnityEngine.UI;

public class EndOfSessionReport : MonoBehaviour
{
    [Header("UI del reporte")]
    public Text idleEventsText;
    public Text badPostureEventsText;
    public Text badPostureTimeText;

    void Start()
    {
        var stats = HandIdleStatsManager.Instance;

        if (stats == null)
        {
            Debug.LogError("No existe HandIdleStatsManager en la escena.");
            return;
        }

        // Asignación de textos
        if (idleEventsText != null)
            idleEventsText.text = $"Eventos de manos quietas: {stats.totalIdleEvents}";


        if (badPostureEventsText != null)
            badPostureEventsText.text = $"Eventos de mala postura: {stats.totalBadPostureEvents}";

        if (badPostureTimeText != null)
            badPostureTimeText.text = $"Tiempo en mala postura: {stats.totalBadPostureTime:F1} seg";
    }
}
