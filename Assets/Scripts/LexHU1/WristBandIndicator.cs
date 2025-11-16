using UnityEngine;

public class WristBandIndicator : MonoBehaviour
{
    [Header("Colores de la manilla")]
    public Color greenColor = new Color(0f, 1f, 0f);
    public Color yellowColor = new Color(1f, 0.92f, 0.016f);
    public Color redColor = new Color(1f, 0f, 0f);

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("[WristBand] No se encontró Renderer en este objeto.");
        }
        else
        {
            rend.material.color = greenColor; // estado inicial
        }
    }

    public void SetColor(string state)
    {
        if (rend == null) return;

        switch (state)
        {
            case "GREEN":
                rend.material.color = greenColor;
                break;

            case "YELLOW":
                rend.material.color = yellowColor;
                break;

            case "RED":
                rend.material.color = redColor;
                break;

            default:
                Debug.LogWarning($"[WristBand] Estado desconocido: {state}");
                break;
        }
    }
}
