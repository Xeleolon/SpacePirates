using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
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
    int curHealth;

    

    

    private void Start()
    {
        animator = GetComponent<Animator>();
        curHealth = health;

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

        Debug.Log("fire");
    }
    #endregion


    #region Health

    public void AlterHealth(int alter)
    {
        curHealth += alter;
        if (curHealth <= 0)
        {
            curHealth = 0;
            Debug.Log("Player Died");
            LevelUIControl.instance.ChangeHeath(curHealth - alter);
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

    #endregion

}
