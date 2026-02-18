using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
//using UnityEngine.Physics2DModule;
public enum handItem {grapple, gun}

public class Player : GridMovement
{
    #region Awake & Input
    private InputSystem_Actions playerControls;

    private InputAction move;

    private InputAction look;
    private InputAction mouseLook;
    private InputAction keyboardActive;
    private InputAction shoot;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();

    }

    private void OnEnable()
    {
        //movement input
        move = playerControls.Player.Move;
        move.Enable();

        //player Rotation inputs
        look = playerControls.Player.Look;
        look.Enable();
        mouseLook = playerControls.Player.MouseLook;
        mouseLook.Enable();
        keyboardActive = playerControls.Player.Keyboard;
        keyboardActive.Enable();
        keyboardActive.performed += KeyboardSwitch;

        //fire button
        shoot = playerControls.Player.Attack;
        shoot.Enable();
        shoot.performed += FireInput;
    }

    private void OnDisable()
    {
        move.Disable();

        look.Disable();
        mouseLook.Disable();
        keyboardActive.Disable();

        shoot.Disable();
    }

    #endregion



    Animator animator;

    [SerializeField] int health = 10;
    [SerializeField] float inputGive = 0.5f;
    [SerializeField] float raycastRange = 1;
    [Header("Combat")]
    [SerializeField] float damage = 1;
    [SerializeField] float actackFrequence = 1;
    [SerializeField] float actackDistance = 1f;
    [SerializeField] float actackSpread = 1.5f;
    [SerializeField] int numberOfHits = 3;
    [SerializeField] ContactFilter2D actackFilter;
    private float actackCloak;
    int curHealth;
    private void OnDrawGizmos()
    {
        Vector2 area = new Vector2(actackDistance, actackSpread);
        Vector2 areaPoint = new Vector2(transform.position.x, transform.position.y);

        switch (inputDirection)
        {
            case MovementDirection.up:
                area = new Vector2(actackSpread, actackDistance);
                areaPoint = new Vector2(transform.position.x, transform.position.y + actackDistance / 2 + 0.5f);
                break;
            case MovementDirection.down:
                area = new Vector2(actackSpread, actackDistance);
                areaPoint = new Vector2(transform.position.x, transform.position.y - actackDistance / 2 + 0.5f);
                break;
            case MovementDirection.right:
                area = new Vector2(actackDistance, actackSpread);
                areaPoint = new Vector2(transform.position.x + actackDistance / 2 + 0.5f, transform.position.y);
                break;
            case MovementDirection.left:
                area = new Vector2(actackDistance, actackSpread);
                areaPoint = new Vector2(transform.position.x - actackDistance / 2 + 0.5f, transform.position.y);
                break;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(areaPoint.x, areaPoint.y, transform.position.z), new Vector3(area.x, area.y, 1));


    }


    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        curHealth = health;
        spawn = transform.position;

        //cam.ScreenToWorldPoint(look);
    }
    //movements
    bool keyboardControl;

    void Update()
    {
        #region movement Updates
        if (!UpdateMove())
        {
            Vector2 input = move.ReadValue<Vector2>();
            //Vector2 movement;
            //movement = input.normalized * speed * Time.deltaTime;
            //transform.position += new Vector3(movement.x, movement.y, transform.position.z);
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.y);
            animator.SetFloat("Speed", input.sqrMagnitude);

            Vector2 directionLeftRight = Vector2.zero;
            Vector2 directionUpDown = Vector2.zero;
            
            if (input.x >= inputGive)
            {
                directionLeftRight = Vector2.right;
            }
            else if (input.x <= -inputGive)
            {
                directionLeftRight = Vector2.left;
            }

            if (input.y >= inputGive)
            {
                directionUpDown = Vector2.up;
            }
            else if (input.y <= -inputGive)
            {
                directionUpDown = Vector2.down;
            }

            //check if can move in that direction
            if (directionLeftRight != Vector2.zero)
            {
                RaycastHit2D hitLeftRight = Physics2D.Raycast(transform.position, directionLeftRight, raycastRange, nullAvoidance);
                if (HitCheck(hitLeftRight))
                {
                    directionLeftRight = Vector2.zero;
                }
            }

            if (directionUpDown != Vector2.zero)
            {
                RaycastHit2D hitUpDown = Physics2D.Raycast(transform.position, directionUpDown, raycastRange, nullAvoidance);
                if (HitCheck(hitUpDown))
                {
                    directionUpDown = Vector2.zero;
                }
            }


            if (directionLeftRight == Vector2.zero && directionUpDown != Vector2.zero)
            {
                moveTowards(directionUpDown);
                //Debug.Log("input Up");
            }
            else if (directionUpDown == Vector2.zero && directionLeftRight != Vector2.zero)
            {
                moveTowards(directionLeftRight);
                //Debug.Log("input side");
            }
            else if (directionUpDown != Vector2.zero && directionLeftRight != Vector2.zero)
            {
                if (Random.Range(0,1) > 0.5f)
                {
                    moveTowards(directionLeftRight);
                    //Debug.Log("booth input picked side");
                }
                else
                {
                    moveTowards(directionUpDown);
                    //Debug.Log("booth input picked up");
                }
            }
            else
            {
                //Debug.Log("No Movement");
            }
        }






        #endregion

        if (actackCloak > 0)
        {
            actackCloak -= 1 * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        #region movement FixedUpdates
        
        #endregion

    }

    #region keyboardSwitch

    private void KeyboardSwitch(InputAction.CallbackContext context)
    {
        keyboardControl = true;
        //Debug.Log("switch called");
    }

    #endregion


    #region WeaponFire
    private void FireInput(InputAction.CallbackContext context)
    {
        Actack();
    }

    void Actack()
    {
        if (actackCloak > 0)
        {
            return;
        }


        Vector2 area = new Vector2(actackDistance, actackSpread);
        Vector2 areaPoint = new Vector2(transform.position.x, transform.position.y);

        switch(inputDirection)
        {
            case MovementDirection.up:
                area = new Vector2(actackSpread, actackDistance);
                areaPoint = new Vector2(transform.position.x, transform.position.y + actackDistance / 2 + 0.5f);
                break;
            case MovementDirection.down:
                area = new Vector2(actackSpread, actackDistance);
                areaPoint = new Vector2(transform.position.x, transform.position.y - actackDistance / 2 + 0.5f);
                break;
            case MovementDirection.right:
                area = new Vector2(actackDistance, actackSpread);
                areaPoint = new Vector2(transform.position.x + actackDistance / 2 + 0.5f, transform.position.y);
                break;
            case MovementDirection.left:
                area = new Vector2(actackDistance, actackSpread);
                areaPoint = new Vector2(transform.position.x - actackDistance / 2 + 0.5f, transform.position.y);
                break;
        }


        Collider2D[] HitEmenies = new Collider2D[numberOfHits];

        int numberofHits = Physics2D.OverlapBox(areaPoint, area, 0, actackFilter, HitEmenies);


        if (numberOfHits <= 0)
        {
            //Debug.Log("Player had no Hits");
            actackCloak = actackFrequence;
            return;
        }
        //Debug.Log("Player has hit ");
        for (int i = 0; i < HitEmenies.Length; i++)
        {
            //Debug.Log(i + " Damaging ");
            if (HitEmenies[i] != null)
            {
                //Debug.Log(i + " Damaging " + HitEmenies[i]);
                EmenyBase emenyHealth = HitEmenies[i].GetComponent<EmenyBase>();
                if (emenyHealth != null)
                {
                    emenyHealth.AlterHealth(-damage);
                    //actackCloak = actackFrequence;
                }
            }
        }

        actackCloak = actackFrequence;
    }
    #endregion


    #region Health

    public void AlterHealth(int alter)
    {
        curHealth += alter;
        
        if (curHealth <= 0)
        {
            curHealth = 0;
            actackCloak = 0;
            Debug.Log("Player Died");
            Respawn();
            LevelUIControl.instance.ChangeHeath(curHealth - alter);
            LevelUIControl.instance.PlayerDied(gameObject);
        }
        else if (curHealth >= health)
        {
            curHealth = health;
            LevelUIControl.instance.ChangeHeath(curHealth);

        }
        else
        {
            if (alter < 0)
            {
                LevelUIControl.instance.ChangeHeath(curHealth - alter);
            }
            else
            {
                LevelUIControl.instance.ChangeHeath(curHealth);
            }
        }
    }

    public void RespawnHealth()
    {
        for (int i = 0; i <= health; i++)
        {
            AlterHealth(1);
        }
    }

    #endregion

}
