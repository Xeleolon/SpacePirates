using UnityEngine;

public class SlimeEmeny : EmenyBase
{
    [SerializeField] float moveDistance = 1;
    [SerializeField] float movePause = 1;
    [SerializeField] float playerDamage = 1;
    [SerializeField] float breakableDamage = 1;
    float movementHoldClock = 0;
    [SerializeField] Rigidbody2D rb;

    bool damage;
    private void Update()
    {
        if (movementHoldClock <= 0)
        {
            randomMovment();
            movementHoldClock = movePause;
            damage = true;
        }
        else
        {
            movementHoldClock -= 1 * Time.deltaTime;
        }

        void randomMovment()
        {
            Vector2 direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            direction = Vector2.ClampMagnitude(direction, moveDistance);
            rb.MovePosition(rb.position + direction);
        }
    }

    void OnCollisionEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //damage Player;
            Player temp = other.gameObject.GetComponent<Player>();
            if (temp != null)
            {
                temp.AlterHealth(-playerDamage);
                damage = false;
            }
            else
            {
                Debug.LogError("Breakable object " + other.gameObject.name + " does not have a Breakable Script acttached");
            }
        }
        else if (other.gameObject.tag == "Breakable")
        {
            Breakable temp = other.gameObject.GetComponent<Breakable>();
            if (temp != null)
            {
                temp.AlterHealth(-breakableDamage);
                damage = false;
            }
            else
            {
                Debug.LogError("Breakable object " + other.gameObject.name + " does not have a Breakable Script acttached");
            }
        }
    }
}
