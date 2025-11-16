using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class ClickableVideo : MonoBehaviour, IPointerClickHandler {
  public VideoPlayer player;     // arrastra el VideoPlayer (WallScreen)
  public AudioSource audioSrc;   // arrastra el AudioSource (WallScreen)

  void Start(){
    if (player){
      player.playOnAwake = false;
      player.Pause(); // empezamos pausado
    }
  }

  public void OnPointerClick(PointerEventData e){
    if (!player) return;
    if (player.isPlaying){
      player.Pause();
    } else {
      if (audioSrc && !audioSrc.enabled) audioSrc.enabled = true;
      player.Play();
      Debug.Log("CLICK en pantalla");
    }
  }
}
