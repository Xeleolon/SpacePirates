using UnityEngine;
public enum Attractant {player, energy, noAttractant};
public class RoomIndex : MonoBehaviour
{
    [SerializeField] Breakable[] avalableBreakable;
    [SerializeField] Breakable[] doors;

    private Breakable[] energy;

    void OnVaildate()
    {
        //assign spellizeds fields
        SetBreakableLists();

    }

    void Awake()
    {
        SetBreakableLists();
    }


    #region generate Breakable Lists
    void SetBreakableLists()
    {

        Breakable[] allBreakables = CompleteAllBreakable();
        energy = BreakableObject(Attractant.energy);

        Breakable[] BreakableObject(Attractant type)
        {
            Breakable[] tempStorage = new Breakable[allBreakables.Length];
            int numbOfAttractant = 0;

            for (int i = 0; i < allBreakables.Length; i++)
            {
                //Debug.Log(i + " placement, attrachant is " + allBreakables[i].attractant);
                if (allBreakables[i].attractant == type)
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

    Transform CheckBreakableLists(Attractant type, Vector3 location)
    {
        switch (type)
        {
            case Attractant.energy:
                return energy[CheckingLists(energy)].transform;

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
                if ((checkthis[i].attractiveMeter + distanceModifier) > valueTarget);
                {
                    placementTarget = i;
                    valueTarget = checkthis[i].attractiveMeter + distanceModifier;
                }

            }
            return placementTarget;
        }

        return null;
    }
}
