using UnityEngine;

public class bubble : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
        
    }

           void OnMouseDown(){
    Debug.Log("Sprite Clicked");
    Destroy(this.gameObject);
}
}
