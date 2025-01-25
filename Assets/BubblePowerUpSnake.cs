using UnityEngine;

public class BubblePowerUp : Bubbles
{
    public Clicker.Waffe waffe;
    public override void onHit() {
        idle = false;
        StartCoroutine(popBubble());
        Clicker.Instance.getUpgradeAvailable(waffe);
    }


}
