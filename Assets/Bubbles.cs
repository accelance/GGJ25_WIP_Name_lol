using UnityEngine;
using System.Collections;


public class Bubbles : MonoBehaviour
{
    void OnMouseDown()
    {
        transform.parent.gameObject.GetComponent<BubbleSpawner>().onHit(transform.GetSiblingIndex());
        if (Clicker.Instance.waffe == Clicker.Waffe.Normal)
        {
        }
    }

   






}
