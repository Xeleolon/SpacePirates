using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoomData
{
    public string name;
    [Tooltip("number of damage systems")]
    public int systems = 1;
    private int damagesytems = 0;
    public Image damageicon;

    public void DamageSystem(bool damage)
    {
        if (damage)
        {
            damagesytems += 1;
            if (damagesytems > systems)
            {
                Debug.Log("Warning attemping to damage" + name + " more than " + systems + " ever add more systems or check if something is breaking more than once");
                damagesytems = systems;
            }
        }
        else
        {
            damagesytems -=1;
            if (damagesytems < 0)
            {
                Debug.Log("Warning attemping to reapir " + name + " more than " + systems + " ever add more systems or check if something is reapairng more than once");
                damagesytems = systems;
            }
        }
    }
    

}