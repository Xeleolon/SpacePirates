using UnityEngine;
public enum MovementDirection {up, down, left, right, stop }
public enum ActackType {none, strike, contiuneTillDead, huntPlayer}
public class GridMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [Header("CollisionChecks")]
    [SerializeField] string[] raycastTagIngore;
    [SerializeField] float rayCastRangeGrid = 1;
    public LayerMask nullAvoidance;
    [SerializeField] float onCollisionDeviation = 0.2f;
    [SerializeField] string[] collisionTagAttack;


    //privatemovevariables

    bool inMotion;
    bool returnToLastTarget = false;
    bool collisionWithWall;
    Vector3 target;
    Vector3 lastTarget;
    [HideInInspector] public MovementDirection inputDirection;
    #region Movement
    public bool UpdateMove()
    {
        if (returnToLastTarget)
        {
            if (transform.position.x == lastTarget.x && transform.position.y == lastTarget.y)
            {
                inMotion = false;
                returnToLastTarget = false;
                //Debug.Log("going out of motion at 1");
                return false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, lastTarget, speed * Time.deltaTime);
                return true;
            }
        }
        else if (inMotion)
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
        }
        //Debug.Log("going out of motion at 2");
        return false;

    }

    public void moveTowards(Vector2 input)
    {
        inputDirection = InputToDirection(input);
        lastTarget = target;
        target = new Vector3(transform.position.x + input.x, transform.position.y + input.y, transform.position.z);
        //Debug.Log(gameObject.name + " direction = " + target);
        inMotion = true;


    }

    public MovementDirection InputToDirection(Vector2 input)
    {
        if (input.x > 0)
        {
            return MovementDirection.right;
        }
        else if (input.x < 0)
        {
            return MovementDirection.left;
        }

        if (input.y > 0)
        {
            return MovementDirection.up;
        }
        else if (input.y < 0)
        {
            return MovementDirection.down;
        }

        return MovementDirection.stop;
    }
    #endregion

    #region Target Update From RoomIndex

    public virtual void TargetUpdate(RoomIndex[] roomIndexRef)
    {
        //empty for emeny ref
    }
    #endregion

    #region collision Check

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inMotion && ForwardCheck())
        {
            if (CheckTag(collision.gameObject.tag, collisionTagAttack))
            {
                if (CallActack(collision.gameObject))
                {
                    target = lastTarget;
                }
            }

            returnToLastTarget = true;
        }

        bool ForwardCheck()
        {
            Vector3 collisionPosition = collision.gameObject.transform.position;
            switch (inputDirection)
            {
                case MovementDirection.right:
                    if (collisionPosition.x >= transform.position.x
                    && collisionPosition.y < transform.position.y + onCollisionDeviation
                    && collisionPosition.y > transform.position.y - onCollisionDeviation)
                    {
                        return true;
                    }
                    break;

                case MovementDirection.left:
                    if (collisionPosition.x <= transform.position.x
                    && collisionPosition.y < transform.position.y + onCollisionDeviation
                    && collisionPosition.y > transform.position.y - onCollisionDeviation)
                    {
                        return true;
                    }
                    break;

                case MovementDirection.up:
                    if (collisionPosition.y >= transform.position.y
                    && collisionPosition.x < transform.position.x + onCollisionDeviation
                    && collisionPosition.x > transform.position.x - onCollisionDeviation)
                    {
                        return true;
                    }
                    break;

                case MovementDirection.down:
                    if (collisionPosition.y <= transform.position.y
                    && collisionPosition.x < transform.position.x + onCollisionDeviation
                    && collisionPosition.x > transform.position.x - onCollisionDeviation)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
    #endregion
    #region Actack

    public virtual bool CallActack(GameObject collisionObject)
    {
        //call Actack
        return false;
    }
    #endregion

    public bool HitCheck(RaycastHit2D hit)
    {
        if (hit.transform == null)
        {
            //Debug.Log("hit but no object actacted " + hit);
            return false;
        }
        else if (CheckTag(hit.transform.gameObject.tag, raycastTagIngore))
        {
            //Debug.Log("succesful hit " + hit.transform.gameObject.name);
            return true;
        }


        

        //Debug.Log("hit " + hit.transform.gameObject.name);

        return false;

        
    }

    bool CheckTag(string tag, string[] checkTagList)
    {
        if (checkTagList.Length <= 0)
        {
            return false;
        }

        for (int tagCheckLoop = 0; tagCheckLoop < checkTagList.Length; tagCheckLoop++)
        {
            if (checkTagList[tagCheckLoop] == tag)
            {
                return true;
            }
        }

        return false;
    }
}
