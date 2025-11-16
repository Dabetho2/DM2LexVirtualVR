using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(SphereCollider))]

public class VideoClick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]private VideoPlayer video;
     [SerializeField]private Renderer screenRender;
  [SerializeField] private Material screenBlack;
    private Material originalMaterial;
    
    [SerializeField] private float triggerRadius = 0.25f;

    private bool playerIsInRange;

    

 private void Awake ()
 {
    originalMaterial = screenRender.material;
    SphereCollider sphere = GetComponent<SphereCollider>();
    sphere.isTrigger= true;
    sphere.radius = triggerRadius;

 }

 private void OnEnable ()
 {
    video.loopPointReached += OnVideoFinished; 
 }

 private void OnDisable()
 {
    video.loopPointReached -= OnVideoFinished; 
 }

 private void Start()
 {
    screenRender.material = screenBlack;
 }

 private void OnTriggerEnter (Collider other)
 {
    if (other.CompareTag("Player"))
    {
        Debug.Log("Hola");
        playerIsInRange = true;
        if (!video.isPlaying)
        {
            PlayVideo();
        }
    }
 }

private void OnTriggerExit (Collider other)
{
    if (!other.CompareTag("Player")) return;

    playerIsInRange = false;

    // Detén o pausa el video al salir
    if (video.isPlaying) video.Pause();   // o video.Stop() si quieres reiniciar tiempo

    // Vuelve a mostrar la pantalla en negro
    screenRender.material = screenBlack;
}

 private void OnVideoFinished(VideoPlayer vp)
 {
    screenRender.material = screenBlack;
    if (playerIsInRange)
    {
        PlayVideo();
    }
 }
 
 private void PlayVideo()
 {
    screenRender.material = originalMaterial;
    video.Play();
 }

}
