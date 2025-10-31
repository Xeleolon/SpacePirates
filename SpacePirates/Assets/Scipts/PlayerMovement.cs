using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Awake & Input
    private InputSystem_Actions playerControls;
    private InputAction move;
    private InputAction look;

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
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
    }

    #endregion

    Vector3 movePoint;
    [Range (1,10)]
    [SerializeField] float speed = 5;

    Rigidbody2D rb;
    public Camera cam;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //cam.ScreenToWorldPoint(look);
    }

    Vector2 movementVar;
    Vector2 mousePos;
    bool angleInputZero;

    void Update()
    {
        Vector2 movementInput = move.ReadValue<Vector2>();
        movementVar = transform.right * movementInput.x + transform.up * movementInput.y;
        
        //look to mouse
        Vector2 mouseLook = look.ReadValue<Vector2>();
        if (mouseLook != Vector2.zero)
        {
            mousePos = mouseLook;
            angleInputZero = true;
            { Debug.Log(mouseLook); }
        }
        else
        {
            angleInputZero = false;
        }



    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementVar * speed * Time.fixedDeltaTime);


        //look to mouse
        if (angleInputZero)
        {
            Vector2 lookDir = mousePos;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

            rb.rotation = angle;
        }
    }
}
