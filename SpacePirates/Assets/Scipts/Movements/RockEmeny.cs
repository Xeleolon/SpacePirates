using UnityEngine;

public class RockEmeny : GridMovement
{
    
    public bool targeting;
    [SerializeField] RoomIndex currentRoom;
    [SerializeField] Attractant attractedTo = Attractant.energy;

    [SerializeField] float rayCastRange = 1;

    [Header("Actacking")]
    [SerializeField] int damage = 1;
    [SerializeField] float actackRayRange = 1;

    [SerializeField] float actackFrequence = 1;
    private float actackClock;
    //private float storagedEnergy;
 
    ActackType actacking = ActackType.none;



    // Update is called once per frame
    public override void SpawnObject(RoomIndex startRoom)
    {
        currentRoom = startRoom;
        AssignTarget(currentRoom);
        PickMove();
    }
    public override void Start()
    {
        base.Start();
        LevelManager.instance.updateTargets += UpdateTarget;
        //AssignTarget(currentRoom);
        //PickMove();
    }
    private void OnDestroy()
    {
        LevelManager.instance.updateTargets -= UpdateTarget;
        Debug.Log("removing Subcrition");
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
                
                break;
        }
        
        if (actackClock > 0)
        {
            actackClock -= 1 * Time.deltaTime;
        }


    }
    #region strikeActack
    void StrikeActack(Vector2 upDown, Transform upDownHit, Vector2 leftRight, Transform leftRightHit) //called if movement can't be performed in planed direction so actack instead
    {
        if (actacking == ActackType.none)
        {
            actacking = ActackType.strike;
        }
        Debug.Log("StrikeActack");
        if (actacking == ActackType.none)
        {
            actacking = ActackType.strike;
        }

        int xPoints = CheckGreater(leftRightHit);
        int yPoints = CheckGreater(upDownHit);

        int CheckGreater(Transform hit)
        {
            if (hit == null)
            {
                return 0;
            }
            else if (hit.gameObject.tag == "Player")
            {
                return 2;
            }
            else if (hit.gameObject.tag == "Breakable")
            {
                return 1;
            }

            return 0;
        }

        if (xPoints == yPoints)
        {
            if (Random.Range(0, 1) > 0.5f)
            {
                InputToDirection(upDown);
            }
            else
            {
                InputToDirection(leftRight);
            }
        }
        else if (xPoints > yPoints)
        {
            InputToDirection(leftRight);
        }
        else
        {
            InputToDirection(upDown);
        }


        Actack();
    }

    #endregion
    #region Actack
    void Actack()
    {
        if (actackClock > 0)
        {
            return;
        }
        PlayStrikeAnimations();
        Vector2 temp = RayDirection();
        RaycastHit2D actackHit = Physics2D.Raycast(transform.position, temp, rayCastRange, nullAvoidance);
        Debug.Log(gameObject.name + " has hit " + actackHit.transform + " actack mode = " + actacking + " direction of actack " + temp);
        if (actackHit.transform == null)
        {
            Debug.Log(gameObject.name + " has hit nothign");
        }
        else if (actackHit.transform.gameObject.tag == "Player")
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
                Actack();
            }
        }
        return false;
    }
    #endregion
    #region Movement

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
                    //StrikeActack(UpDownDirection, Yhit.transform, leftRightDirection, Xhit.transform);

                    //do nothing
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
    #endregion



    #region TargetAssignment&Update

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
    private void UpdateTarget()
    {
        AssignTarget(currentRoom);
    }
    public void AssignTarget(RoomIndex newRoom)
    {
        currentRoom = newRoom;
        if (newRoom == null)
        {
            Debug.LogWarning("No Room Available for " + gameObject.name);
            return;
        }
        Breakable targetBreakable = currentRoom.CheckBreakableLists(attractedTo, transform.position, false);
        
        if (targetBreakable != null)
        {
            if (actacking == ActackType.contiuneTillDead)
            {
                actacking = ActackType.none;
            }
            targetplace = targetBreakable.transform;
        }
        else
        {
            Debug.Log("No attracnct part of door");
        }
    }

    #endregion

    float NegavtiveCheck(float negCheck)
    {
        if (negCheck < 0)
        {
            negCheck = negCheck * -1;
        }
        return negCheck;
    }
}
