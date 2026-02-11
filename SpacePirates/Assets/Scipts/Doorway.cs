using UnityEngine;


public class Doorway : Breakable
{
    public int objectNumber = 0;
    [Tooltip("How many doors away is each door from this door. This is times in level manager by sensor Deperation")]
    public int[] neighbooringDoors;

    RoomIndex[] roomsReffences;
    public Breakable energyTarget;

    public void GetScore()
    {
        if (roomsReffences.Length <= 0)
        {
            Debug.LogError(gameObject.name + "Does not have any neighboors asigned");
            return;
        }
        
        for (int i = 0; i < roomsReffences.Length; i++)
        {
            //Enegery
            Breakable tempTarget = roomsReffences[i].CheckBreakableLists(Attractant.energy, transform.position);

            if (energyActractive < tempTarget.energyActractive)
            {
                energyActractive = tempTarget.energyActractive;
                energyTarget = tempTarget;
            }
            else if (energyActractive == tempTarget.energyActractive && 0.5 > Random.Range(0,1)) //case of equal objects
            {
                energyActractive = tempTarget.energyActractive;
                energyTarget = tempTarget;
            }
        }

        
    }

    public void AsisgnTarget(Attractant type, Breakable target)
    {
        switch (type)
        {
            case Attractant.energy:
                energyTarget = target;
                energyActractive = target.energyActractive;
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



}
