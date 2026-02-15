using UnityEngine;

public class ChangeMovemements : MonoBehaviour
{
    [SerializeField] movements movementTypeEnter = movements.playerGravity;
    [SerializeField] movements movementTypeExit = movements.PlayerZeroG;
    [SerializeField] bool exit;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseMovement creature = other.gameObject.GetComponent<BaseMovement>();
        if (creature != null)
        {
            creature.SwitchMovement(movementTypeEnter);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BaseMovement creature = other.gameObject.GetComponent<BaseMovement>();
        if (creature != null)
        {
            creature.SwitchMovement(movementTypeExit);
        }
    }
}
