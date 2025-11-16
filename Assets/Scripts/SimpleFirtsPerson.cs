using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
public class SimpleFPS : MonoBehaviour {
  public float speed = 3.5f, lookSensitivity = 300f;
  public Transform cam;
  CharacterController cc; float pitch, yVel;

  void Awake(){
    cc = GetComponent<CharacterController>();
    if(!cam) cam = GetComponentInChildren<Camera>()?.transform;
    Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
  }

  void Update(){

      if (Cursor.lockState != CursorLockMode.Locked) return;
    // Mirar con mouse (funciona con Input System o el antiguo)
    float mx, my;
#if ENABLE_INPUT_SYSTEM
    Vector2 d = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
    mx = d.x * lookSensitivity * Time.deltaTime;
    my = d.y * lookSensitivity * Time.deltaTime;
#else
    mx = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
    my = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
#endif
    transform.Rotate(0f, mx, 0f);
    pitch = Mathf.Clamp(pitch - my, -85f, 85f);
    if (cam) cam.localEulerAngles = new Vector3(pitch, 0, 0);

    // Movimiento WASD
    float x, z;
#if ENABLE_INPUT_SYSTEM
    x = (Keyboard.current?.dKey.isPressed == true ? 1 : 0) - (Keyboard.current?.aKey.isPressed == true ? 1 : 0);
    z = (Keyboard.current?.wKey.isPressed == true ? 1 : 0) - (Keyboard.current?.sKey.isPressed == true ? 1 : 0);
#else
    x = Input.GetAxisRaw("Horizontal");
    z = Input.GetAxisRaw("Vertical");
#endif
    Vector3 move = (transform.right * x + transform.forward * z).normalized * speed;

    // Gravedad
    if (cc.isGrounded && yVel < 0) yVel = -2f;
    yVel += Physics.gravity.y * Time.deltaTime;
    move += Vector3.up * yVel;

    cc.Move(move * Time.deltaTime);
  }
  
}
