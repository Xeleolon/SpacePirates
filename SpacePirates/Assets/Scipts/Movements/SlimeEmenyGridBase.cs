using UnityEngine;

public class SlimeEmenyGridBase : GridMovement
{
    [SerializeField] float movePause = 1;
    float movementHoldClock = 0;
    [SerializeField] float rayCastRange = 1;
    [SerializeField] LayerMask nullAvoidance;

    [Header("Damage")]
    [SerializeField] int playerDamage = 1;
    [SerializeField] int breakableDamage = 1;

    public bool stepMovement = false;

    bool damage;

    void Start()
    {
    }
    int[] availableDirection = new int[0];
    RaycastHit2D[] hit = new RaycastHit2D[4];
    private void Update()
    {
        if (!UpdateMove() /*&& stepMovement*/)
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
                if (!HitCheck(hit[i]))
                {
                    //Debug.Log("Hit " + direction + " had not colision with an object");
                    int[] tempDirections = new int[availableDirection.Length + 1];
                    //Debug.Log("hit " + i + "temp direction slots " + tempDirections.Length);

                    if (availableDirection.Length >= 1)
                    {
                        for (int var = 0; var < availableDirection.Length; var ++)
                        {
                            tempDirections[var] = availableDirection[var];
                            //Debug.Log(i + " cycle " tempDirections[var] + " temp var/availbale var" + availableDirection[var]);
                        }
                    }

                    int placement = availableDirection.Length;
                    if (placement < 0)
                    {
                        placement = 0;
                    }

                    //Debug.Log("hit " + i + "temp direction slots " + tempDirections.Length + " placement " + placement);
                    tempDirections[placement] = i + 1;
                    //Debug.Log(i + " placment has varible = " + tempDirections[placement]);

                    availableDirection = tempDirections;

                    //Debug.Log("Hit " + i + " has produced " + availableDirection.Length);
                } 
                else
                {
                    //Debug.Log("Hit " + direction + " has collided");
                }
            }

            if (availableDirection.Length > 0)
            {
                int randomDirection = availableDirection[Random.Range(0, availableDirection.Length - 1)];
                //Debug.Log("random picked " + randomDirection + "out of " + availableDirection.Length);
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
                //Debug.Log("number of hits = " + hit.Length + " + direction going in ="+ randomDirection);
                if (hit[randomDirection - 1])
                {
                    Actack(hit[randomDirection - 1].transform.gameObject);
                    //Debug.Log("actacking " + hit[randomDirection - 1].transform.gameObject.name);
                }
                else
                {
                    moveTowards(direction);
                    //Debug.Log("Moving towards " + direction);
                }

                stepMovement = false;
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
