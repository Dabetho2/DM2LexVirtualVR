using UnityEngine;

/// <summary>
/// Lleva TODAS las métricas globales de expresión corporal.
/// Es persistente entre escenas.
/// </summary>
public class HandIdleStatsManager : MonoBehaviour
{
    public static HandIdleStatsManager Instance;

    [Header("Eventos registrados durante la sesión")]
    public int totalIdleEvents = 0;            // Veces que se activó la alerta de manos quietas
    public int totalBadPostureEvents = 0;      // Veces que se activó la alerta de mala postura

    [Header("Tiempos registrados")]
    // Ya NO manejamos tiempo de quietud, solo tiempo de mala postura
    public float totalBadPostureTime = 0f;     // Tiempo acumulado en mala postura

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ----------------------------------------------------------
    // QUIETUD
    // ----------------------------------------------------------
    public void RegisterIdleEvent()
    {
        totalIdleEvents++;
        Debug.Log($"[Stats] Evento de manos quietas agregado. Total: {totalIdleEvents}");
    }

    // (Eliminado AddIdleTime, ya no se usa tiempo de quietud)

    // ----------------------------------------------------------
    // POSTURA
    // ----------------------------------------------------------
    public void RegisterBadPostureEvent()
    {
        totalBadPostureEvents++;
        Debug.Log($"[Stats] Evento de mala postura agregado. Total: {totalBadPostureEvents}");
    }

    public void AddBadPostureTime(float seconds)
    {
        totalBadPostureTime += seconds;
    }

    // ----------------------------------------------------------
    // RESET PARA NUEVA SIMULACIÓN
    // ----------------------------------------------------------
    public void ResetStats()
    {
        totalIdleEvents = 0;
        totalBadPostureEvents = 0;

        // Ya no hay totalIdleTime
        totalBadPostureTime = 0f;

        Debug.Log("[Stats] Métricas reiniciadas.");
    }
}
