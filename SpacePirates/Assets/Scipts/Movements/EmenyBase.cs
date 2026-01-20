using UnityEngine;

public class EmenyBase : MonoBehaviour
{
    [SerializeField] float health = 1;


    public void AlterHealth(float alter)
    {
        health += alter;

        if (health <= 0)
        {

        }
    }

    public void EmenyDied()
    {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }
}
