using UnityEngine;


public class Doorway : Breakable
{
    public int objectNumber = 0;
    [Tooltip("How many doors away is each door from this door. This is times in level manager by sensor Deperation")]
    public int[] neighbooringDoors;

    [SerializeField] RoomIndex[] roomsReffences;
    public Breakable energyTarget;
    [SerializeField] string emenyTag;
    [SerializeField] string playerTag;
    public void GetScore()
    {
        if (roomsReffences == null || roomsReffences.Length <= 0)
        {
            Debug.LogError(gameObject.name + "Does not have any neighboors asigned");
            return;
        }
        energyTarget = this;

        for (int i = 0; i < roomsReffences.Length; i++)
        {
            //Enegery
            Breakable tempTarget = roomsReffences[i].CheckBreakableLists(Attractant.energy, transform.position);
            if (tempTarget != null)
            {
                if (energyActractive < tempTarget.energyActractive)
                {
                    energyActractive = tempTarget.energyActractive;
                    energyTarget = tempTarget;
                }
                else if (energyActractive == tempTarget.energyActractive && 0.5 > Random.Range(0, 1)) //case of equal objects
                {
                    energyActractive = tempTarget.energyActractive;
                    energyTarget = tempTarget;
                }
            }
        }

        
    }

    public void AsisgnTarget(Attractant type, Breakable target, int targetActractiveness)
    {
        switch (type)
        {
            case Attractant.energy:
                energyTarget = target;
                energyActractive = targetActractiveness;
                break;

            default:
                Debug.LogWarning(type + " has no assigned target data setup in DoorWay script");
                break;


        }
    }

    public Breakable GiveBreakaable(Attractant type)
    {
        switch(type)
        {
            case Attractant.energy:
            return energyTarget;

            default:
                Debug.LogWarning(type + " has no assigned target data setup in DoorWay script");
            return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == emenyTag)
        {
            GridMovement emenyMovementScript = other.gameObject.GetComponent<GridMovement>();
            if (emenyMovementScript != null)
            {
                emenyMovementScript.TargetUpdate(roomsReffences);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == playerTag)
        {
            //provide player as avialble option
        }
    }



}
