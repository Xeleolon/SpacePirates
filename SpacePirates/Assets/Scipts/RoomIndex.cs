using UnityEngine;
public enum Attractant {player, energy, noAttractant};

public class RoomIndex : MonoBehaviour
{
    [SerializeField] Breakable[] avalableBreakable;
    [SerializeField] Breakable[] doors;

    private Breakable[] energy;


    #region generate Breakable Lists
    public bool SetBreakableLists()
    {

        Breakable[] allBreakables = CompleteAllBreakable();
        energy = BreakableObject(Attractant.energy);

        return true;

        Breakable[] BreakableObject(Attractant type)
        {
            Breakable[] tempStorage = new Breakable[allBreakables.Length];
            int numbOfAttractant = 0;

            for (int i = 0; i < allBreakables.Length; i++)
            {
                //Debug.Log(i + " placement, attrachant is " + allBreakables[i].attractant);
                if (allBreakables[i].CheckAttractanctMatch(type))
                {
                    tempStorage[numbOfAttractant] = allBreakables[i];
                    numbOfAttractant = numbOfAttractant + 1;
                }
            }

            //Debug.Log("num of attractant = " + numbOfAttractant + ", tempStorage Length" + tempStorage.Length);

            Breakable[] finalizedBreakables = new Breakable[numbOfAttractant];
            for (int i = 0; i < numbOfAttractant; i++)
            {
                finalizedBreakables[i] = tempStorage[i];
            }

            return finalizedBreakables;
        }

        Breakable[] CompleteAllBreakable()
        {
            Breakable[] tempStorage = new Breakable[avalableBreakable.Length + doors.Length];

            for (int i = 0; i < avalableBreakable.Length; i++)
            {
                tempStorage[i] = avalableBreakable[i];
            }

            for (int i = 0; i < doors.Length; i++)
            {
                tempStorage[avalableBreakable.Length + i] = doors[i];
            }
            return tempStorage;
        }
    }
    #endregion

    public Breakable CheckBreakableLists(Attractant type, Vector3 location)
    {
        switch (type)
        {
            case Attractant.energy:
                if (energy == null || energy.Length == 0)
                {
                    return null;
                }
                else
                {
                    return energy[CheckingLists(energy)];
                }

            case Attractant.player:

                return null ;
        }

        int CheckingLists(Breakable[] checkthis)
        {
            int placementTarget = 0;
            float valueTarget = 0;

            for (int i = 0; i < checkthis.Length; i++)
            {
                float distanceModifier= Vector3.Distance(checkthis[i].transform.position, location);
                distanceModifier = distanceModifier / 100;
                int checkValue = 0;

                switch(type)
                {
                    case Attractant.energy:

                        checkValue = checkthis[i].energyActractive;
                        break;
                    default:
                        checkValue = 0;
                        Debug.LogWarning(type + " is checking through default set up code line for correct value");
                        break;

                }
                if ((checkValue + distanceModifier) > valueTarget);
                {
                    placementTarget = i;
                    valueTarget = checkValue + distanceModifier;
                }

            }

            //Debug.Log(placementTarget + " = placmeent target " + energy.Length + " = engery length");
            return placementTarget;
        }

        return null;
    }
}
