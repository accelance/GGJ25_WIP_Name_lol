using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown() {
        if(Clicker.Instance.Waffe == Waffe.Normal) {
            onHit();
        }
    }

    public void onHit() {
        Debug.Log("bubble was hit");
        HP -= 1;
        if(HP <= 0) {
            this.gameObject.SetActive(false);
        }
    }
}
