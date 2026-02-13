using UnityEngine;

public class RockEmeny : GridMovement
{
    public Transform targetplace;
    public bool targeting;
    [SerializeField] RoomIndex currentRoom;

    [SerializeField] float rayCastRange = 1;
    [SerializeField] LayerMask nullAvoidance;

    bool actacking = false;



    // Update is called once per frame
    void Start()
    {
        PickMove();
    }
    void Update()
    {
    
        
        //moveTowards(direction);
        /*if (UpdateMove())
        {
            targeting = false;
        }*/

        if (!actacking)
        {
            if(!UpdateMove())
            {
                PickMove();
            }
            //PickMove();
        }
        
    }

    void Actack(RaycastHit2D hit)
    {
        Debug.Log("actacking " + hit);
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
                        float varRandom = Random.Range(0,1);
                        actacking = true;
                        targeting = false;
                        if (varRandom > 0.5f)
                        {
                            Actack(Xhit);
                        }
                        else
                        {
                            Actack(Yhit);
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

                    Xhit = Physics2D.Raycast(transform.position, leftRightDirection, rayCastRange, nullAvoidance);
                    if (!HitCheck(Xhit))
                    {
                        direction = UpDownDirection;
                    }
                    else
                    {
                        float varRandom = Random.Range(0,1);
                        actacking = true;
                        targeting = false;
                        if (varRandom > 0.5f)
                        {
                            Actack(Xhit);
                            
                        }
                        else
                        {
                            Actack(Yhit);
                        }
                    }
                }
            }

            if (!actacking)
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
        for (int i = 0; i < roomIndexRef.Length; i++)
        {
            if (!foundRoom && roomIndexRef[i] != currentRoom)
            {
                foundRoom = true;
                AssignTarget(roomIndexRef[i]);
            }
        }
    }

    public void AssignTarget(RoomIndex newRoom)
    {
        currentRoom = newRoom;
        Breakable targetBreakable = currentRoom.CheckBreakableLists(Attractant.energy, transform.position);
        
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
