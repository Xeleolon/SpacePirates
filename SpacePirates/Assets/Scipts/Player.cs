using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public enum movements { playerGravity, PlayerZeroG, Ship}
public enum handItem {grapple, gun}

public class Player : MonoBehaviour
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

    [Header("Movements")]
    [SerializeField] movements controlMovement = movements.playerGravity;

    //Vector3 movePoint;
    [Range(1, 10)]
    [SerializeField] float speedGravity = 5;
    [Range(1, 10)]
    [SerializeField] float speedZeroG = 5;

    Rigidbody2D rb;
    Animator animator;
    public Camera cameraActive;

    [SerializeField] float health = 10;
    float curHealth;

    

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = curHealth;

        //cam.ScreenToWorldPoint(look);
    }
    //movements
    Vector2 heldInput;
    bool keyboardControl;

    void Update()
    {
        #region movement Updates
        

        
         Vector2 input = move.ReadValue<Vector2>();
        //Vector2 movement;
        //movement = input.normalized * speed * Time.deltaTime;
        //transform.position += new Vector3(movement.x, movement.y, transform.position.z);
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Speed", input.sqrMagnitude);

        float curSpeed;
        switch (controlMovement)
        {
            case movements.playerGravity:
                heldInput = input;
                curSpeed = speedGravity;
                break;
            case movements.PlayerZeroG:
                if (heldInput.x != input.x && input.x != 0)
                {
                    heldInput.x += input.x;
                }

                if (heldInput.y != input.y && input.y != 0)
                {
                    heldInput.y += input.y;
                }
                curSpeed = speedZeroG;
                break;
        }

        //Debug.Log(input.sqrMagnitude);
        rb.MovePosition(rb.position + heldInput.normalized * speedGravity * Time.fixedDeltaTime);



        #endregion





    }

    private void FixedUpdate()
    {
        #region movement FixedUpdates
        
        #endregion

    }

    #region movement Switch
    public void SwitchMovement(movements movementType)
    {

        switch (movementType)
        {
            case movements.playerGravity:
                rb.linearVelocity = Vector3.zero;
                controlMovement = movementType;
            break;

            case movements.PlayerZeroG:
                controlMovement = movementType;
            break;

            case movements.Ship:

            break;
        }
    }

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

    public void AlterHealth(float alter)
    {
        curHealth += alter;
        if (curHealth <= 0)
        {
            curHealth = 0;
            Debug.Log("Player Died");
        }
        else if (curHealth >= health)
        {
            curHealth = health;

        }
    }

    #endregion

}
