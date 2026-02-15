using UnityEngine;

public class RockEmeny : GridMovement
{
    public Transform targetplace;
    public bool targeting;
    [SerializeField] RoomIndex currentRoom;

    [SerializeField] float rayCastRange = 1;

    [Header("Actacking")]
    [SerializeField] int damage = 1;
    [SerializeField] float actackRayRange = 1;

    [SerializeField] float actackFrequence = 1;
    private float actackClock;
    //private float storagedEnergy;
 
    ActackType actacking = ActackType.none;



    // Update is called once per frame
    void Start()
    {
        AssignTarget(currentRoom);
        PickMove();
    }
    void Update()
    {

        
        switch (actacking)
        {
            case ActackType.none:
                if (!UpdateMove())
                {
                    /*
                    if (targeting)
                    {
                        PickMove();
                    }
                    */
                    PickMove();
                }
                break;
            case ActackType.huntPlayer:
                if (!UpdateMove())
                {
                    PickMove();
                }
                Actack();
                break;
            case ActackType.contiuneTillDead:
                Actack();
                break;
            case ActackType.strike:
                Actack();
                break;
        }
        
        if (actackClock > 0)
        {
            actackClock -= 1 * Time.deltaTime;
        }


    }

    void Actack()
    {
        if (actackClock > 0)
        {
            return;
        }

        RaycastHit2D actackHit = Physics2D.Raycast(transform.position, RayDirection(), rayCastRange, nullAvoidance);

        if (actackHit.transform.gameObject.tag == "Player")
        {
            Player player = actackHit.transform.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.AlterHealth(-damage);
                ActackSuccess();
            }
        }
        else if (actackHit.transform.gameObject.tag == "Breakable")
        {
            Breakable breakable = actackHit.transform.gameObject.GetComponent<Breakable>();
            if (breakable != null)
            {
                breakable.AlterHealth(-damage);
                ActackSuccess();
            }
        }

        void ActackSuccess()
        {
            if (actacking == ActackType.strike)
            {
                actacking = ActackType.none;
            }
            Debug.Log("Actacking " + actackHit.transform.gameObject.name);

            actackClock = actackFrequence;
        }

        Vector2 RayDirection()
        {
            switch (inputDirection)
            {
                case MovementDirection.up:
                    return Vector2.up;
                case MovementDirection.down:
                    return Vector2.down;
                case MovementDirection.right:
                    return Vector2.right;
                case MovementDirection.left:
                    return Vector2.left;
            }
            return Vector2.zero;
        }
    }
    public override bool CallActack(GameObject collisionObject)
    {
        if (collisionObject.tag == "Player")
        {
            actacking = ActackType.huntPlayer;
            return false;
        }
        Breakable targetBreakable = collisionObject.GetComponent<Breakable>();
        if (targetBreakable != null)
        {
            if (collisionObject.transform == targetplace)
            {
                actacking = ActackType.contiuneTillDead;
                return true;
            }
            else
            {
                actacking = ActackType.strike;
            }
        }
        return false;
    }

    void PickMove()
    {
        Vector2 direction = Vector2.zero;
        if (targetplace == null)
        {
            targeting = false;
            return;
        }
        float Xdifferntial = NegavtiveCheck(targetplace.position.x - transform.position.x);
        float Ydifferntial = NegavtiveCheck(targetplace.position.y - transform.position.y);

        RaycastHit2D Xhit;
        RaycastHit2D Yhit;
        Vector2 UpDownDirection = Vector2.zero;
        Vector2 leftRightDirection = Vector2.zero;

        if (Xdifferntial >= Ydifferntial)
        {

            leftRightDirection = LeftRight();

            Xhit = Physics2D.Raycast(transform.position, leftRightDirection, rayCastRange, nullAvoidance);
            if (!HitCheck(Xhit))
            {
                direction = leftRightDirection;
            }
            else
            {

                UpDownDirection = UpDown();
                Yhit = Physics2D.Raycast(transform.position, UpDownDirection, rayCastRange, nullAvoidance);
                if (!HitCheck(Yhit))
                {
                    direction = UpDownDirection;
                }
                else
                {
                    float varRandom = Random.Range(0, 1);

                    if (actacking == ActackType.none)
                    {
                        actacking = ActackType.strike;
                    }

                    targeting = false;
                    if (varRandom > 0.5f)
                    {
                        Actack();
                    }
                    else
                    {
                        Actack();
                    }
                }
            }
        }
        else if (Xdifferntial < Ydifferntial)
        {
            //Debug.Log("moving up or down");


            UpDownDirection = UpDown();
            Yhit = Physics2D.Raycast(transform.position, UpDownDirection, rayCastRange, nullAvoidance);
            if (!HitCheck(Yhit))
            {
                //Debug.Log("Moving up/down and providing direction " + UpDownDirection);
                direction = UpDownDirection;
            }
            else
            {
                leftRightDirection = LeftRight();
                //Debug.Log("Left and Right and providing direction " + leftRightDirection);

                Xhit = Physics2D.Raycast(transform.position, leftRightDirection, rayCastRange, nullAvoidance);
                if (!HitCheck(Xhit))
                {
                    direction = leftRightDirection;
                }
                else
                {
                    float varRandom = Random.Range(0, 1);

                    if (actacking == ActackType.none)
                    {
                        actacking = ActackType.strike;
                    }

                    targeting = false;
                    if (varRandom > 0.5f)
                    {
                        Actack();

                    }
                    else
                    {
                        Actack();
                    }
                }
            }
        }

        if (actacking == ActackType.none)
        {
            targeting = false;
            moveTowards(direction);
        }


        Vector2 UpDown()
        {
            if (targetplace.position.y == transform.position.y)
            {
                return Vector2.zero;
            }
            else if (targetplace.position.y > transform.position.y)
            {
                return Vector2.up;
            }
            return Vector2.down;
        }

        Vector2 LeftRight()
        {
            if (targetplace.position.x == transform.position.x)
            {
                return Vector2.zero;
            }
            else if (targetplace.position.x > transform.position.x)
            {
                return Vector2.right;
            }
            return Vector2.left;
        }


    }


        float NegavtiveCheck(float negCheck)
        {
            if (negCheck < 0)
            {
                negCheck = negCheck * -1;
            }
            return negCheck;
        }

    public override void TargetUpdate(RoomIndex[] roomIndexRef)
    {
        bool foundRoom = false;
        //Debug.Log("enter new room");
        for (int i = 0; i < roomIndexRef.Length; i++)
        {
            //Debug.Log("room Check" + i);
            if (!foundRoom && roomIndexRef[i] != currentRoom)
            {
                foundRoom = true;
                //Debug.Log("found room" + i);
                AssignTarget(roomIndexRef[i]);
            }
        }
    }

    public void AssignTarget(RoomIndex newRoom)
    {
        currentRoom = newRoom;
        Breakable targetBreakable = currentRoom.CheckBreakableLists(Attractant.energy, transform.position, false);
        
        if (targetBreakable != null)
        {
            targetplace = targetBreakable.transform;
        }
        else
        {
            Debug.Log("No attracnct part of door");
        }
    }
}
