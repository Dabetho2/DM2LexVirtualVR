using UnityEngine;

public class HandMotionSensor : MonoBehaviour
{
    [Header("Hand Reference")]
    public Transform hand;

    [Header("Sensitivity Settings")]
    public float stillSpeedThreshold = 0.01f;       // bajo = quieto
    public float movingSpeedThreshold = 0.05f;      // detectable como movimiento suave
    public float shakeAccelThreshold = 0.35f;       // aceleración fuerte de la mano
    public float shakeRotationThreshold = 10f;      // rotación brusca de mano
    public float fastMovementThreshold = 1.0f;      // movimiento MUY rápido (excluir de shaking)

    [Header("Internal State (Read Only)")]
    public float currentSpeed;
    public float currentAcceleration;
    public float rotationDelta;

    public bool isMoving;
    public bool isShaking;
    public bool isTooStill;
    public bool isMovingTooFast;

    private Vector3 lastLocalPos;
    private Quaternion lastLocalRot;
    private float lastSpeed;

    void Start()
    {
        if (hand == null)
        {
            Debug.LogWarning("[HandMotionSensor] No se asignó la mano.");
            enabled = false;
            return;
        }

        // Guardamos la posición y rotación LOCAL respecto al XR Origin
        lastLocalPos = hand.localPosition;
        lastLocalRot = hand.localRotation;
    }

    void Update()
    {
        // 1. Tomamos posición y rotación LOCALES
        Vector3 localPos = hand.localPosition;
        Quaternion localRot = hand.localRotation;

        float dt = Time.deltaTime;
        if (dt <= 0f) return;

        // -------------------------------
        // 1. Velocidad LOCAL de la mano
        // -------------------------------
        float distance = Vector3.Distance(localPos, lastLocalPos);
        currentSpeed = distance / dt;

        // -------------------------------
        // 2. Aceleración LOCAL
        // -------------------------------
        currentAcceleration = (currentSpeed - lastSpeed) / dt;

        // -------------------------------
        // 3. Rotación LOCAL
        // -------------------------------
        rotationDelta = Quaternion.Angle(localRot, lastLocalRot);

        // -------------------------------
        // 4. Estados
        // -------------------------------
        isTooStill       = currentSpeed < stillSpeedThreshold;
        isMoving         = currentSpeed > movingSpeedThreshold;
        isMovingTooFast  = currentSpeed > fastMovementThreshold;

        // Temblor real: aceleración local O rotación local brusca
        bool accelShake    = Mathf.Abs(currentAcceleration) > shakeAccelThreshold;
        bool rotationShake = rotationDelta > shakeRotationThreshold;

        // isShaking se activa SOLO si NO es movimiento brusco y SI hay jitter
        isShaking = (accelShake || rotationShake) && !isMovingTooFast;

        // DEBUG opcional
        // Debug.Log($"[{hand.name}] Speed:{currentSpeed:F3} Accel:{currentAcceleration:F3} RotΔ:{rotationDelta:F1} Shake:{isShaking}");

        // Guardamos últimas posiciones locales
        lastLocalPos = localPos;
        lastLocalRot = localRot;
        lastSpeed = currentSpeed;
    }
}
