using UnityEngine;

[System.Serializable]
public class BubbleTemplate
{
    public BubbleKind kind;
    public int maxHP;
    public float averageSpeed;
    public float speedVariance;
    public float averageSize;
    public float sizeVariance;
    public BubbleTemplate(BubbleKind kind, int maxHP, float averageSpeed, float speedVariance, float averageSize, float sizeVariance)
    {
        this.kind = kind;
        this.averageSpeed = averageSpeed;
        this.speedVariance = speedVariance;
        this.averageSize = averageSize;
        this.sizeVariance = sizeVariance;
        this.maxHP = maxHP;
    }
}