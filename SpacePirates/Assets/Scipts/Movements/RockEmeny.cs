using UnityEngine;

public class RockEmeny : GridMovement
{
    public Transform targetplace;
    public bool targeting;

    [SerializeField] float rayCastRange = 1;
    [SerializeField] LayerMask nullAvoidance;
    [SerializeField] string wall;



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

        if (!UpdateMove())
        {
            PickMove();
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
            bool Actacking = false;

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
                        Actacking = true;
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
                
                
                
                UpDownDirection = UpDown();
                Yhit = Physics2D.Raycast(transform.position, UpDownDirection, rayCastRange, nullAvoidance);
                if (!HitCheck(Yhit))
                {
                    direction = leftRightDirection;
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
                        Actacking = true;
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
            if (!Actacking)
            {
                targeting = false;
                moveTowards(direction);
            }


            Vector2 UpDown()
            {
                if (targetplace.position.x == transform.position.x)
                {
                    return Vector2.zero;
                }
                else if (targetplace.position.x > transform.position.x)
                {
                    return Vector2.up;
                }
                    return Vector2.down;
            }

            Vector2 LeftRight()
            {
                if (targetplace.position.y == transform.position.y)
                {
                    return Vector2.zero;
                }
                else if (targetplace.position.y > transform.position.y)
                {
                    return Vector2.right;
                }
                    return Vector2.left;
            }

            bool HitCheck(RaycastHit2D hit)
            {
                if (hit == null)
                {
                    Debug.Log("hit = null");
                    return false;
                }
                else if (hit.transform == null)
                {
                    Debug.Log("hit but no object actacted");
                    return false;
                }
                else if (hit.transform.gameObject.tag == wall || hit.transform.gameObject.tag == "Player")
                {
                    Debug.Log("succesful hit " + hit.transform.gameObject.name);
                    return true; 
                }

                Debug.Log("hit " + hit.transform.gameObject.name);

                return false;
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
}
