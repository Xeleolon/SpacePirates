using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoomData
{
    [HideInInspector]
    public string fontName;
    public RoomIndex roomIndex;
    private int systemsHealth = 1;
    private int damageSystems = 0;
    public Image damageIcon;

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
    public void SetSystems(int health)
    {
        systemsHealth = health;
        damageSystems = systemsHealth;
    }
    public bool DamageSystem(int damage)
    {
        damageSystems += damage;

        if (damageSystems <= 0 )
        {
            damageSystems = 0;
            return false;
        }
        else if (damageSystems > systemsHealth)
        {
            damageSystems = systemsHealth;
        }

        return true;
    }
    

}