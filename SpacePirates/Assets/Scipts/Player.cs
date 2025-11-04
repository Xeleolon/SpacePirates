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
    public Camera cameraActive;

    [Header("Weapon/Tools")]
    [SerializeField] handItem heldItem = handItem.grapple;

    [SerializeField] Transform grappleHead;
    [SerializeField] Transform grappleHeadHome;
    [SerializeField] float grappleSpeed = 2;
    [SerializeField] float grappleReturnSpeed = 10;
    [SerializeField] float grappleRange = 5;
    [SerializeField] OnCollision grappleCollision;
    private Vector3 grappleOffset;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grappleOffset = grappleHead.position - transform.position;

        //cam.ScreenToWorldPoint(look);
    }
    //movements
    Vector2 movementVar;
    Vector2 mousePos;
    bool keyboardControl;

    private enum weaponState { prepped, loading, returning, firing, hit}
    //fire
    bool toolInput;
    bool grappleMovementOverride = false;
    weaponState grappleState = weaponState.prepped;

    void Update()
    {
        #region movement Updates
        Vector2 movementInput = move.ReadValue<Vector2>();
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
        }
        #endregion

        if (toolInput)
        {
            switch (heldItem)
            {
                case handItem.grapple:
                    GrappleFire();
                break;

                case handItem.gun:
                    toolInput = false;
                    grappleMovementOverride = false;
                break;
            }
        }

        void GrappleFire()
        {
           if (shoot.ReadValue<float>() <= 0)
           {
                grappleHead.SetParent(transform.parent);
                toolInput = false;
                grappleMovementOverride = false;
                if (grappleState != weaponState.prepped)
                {
                    grappleState = weaponState.returning;
               
                    StartCoroutine(ReturnGrapple());
                }
                return;
           }

           switch (grappleState)
            {
                case weaponState.prepped: //ready to fire
                    grappleMovementOverride = true;
                    grappleState = weaponState.firing;
                    grappleCollision.disableControl = true;
                    firingGrapple();
                    break;

                case weaponState.firing: //in motion of firing
                    firingGrapple();
                    break;

            }

            void firingGrapple()
            {
                if (Vector3.Distance(grappleHead.position, transform.position) >= grappleRange)
                {
                    grappleState = weaponState.returning;
                    StartCoroutine(ReturnGrapple());
                }
                else
                {
                    grappleHead.position = Vector3.MoveTowards(grappleHead.position, grappleHeadHome.up * (grappleRange + 1), (grappleSpeed * Time.deltaTime));
                }
            }
        }

    }

    private void FixedUpdate()
    {
        #region movement FixedUpdates
        //movments
        if (!grappleMovementOverride)
        {
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
        if (toolInput)
        {
            return;
        }

        switch (heldItem)
        {
            case handItem.grapple:

                toolInput = true;
            break;

            case handItem.gun:
                break;
        }
    }
    #endregion

    #region grapple
    private IEnumerator ReturnGrapple()
    {
        grappleMovementOverride = false;

        while (Vector3.Distance(grappleHead.position, grappleHeadHome.position) > 0.1f)
        {
            grappleHead.position = Vector3.MoveTowards(grappleHead.position, grappleHeadHome.position, (grappleReturnSpeed * Time.deltaTime + rb.linearVelocity.x + rb.linearVelocity.y));
            yield return null;
        }
        grappleState = weaponState.prepped;
        grappleHead.SetParent(transform.GetChild(0));
        yield return null;

    }

    private IEnumerator HitMoveGrapple()
    {
        grappleState = weaponState.hit;
        grappleCollision.disableControl = false;
        while (Vector3.Distance(transform.position, grappleHead.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, grappleHead.position, (grappleReturnSpeed * Time.deltaTime));
            yield return null;
        }
        grappleMovementOverride = false;
        grappleState = weaponState.prepped;
        grappleHead.SetParent(transform.GetChild(0));
        yield return null;

    }

    public void GrappleHit(GameObject other)
    {
        Debug.Log("grapple hit");
        //GameObject hit = other.GetComponent<OnCollision>().lastHit;
        StartCoroutine(HitMoveGrapple());
        rb.linearVelocity = Vector3.zero;
    }
    #endregion
}
