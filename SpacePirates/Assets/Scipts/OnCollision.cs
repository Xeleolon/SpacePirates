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
    void OnCollision2D(Collider2D other)
    {
        Debug.Log("collison detected point 1");
        if (onCollision)
        {
            Debug.Log("collison detected point 2");
            Collision(other.gameObject);
        }
    }

    void StayCollision2D(Collider2D other)
    {
        if (stayCollision)
        {
            Collision(other.gameObject);
        }
    }

    void ExitCollision2D(Collider2D other)
    {
        if (exitCollision)
        {
            Collision(other.gameObject);
        }
    }

    void OnTrigger2D(Collision2D other)
    {
        if (onTrigger)
        {
            Collision(other.gameObject);
        }
    }

    void StayTrigger2D(Collision2D other)
    {
        if (stayTrigger)
        {
            Collision(other.gameObject);
        }
    }

    void ExitTrigger2D(Collision2D other)
    {
        if (exitTrigger)
        {
            Collision(other.gameObject);
        }
    }

    void Collision(GameObject other)
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
