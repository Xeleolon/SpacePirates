using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    [SerializeField] MonoBehaviour script;
    [System.Serializable]
    public class CollisionEvent : UnityEvent<GameObject>
    {
        public GameObject gameObject;
    }

    public CollisionEvent collisionEvent = new CollisionEvent();
    public bool disableControl = true;
    [SerializeField] string collisionTag;
    [SerializeField] bool onCollision = false;
    [SerializeField] bool stayCollision = false;
    [SerializeField] bool exitCollision = false;
    [SerializeField] bool onTrigger = false;
    [SerializeField] bool stayTrigger = false;
    [SerializeField] bool exitTrigger = false;

    [HideInInspector] public GameObject lastHit;
    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("on collison detected point 1");
        if (onCollision)
        {
            //Debug.Log("collison detected point 2");
            CollisionOccured(other.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (stayCollision)
        {
            CollisionOccured(other.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (exitCollision)
        {
            CollisionOccured(other.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("on collison detected point 1 view trigger");
        if (onTrigger)
        {
            CollisionOccured(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (stayTrigger)
        {
            CollisionOccured(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (exitTrigger)
        {
            CollisionOccured(other.gameObject);
        }
    }

    void CollisionOccured(GameObject other)
    {
        if (!disableControl)
        {
            return;
        }
        if (collisionTag == string.Empty)
        {
            Debug.Log("collison detected point 3 empty tag");
            passCollsion();
        }
        else if (other.tag == collisionTag)
        {
            Debug.Log("collison detected point 3 tag met requipment");
            passCollsion();
        }


        void passCollsion()
        {
            if (other.tag != gameObject.tag)
            {
                lastHit = other;
                collisionEvent.Invoke(other.gameObject);
            }
        }
    }


}
