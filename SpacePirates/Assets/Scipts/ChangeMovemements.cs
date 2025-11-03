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
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.SwitchMovement(movementTypeEnter, ship);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (exit && other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.SwitchMovement(movements.PlayerZeroG, ship);
            }
        }
    }
}
