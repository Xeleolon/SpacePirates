using UnityEngine;
public enum MovementDirection {up, down, left, right, stop }
public enum ActackType {none, strike, contiuneTillDead, huntPlayer}
public class GridMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [Tooltip("if the object takes more than speed * this it returns to last target")]
    [SerializeField] float cancelSpeed = 1.5f;
    private float cancelSpeedClock;
    [Header("CollisionChecks")]
    [SerializeField] string[] raycastTagIngore;
    [SerializeField] float rayCastRangeGrid = 1;
    [Tooltip("commonly used for actacking through collision")]
    public LayerMask nullAvoidance;
    [SerializeField] float onCollisionDeviation = 0.2f;
    [SerializeField] string[] collisionTagAttack;
    [Header("Animation")]
    [HideInInspector] public Animator animator;
    [SerializeField] Animator strikeAnimator;
    [SerializeField] string[] strikeAnimations;
    [SerializeField] bool useMovementAnimations;
    [SerializeField] string[] movementAnimations;


    //privatemovevariables

    bool inMotion;
    bool returnToLastTarget = false;
    bool collisionWithWall;
    Vector3 target;
    Vector3 lastTarget;
    [HideInInspector] public Vector3 spawn;
    [HideInInspector] public MovementDirection inputDirection;
    

    public Transform targetplace;
    Rigidbody2D rb;
    public virtual void Start()
    {
        //strikeAnimator = GetComponent<Animator>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayMovementAnimations();
    }
    #region Movement
    public bool UpdateMove()
    {
        //movement
        if (returnToLastTarget)
        {
            rb.linearVelocity = Vector2.zero;
            if (transform.position.x == lastTarget.x && transform.position.y == lastTarget.y)
            {

                target = lastTarget;
                //Debug.Log(gameObject.name + " Returning to last target");
                inMotion = false;
                returnToLastTarget = false;
                cancelSpeedClock = 0;
                //Debug.Log("going out of motion at 1");
                return false;
            }
            else
            {
                if (cancelSpeedClock > cancelSpeed)
                {
                    return false;
                }

                transform.position = Vector3.MoveTowards(transform.position, lastTarget, speed * Time.deltaTime);
                return true;
            }
        }
        else if (inMotion)
        {
            rb.linearVelocity = Vector2.zero;
            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                inMotion = false;
                cancelSpeedClock = 0;
                //Debug.Log("going out of motion at 1");
                return false;
            }
            else
            {
                if (cancelSpeedClock > cancelSpeed)
                {
                    return false;
                }
                
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                cancelSpeedClock += 1 * Time.deltaTime;
                
                return true;
            }
        }
        //Debug.Log("going out of motion at 2");
        return false;

    }

    public void moveTowards(Vector2 input)
    {

        inputDirection = InputToDirection(input);
        lastTarget = CheckTargets(target);
        target = CheckTargets(new Vector3(transform.position.x + input.x, transform.position.y + input.y, transform.position.z));
        //Debug.Log(gameObject.name + " Loading target " + target + " loading old Target " + lastTarget);
        PlayMovementAnimations();
        //recallicutae returnToLastTarget and Target to be on grid only
        Vector3 CheckTargets(Vector3 oldTarget)
        {
            Vector3 newTarget = oldTarget;
            newTarget.x = Mathf.Round(oldTarget.x);
            newTarget.y = Mathf.Round(oldTarget.y);


            return newTarget;

        }
        //Debug.Log(gameObject.name + " direction = " + target);
        inMotion = true;


    }

    public MovementDirection InputToDirection(Vector2 input)
    {
        //Debug.Log("movment direction = " + input);
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

        //return MovementDirection.stop;
        return inputDirection;
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
        if (inMotion)
        {

            if (CheckTag(collision.gameObject.tag, collisionTagAttack))
            {
                if (CallActack(collision.gameObject))
                {
                    //Debug.Log(gameObject.name + " tripped call Actack for collision");
                    target = lastTarget;
                }
            }
            cancelSpeedClock = 0;
            returnToLastTarget = true;
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

    #region Animations
    public void PlayStrikeAnimations()
    {
        //Debug.Log("Checking can Play Strike animations");
        if (strikeAnimator == null)
        {
            return;
        }
        if (strikeAnimations.Length < 4)
        {
            return;
        }
        //Debug.Log("Play Strike Animation");
        switch (inputDirection)
        {
            case MovementDirection.up:
                strikeAnimator.Play(strikeAnimations[0]);
                break;
            case MovementDirection.down:
                strikeAnimator.Play(strikeAnimations[1]);
                break;
            case MovementDirection.right:
                strikeAnimator.Play(strikeAnimations[2]);
                break;
            case MovementDirection.left:
                strikeAnimator.Play(strikeAnimations[3]);
                break;
        }
    }

    void PlayMovementAnimations()
    {
        //Debug.Log("Checking can Play Movement animations");
        if (animator == null)
        {
            return;
        }

        if (!useMovementAnimations)
        {
            return;
        }

        if (movementAnimations.Length < 4)
        {
            return;
        }
        //Debug.Log("Play Movement Animation");

        switch (inputDirection)
        {
            case MovementDirection.up:
                animator.Play(movementAnimations[0]);
                break;
            case MovementDirection.down:
                animator.Play(movementAnimations[1]);
                break;
            case MovementDirection.right:
                animator.Play(movementAnimations[2]);
                break;
            case MovementDirection.left:
                animator.Play(movementAnimations[3]);
                break;
        }
    }

    #endregion;
    public virtual void SpawnObject(RoomIndex startRoom)
    {

    }
    public void Respawn()
    {
        lastTarget = spawn;
        target = spawn;
        transform.position = spawn;
        returnToLastTarget = false;
        inMotion = false;
    }
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

    public Vector2 RayDirection()
    {
        //Debug.Log(inputDirection);
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
