using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;


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
                //Debug.Log("in motion");
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
}
