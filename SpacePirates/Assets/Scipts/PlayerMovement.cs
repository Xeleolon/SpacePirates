using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Awake & Input
    private InputSystem_Actions playerControls;

    private InputAction move;

    private InputAction look;
    private InputAction mouseLook;
    private InputAction keyboardActive;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        look = playerControls.Player.Look;
        look.Enable();
        mouseLook = playerControls.Player.MouseLook;
        mouseLook.Enable();
        keyboardActive = playerControls.Player.Keyboard;
        keyboardActive.Enable();
        keyboardActive.performed += KeyboardSwitch;
    }

    private void OnDisable()
    {
        move.Disable();

        look.Disable();
        mouseLook.Disable();
        keyboardActive.Disable();
    }

    #endregion

    Vector3 movePoint;
    [Range (1,10)]
    [SerializeField] float speed = 5;

    Rigidbody2D rb;
    public Camera cameraActive;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //cam.ScreenToWorldPoint(look);
    }

    Vector2 movementVar;
    Vector2 mousePos;
    bool keyboardControl;

    void Update()
    {
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



    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementVar * speed * Time.fixedDeltaTime);
        Vector2 lookDir;

        //look to mouse
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

    private void KeyboardSwitch(InputAction.CallbackContext context)
    {
        keyboardControl = true;
        Debug.Log("switch called");
    }
}
