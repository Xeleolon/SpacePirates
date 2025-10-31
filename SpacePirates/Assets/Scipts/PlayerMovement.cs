using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Awake & Input
    private InputSystem_Actions playerControls;
    private InputAction move;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    #endregion

    Vector3 movePoint;
    [Range (1,10)]
    [SerializeField] float speed = 1;
    Rigidbody2D rb;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }



    void Update()
    {

        Vector2 input = move.ReadValue<Vector2>();
        //Vector2 movement;
        //movement = input.normalized * speed * Time.deltaTime;
        //transform.position += new Vector3(movement.x, movement.y, transform.position.z);
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Speed", input.sqrMagnitude);
        Debug.Log(input.sqrMagnitude);
        rb.MovePosition(rb.position + input.normalized * speed * Time.fixedDeltaTime);

        
    }
}
