using UnityEngine;

public class SlimeEmenyGridBase : GridMovement
{
    [SerializeField] float movePause = 1;
    float movementHoldClock = 0;

    [Header("Damage")]
    [SerializeField] int playerDamage = 1;
    [SerializeField] int breakableDamage = 1;
    [SerializeField] float rayCastRange = 1;
    [SerializeField] LayerMask nullAvoidance;
    [SerializeField] string wallObject = "wall";

    bool damage;

    void Start()
    {
    }
    int[] availableDirection = new int[0];
    RaycastHit2D[] hit = new RaycastHit2D[4];
    private void Update()
    {
        if (!UpdateMove())
        {
            RandomMovment();
        }
        
        void RandomMovment()
        {

            //int[] availableDirection = new int[0];
            //RaycastHit2D[] hit = new RaycastHit2D[4];
            Vector2 direction = Vector2.zero;

            for (int i = 0; i < hit.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        direction = Vector2.up;
                        break;
                    case 1:
                        direction = Vector2.right;
                        break;
                    case 2:
                        direction = Vector2.down;
                        break;
                    case 3:
                        direction = Vector2.left;
                        break;

                }
                hit[i] = Physics2D.Raycast(transform.position, direction, rayCastRange, nullAvoidance);
                if ((hit[i] && hit[i].transform.tag != wallObject) || !hit[i])
                {
                    int[] tempDirections = new int[availableDirection.Length + 1];
                    if (availableDirection.Length > 1)
                    {
                        tempDirections = availableDirection;
                    }
                    int placement = availableDirection.Length - 1;
                    if (placement < 0)
                    {
                        placement = 0;
                    }
                    
                    tempDirections[placement] = i + 1;
                    availableDirection = tempDirections;
                }
            }

            if (availableDirection.Length > 0)
            {
                int randomDirection = availableDirection[Random.Range(0, availableDirection.Length - 1)];

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
                Debug.Log(hit.Length + " "+ randomDirection);
                if (hit[randomDirection - 1 ])
                {
                    Actack(hit[randomDirection - 1].transform.gameObject);
                }
                else
                {
                    moveTowards(direction);
                }
            }

            
        }
    }

    void Actack(GameObject actackObject)
    {
        if (actackObject.tag == "Player")
        {
            //damage Player;
            Player temp = actackObject.GetComponent<Player>();
            if (temp != null)
            {
                temp.AlterHealth(-playerDamage);
                damage = false;
            }
            else
            {
                Debug.LogError("Breakable object " + actackObject.name + " does not have a Breakable Script acttached");
            }
        }
        else if (actackObject.tag == "Breakable")
        {
            Breakable temp = actackObject.GetComponent<Breakable>();
            if (temp != null)
            {
                temp.AlterHealth(-breakableDamage);
                damage = false;
            }
            else
            {
                Debug.LogError("Breakable object " + actackObject.name + " does not have a Breakable Script acttached");
            }
        }
    }

    /*void OnCollisionEnter2D(Collision2D other)
    {
        Actack(other.gameObject);
    }*/
}
