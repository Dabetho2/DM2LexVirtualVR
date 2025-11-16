using UnityEngine;

public class HandPostureIndicator : MonoBehaviour
{
    [Header("Hands")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("User Reference")]
    public Transform head;

    [Header("Wrist Band (HU 1.3.1)")]
    public WristBandIndicator wristBand;

    [Header("Posture UI (para HU 2.1.1)")]
    public GameObject postureIcon;

    [Header("Position Rules")]
    public float minHeight = -0.25f;  // demasiado abajo = rojo
    public float maxHeight = 0.40f;   // demasiado arriba = rojo
    public float maxSide = 0.45f;     // demasiado separado = rojo
    public float minForward = 0.10f;  // demasiado pegado = rojo
    public float maxForward = 0.80f;  // demasiado extendido = rojo

    [Header("Rotation Rules")]
    public float maxAngleForGood = 35f;    // <35° = verde
    public float maxAngleForWarn = 75f;    // 35–75° = amarillo
                                           // >75° = rojo

    [Header("Stats")]
    public float minTimeForBadPostureEvent = 1.5f;

    private HandIdleStatsManager statsManager;
    private bool postureBad = false;
    private float badPostureTimer = 0f;

    void Start()
    {
        statsManager = HandIdleStatsManager.Instance;

        if (postureIcon != null)
            postureIcon.SetActive(false);
    }

    void Update()
    {
        PostureState left = EvaluateHand(leftHand);
        PostureState right = EvaluateHand(rightHand);

        // La peor postura entre ambas manda
        PostureState final = (PostureState)Mathf.Max((int)left, (int)right);

        UpdateWristBand(final);
        UpdateUI(final);
        RegisterBadPosture(final);
    }

    enum PostureState
    {
        GREEN = 0,
        YELLOW = 1,
        RED = 2
    }

    PostureState EvaluateHand(Transform hand)
    {
        if (hand == null || head == null)
            return PostureState.GREEN;

        // Posición relativa
        Vector3 local = head.InverseTransformPoint(hand.position);

        bool badPos =
            local.y < minHeight ||
            local.y > maxHeight ||
            Mathf.Abs(local.x) > maxSide ||
            local.z < minForward ||
            local.z > maxForward;

        // Rotación real de la mano
        float angle = Vector3.Angle(hand.up, Vector3.up);

        if (badPos || angle > maxAngleForWarn)
            return PostureState.RED;

        if (angle > maxAngleForGood)
            return PostureState.YELLOW;

        return PostureState.GREEN;
    }

    void UpdateWristBand(PostureState state)
    {
        if (wristBand == null) return;

        switch (state)
        {
            case PostureState.GREEN:
                wristBand.SetColor("GREEN");
                break;

            case PostureState.YELLOW:
                wristBand.SetColor("YELLOW");
                break;

            case PostureState.RED:
                wristBand.SetColor("RED");
                break;
        }
    }

    void UpdateUI(PostureState state)
    {
        if (postureIcon == null) return;

        postureIcon.SetActive(state == PostureState.RED);
    }

    void RegisterBadPosture(PostureState state)
    {
        if (statsManager == null) return;

        if (state == PostureState.RED)
        {
            badPostureTimer += Time.deltaTime;

            if (!postureBad && badPostureTimer >= minTimeForBadPostureEvent)
            {
                postureBad = true;
                statsManager.RegisterBadPostureEvent();
            }
        }
        else
        {
            postureBad = false;
            badPostureTimer = 0f;
        }
    }
}
