using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoomData
{
    [HideInInspector]
    public string fontName;
    public RoomIndex roomIndex;
    private int systemsHealth;
    private int damageSystems;
    public Image damageIcon;
    #region systemname

    private bool nameSet;
    public bool setName
    {
        get
        {
            if (roomIndex != null)
            {
                fontName = roomIndex.name;
            }
            return nameSet;
        }
        set
        {
            if (roomIndex != null)
            {
                fontName = roomIndex.name;
            }
            nameSet = value;
        }
    }

    public void SetNames()
    {
        if (roomIndex != null)
        {
            fontName = roomIndex.name;
        }
    }
    #endregion

    public void SetSystems(int health)
    {
        systemsHealth = health;
        damageSystems = systemsHealth;
    }
    public float DamageSystem(int damage)
    {
        damageSystems += damage;

        if (damageSystems <= 0 )
        {
            damageSystems = 0;
            
        }
        else if (damageSystems > systemsHealth)
        {
            damageSystems = systemsHealth;
        }
        float percent = damageSystems;
        float Whole = systemsHealth;
        return percent / Whole;
    }
    

}