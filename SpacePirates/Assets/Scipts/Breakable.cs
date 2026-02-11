using UnityEngine;
public class Breakable : MonoBehaviour
{

    [SerializeField] float health = 1;
    public Attractant[] attractant;
    [Range(0,10)]
    public int energyActractive = 0;
    float currentHeath;
    //Sprite[] sprite;

    bool broken = false;
    bool damage = false;

    void Start()
    {
        currentHeath = health;
    }
    
    public void AlterHealth(float alter)
    {
        currentHeath += alter;

        if (currentHeath >= health)
        {
            currentHeath = health;
            damage = false;
            broken = false;
        }
        else if (currentHeath < health)
        {
            damage = true;
            if (currentHeath <= 0)
            {
                currentHeath = 0;
                broken = true;
            }
            else
            {
                broken = false;
            }
        }
    }

    public bool CheckAttractanctMatch(Attractant matchCheck)
    {
        if (attractant.Length <= 0)
        {
            return false;
        }

        for (int i = 0; i < attractant.Length; i++)
        {
            if (attractant[i] == matchCheck)
            {
                return true;
            }
        }
        return false;
    }

    public int GiveValue(Attractant type)
    {
        switch (type)
        {
            case Attractant.energy:
                return energyActractive;

            default:
                Debug.LogWarning(type + " has no assigned target data setup in DoorWay script");
                return 0;
        }
    }
}
