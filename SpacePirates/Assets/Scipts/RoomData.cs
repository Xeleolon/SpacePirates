using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoomData
{
    public string name;
    [Tooltip("number of damage systems")]
    public int systems = 1;
    private int damagesystems = 0;
    public Image damageIcon;

    public void DamageSystem(bool damage)
    {
        if (damage)
        {
            damagesystems += 1;
            if (damagesystems > systems)
            {
                Debug.Log("Warning attemping to damage" + name + " more than " + systems + " ever add more systems or check if something is breaking more than once");
                damagesystems = systems;
            }
        }
        else
        {
            damagesystems -= 1;
            if (damagesystems < 0)
            {
                Debug.Log("Warning attemping to reapir " + name + " more than " + systems + " ever add more systems or check if something is reapairng more than once");
                damagesystems = systems;
            }
        }
    }
    

}