using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public Image _image;
    public Sprite _default, _pressed;
    public AudioClip _compressClip, _uncompressClip;
    public AudioSource _source;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void PlayGame() {
    SceneManager.LoadSceneAsync("Game");
   }

   public void QuitGame() {
    Application.Quit();
   }

   public void OnPointerDown(PointerEventData eventData){
    _image.sprite = _pressed;
    _source.PlayOneShot(_compressClip);
    
   }

   public void OnPointerUp(PointerEventData eventData){
    _image.sprite = _default;
    _source.PlayOneShot(_uncompressClip);
   }
}
