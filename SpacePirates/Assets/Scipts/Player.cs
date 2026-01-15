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
    [SerializeField] float speedShip = 5;

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
    Vector2 movementVar;
    Vector2 mousePos;
    bool keyboardControl;

    

    void Update()
    {
        #region movement Updates
        /*Vector2 movementInput = move.ReadValue<Vector2>();
        movementVar = transform.right * movementInput.x + transform.up * movementInput.y;
        
        //look to mouse
        Vector2 mouseVector = look.ReadValue<Vector2>();

        if (mouseVector != Vector2.zero)
        {
            keyboardControl = false;
            mousePos = mouseVector;
        }
        else if (keyboardControl)
        {
            mouseVector = cameraActive.ScreenToWorldPoint(mouseLook.ReadValue<Vector2>());
            mousePos = mouseVector;
        }*/

        
         Vector2 input = move.ReadValue<Vector2>();
        //Vector2 movement;
        //movement = input.normalized * speed * Time.deltaTime;
        //transform.position += new Vector3(movement.x, movement.y, transform.position.z);
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Speed", input.sqrMagnitude);
        Debug.Log(input.sqrMagnitude);
        rb.MovePosition(rb.position + input.normalized * speedGravity * Time.fixedDeltaTime);
         
         
         
#endregion





    }

    private void FixedUpdate()
    {
        #region movement FixedUpdates
        /*//movments
        switch (controlMovement)
        {
             case movements.playerGravity:
                PlayerGravityMovement();
                PlayerRotation();
                break;

             case movements.PlayerZeroG:
                PlayerZeroGMovement();
                PlayerRotation();
                PlayerZeroGMovement();
                break;
        }

        void PlayerGravityMovement()
        {
            rb.MovePosition(rb.position + movementVar * speedGravity * Time.fixedDeltaTime);
        }

        void PlayerZeroGMovement()
        {
            rb.AddForce(movementVar * speedZeroG);
        }


        //playerRotation
        void PlayerRotation()
        {
            Vector2 lookDir;
            if (!keyboardControl)
            {
                lookDir = mousePos;
            }
            else
            {
                lookDir = mousePos - rb.position;
            }
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

            rb.rotation = angle;
        }
        */
        #endregion

    }

    #region movement Switch
    public void SwitchMovement(movements movementType, GameObject controlObject)
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
