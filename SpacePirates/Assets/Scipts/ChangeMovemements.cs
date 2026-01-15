using UnityEngine;

public class ChangeMovemements : MonoBehaviour
{
    [SerializeField] movements movementTypeEnter = movements.Ship;
    [SerializeField] bool exit;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.SwitchMovement(movementTypeEnter);
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
                player.SwitchMovement(movements.PlayerZeroG);
            }
        }
    }
}
