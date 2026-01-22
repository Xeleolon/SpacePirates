using UnityEngine;

public class SlimeEmeny : BaseMovement
{
    [SerializeField] float movePause = 1;
    float movementHoldClock = 0;

    [Header("Damage")]
    [SerializeField] int playerDamage = 1;
    [SerializeField] int breakableDamage = 1;

    bool damage;

    public override void Start()
    {
        base.Start();
        useTime = false;
    }
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
            Vector2 direction = Vector2.zero;
            int randomDirection = Random.Range(0, 4);
            switch (randomDirection)
            {
                case 1:
                    direction = Vector2.up;
                    break;
                case 2:
                    direction = Vector2.right;
                    break;
                case 3:
                    direction = Vector2.down;
                    break;
                case 4:
                    direction = Vector2.left;
                    break;

            }

            UpdateMove(direction);


        }
    }

    void OnCollisionEnter2D(Collision2D other)
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
