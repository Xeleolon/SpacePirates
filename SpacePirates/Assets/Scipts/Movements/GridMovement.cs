using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    
    [SerializeField] string wall;


    //privatemovevariables

    bool inMotion;
    Vector3 target;

    public bool UpdateMove()
    {
        if (inMotion)
        {
            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                inMotion = false;
                //Debug.Log("going out of motion at 1");
                return false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                //Debug.Log(gameObject.name + " is in motion");
                return true;             
            }
        }
        //Debug.Log("going out of motion at 2");
        return false;
    }

    public void moveTowards(Vector2 input)
    {
        target = new Vector3(transform.position.x + input.x, transform.position.y + input.y, transform.position.z);
        //Debug.Log("target = " + target);
        inMotion = true;
    }

    public virtual void TargetUpdate(RoomIndex[] roomIndexRef)
    {

    }

    public bool HitCheck(RaycastHit2D hit)
            {
                if (hit.transform == null)
                {
                    //Debug.Log("hit but no object actacted");
                    return false;
                }
                else if (hit.transform.gameObject.tag == wall || hit.transform.gameObject.tag == "Player")
                {
                    //Debug.Log("succesful hit " + hit.transform.gameObject.name);
                    return true; 
                }

               //Debug.Log("hit " + hit.transform.gameObject.name);

                return false;
            }
}
