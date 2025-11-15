using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CursorToggle : MonoBehaviour {
  void Update(){
    bool pressed =
    #if ENABLE_INPUT_SYSTEM
      Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame;
    #else
      Input.GetKeyDown(KeyCode.Tab);
    #endif

    if (pressed){
      if (Cursor.lockState == CursorLockMode.Locked){
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
      } else {
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
      }
    }
  }
}
