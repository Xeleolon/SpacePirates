using UnityEngine;
public enum movements { playerGravity, PlayerZeroG}

public class BaseMovement : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] movements controlMovement = movements.playerGravity;

    //Vector3 movePoint;
    [Range(1, 10)]
    [SerializeField] float speedGravity = 5;
    [Range(1, 10)]
    [SerializeField] float speedZeroG = 5;
    [HideInInspector] public bool useTime;

    Rigidbody2D rb;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 heldInput;
    public void UpdateMove(Vector2 input)
    {
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
        float addTime = 1;
        if (useTime)
        {
            addTime = Time.fixedDeltaTime;
        }
        rb.MovePosition(rb.position + heldInput.normalized * speedGravity * addTime);
    }

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
        }
    }
}
