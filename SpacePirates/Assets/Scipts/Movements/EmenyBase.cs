using UnityEngine;

public class EmenyBase : MonoBehaviour
{
    [SerializeField] float health = 1;

    [Header("DamageSpriteRenderer")]
    private Color orignalcolor;
    [SerializeField] Color damageColor;
    private SpriteRenderer spriteRenderer;
    [SerializeField] float damageFlashLength;
    private float damageClock;
    private bool damaging;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        orignalcolor = spriteRenderer.color;
    }
    private void Update()
    {
        if (damaging)
        {
            if (damageClock <= 0)
            {
                damageClock = 0;
                damaging = false;
                spriteRenderer.color = orignalcolor;
            }
            else
            {
                damageClock -= 1 * Time.deltaTime;
            }
        }
    }


    public void AlterHealth(float alter)
    {
        health += alter;
        spriteRenderer.color = damageColor;
        damageClock = damageFlashLength;
        damaging = true;


        if (health <= 0)
        {
            EmenyDied();
        }
    }

    public void EmenyDied()
    {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }
}
