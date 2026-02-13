using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    
    [SerializeField] string wall;
    [SerializeField] float rayCastRangeGrid = 1;
    public LayerMask nullAvoidance;


    //privatemovevariables

    bool inMotion;
    bool collisionWithWall;
    Vector3 target;
    Vector3 lastTarget;
    Vector2 inputDirection;

    public bool UpdateMove()
    {
        if (inMotion)
        {
            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                inMotion = false;
                //Debug.Log("going out of motion at 1");
                return false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                return true;
            }
            /*
            if (collisionWithWall)
            {
                if (transform.position.x == target.x && transform.position.y == target.y)
                {
                    inMotion = false;
                    //Debug.Log("going out of motion at 1");
                    return false;
                }
                else
                {
                    Vector3 rayStart = transform.position;
                    rayStart.x = rayStart.x + inputDirection.x / 2;
                    rayStart.y = rayStart.y + inputDirection.y / 2;

                    RaycastHit2D hitRay = Physics2D.Raycast(rayStart, inputDirection, rayCastRangeGrid);
                    if (HitCheck(hitRay))
                    {
                        collisionWithWall = true;
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    }
                    //Debug.Log(gameObject.name + " is in motion");
                    return true;
                }
            }
            else
            {
                if (transform.position.x == lastTarget.x && transform.position.y == lastTarget.y)
                {
                    inMotion = false;
                    collisionWithWall = false;
                    return false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, lastTarget, speed * 2 * Time.deltaTime);
                    return true;
                }

            }
            */
        }
        //Debug.Log("going out of motion at 2");
        return false;

    }

    public void moveTowards(Vector2 input)
    {
        inputDirection = input;
        lastTarget = target;
        target = new Vector3(transform.position.x + input.x, transform.position.y + input.y, transform.position.z);
        Debug.Log(gameObject.name + " direction = " + target);
        inMotion = true;
    }

    public virtual void TargetUpdate(RoomIndex[] roomIndexRef)
    {
        //empty for emeny ref
    }

    public bool HitCheck(RaycastHit2D hit)
    {
        if (hit.transform == null)
        {
            //Debug.Log("hit but no object actacted " + hit);
            return false;
        }
        else if (hit.transform.gameObject.tag == wall || hit.transform.gameObject.tag == "Player")
        {
            //Debug.Log("succesful hit " + hit.transform.gameObject.name);
            return true;
        }

        //Debug.Log("hit " + hit.transform.gameObject.name);

        return false;
    }
}
