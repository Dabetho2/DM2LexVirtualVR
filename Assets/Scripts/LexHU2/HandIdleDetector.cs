using UnityEngine;

public class HandIdleDetector : MonoBehaviour
{
    [Header("Referencias a las manos")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("UI de advertencia")]
    public GameObject handIdleIcon;
    public AudioSource popSound;

    [Header("Sensibilidad")]
    public float movementThreshold = 0.02f;
    public float rotationThreshold = 5f;

    [Header("Tiempo para avisar")]
    public float stillTimeToWarn = 5f;

    [Header("Métrica de comportamiento")]
    [Tooltip("Cantidad de veces que el usuario dejó de mover las manos y se activó la alerta")]
    public int idleEventsCount = 0;

    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;
    private Quaternion lastLeftRot;
    private Quaternion lastRightRot;

    private float stillTimer = 0f;
    private bool alreadyWarned = false;

    // NUEVO: referencia al StatsManager (solo para contar eventos)
    private HandIdleStatsManager statsManager;

    void Start()
    {
        statsManager = FindObjectOfType<HandIdleStatsManager>();

        if (leftHand != null)
        {
            lastLeftPos = leftHand.position;
            lastLeftRot = leftHand.rotation;
        }

        if (rightHand != null)
        {
            lastRightPos = rightHand.position;
            lastRightRot = rightHand.rotation;
        }

        if (handIdleIcon != null)
            handIdleIcon.SetActive(false);
    }

    void Update()
    {
        if (leftHand == null || rightHand == null)
            return;

        float leftPosDelta  = Vector3.Distance(leftHand.position,  lastLeftPos);
        float rightPosDelta = Vector3.Distance(rightHand.position, lastRightPos);

        float leftRotDelta  = Quaternion.Angle(leftHand.rotation,  lastLeftRot);
        float rightRotDelta = Quaternion.Angle(rightHand.rotation, lastRightRot);

        bool leftMoved  = leftPosDelta  > movementThreshold || leftRotDelta  > rotationThreshold;
        bool rightMoved = rightPosDelta > movementThreshold || rightRotDelta > rotationThreshold;

        bool handsAreMoving = leftMoved || rightMoved;

        if (handsAreMoving)
        {
            stillTimer = 0f;

            if (alreadyWarned)
            {
                if (handIdleIcon != null)
                    handIdleIcon.SetActive(false);

                alreadyWarned = false;
            }
        }
        else
        {
            stillTimer += Time.deltaTime;

            if (stillTimer >= stillTimeToWarn && !alreadyWarned)
            {
                // Evento local
                idleEventsCount++;
                Debug.Log($"[Métrica] Evento de manos quietas detectado. Total local: {idleEventsCount}");

                // Evento global (Stats)
                if (statsManager != null)
                    statsManager.RegisterIdleEvent();

                // Mostrar icono
                if (handIdleIcon != null)
                    handIdleIcon.SetActive(true);

                // Sonido
                if (popSound != null)
                    popSound.Play();

                // Evitar duplicados hasta que vuelva a mover las manos
                alreadyWarned = true;
            }
        }

        lastLeftPos  = leftHand.position;
        lastRightPos = rightHand.position;
        lastLeftRot  = leftHand.rotation;
        lastRightRot = rightHand.rotation;
    }
}
