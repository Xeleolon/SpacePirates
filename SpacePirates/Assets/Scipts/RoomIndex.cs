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
        Breakable[] allBreakables = CompleteAllBreakable();
        energy = BreakableObject(Attractant.energy);




        Breakable[] BreakableObject(Attractant type)
        {
            Breakable[] tempStorage = new Breakable[allBreakables.Length];
            int numbOfAttractant = 0;

            for (int i = 0; i < allBreakables.Length; i++)
            {
                if (allBreakables[i].attractant == type)
                {
                    tempStorage[numbOfAttractant] = allBreakables[i];
                    numbOfAttractant =+ 1;
                }
            }

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
}
