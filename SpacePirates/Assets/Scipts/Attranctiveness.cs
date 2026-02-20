using UnityEngine;

[System.Serializable]
public class Attranctiveness
{
    [HideInInspector]
    public string fontName;
    public Attractant attractant;
    public Breakable target;
    [Range(0, 10)]
    public int startAttractant;
    [HideInInspector] public int value;

    public void SetNames()
    {
        fontName = attractant.ToString();
        SetAttranct();
    }

    public void SetAttranct()
    {
        value = startAttractant;
    }
}
