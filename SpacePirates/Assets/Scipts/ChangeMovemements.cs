using UnityEngine;

public class ChangeMovemements : MonoBehaviour
{
    [SerializeField] movements movementTypeEnter = movements.Ship;
    [SerializeField] bool exit;
    [SerializeField] GameObject ship;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().SwitchMovement(movementTypeEnter, ship);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (exit && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().SwitchMovement(movements.PlayerZeroG, ship);
        }
    }
}
