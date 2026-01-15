using UnityEngine;
public class Breakable : MonoBehaviour
{

    [SerializeField] float health = 1;
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
}
