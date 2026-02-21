using UnityEngine;
using System.Collections.Generic;
public enum Attractant {player, energy, noAttractant, sleapPods, computer};
[System.Serializable]
public class BreakableLists
{
    [HideInInspector]
    public string fontName;
    public Attractant attractant;
    [HideInInspector]
    public Breakable[] array;
    public void SetNames()
    {
        fontName = attractant.ToString();
    }
}
public class RoomIndex : MonoBehaviour
{
    [SerializeField] Breakable[] avalableBreakable;
    [SerializeField] Breakable[] doors;
    #region typeLists
    public List<BreakableLists> breakableLists = new List<BreakableLists>();
    #endregion

    private int numberOfCAlls;
    [HideInInspector] public int TotalHealth;
    private bool loopOnced;
    [SerializeField] bool showSpawnLocations;
    public Transform spawnLocationParent;
    [HideInInspector] public Transform[] spawnLocations;

    #region onDrawGizmos & Validate
    private void OnDrawGizmos()
    {
        if (spawnLocationParent == null || !showSpawnLocations)
        {
            return;
        }
        int childCount = spawnLocationParent.childCount;
        spawnLocations = new Transform[childCount];
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(0.9f, 0.9f, 0.9f);
        for (int gizmosLoop = 0; gizmosLoop < spawnLocations.Length; gizmosLoop++)
        {
            spawnLocations[gizmosLoop] = spawnLocationParent.GetChild(gizmosLoop);
            Gizmos.DrawWireCube(new Vector3(spawnLocations[gizmosLoop].position.x, spawnLocations[gizmosLoop].position.y, transform.position.z), boxSize);
        }
    }
    private void OnValidate()
    {
        if (breakableLists.Count > 0)
        {
            for (int i = 0; i < breakableLists.Count; i++)
            {

                breakableLists[i].SetNames();
            }
        }
    }
    #endregion

    private void Start()
    {
        if (spawnLocationParent != null)
        {
            int childCount = spawnLocationParent.childCount;
            spawnLocations = new Transform[childCount];

            for (int spawnLoop = 0; spawnLoop < spawnLocations.Length; spawnLoop++)
            {
                spawnLocations[spawnLoop] = spawnLocationParent.GetChild(spawnLoop);
            }
        }
    }


    #region generate Breakable Lists
    public bool SetBreakableLists()
    {
        TotalHealth = 0;
        Breakable[] allBreakables = CompleteAllBreakable();
        if (breakableLists.Count > 0)
        {
            for (int i = 0; i < breakableLists.Count; i++)
            {
                breakableLists[i].array = BreakableObject(breakableLists[i].attractant);
            }
        }
        else
        {
            Debug.LogError("No Breakable List set in Area Index " + gameObject.name);
        }

        if (!loopOnced)
        {
            LevelUIControl.instance.SetRoomHealth(this, TotalHealth);
        }
        loopOnced = true;
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
                if (tempStorage[i] == null)
                {
                    Debug.Log(gameObject.name + " "+  tempStorage[i] + " " + i + " is null");
                    return null;
                }
                TotalHealth += tempStorage[i].health;

                if (!loopOnced)
                {
                    
                    tempStorage[i].SetRoom(this);
                }
            }

            for (int i = 0; i < doors.Length; i++)
            {
                tempStorage[avalableBreakable.Length + i] = doors[i];
            }
            return tempStorage;
        }
    }
    #endregion

    public Breakable CheckBreakableLists(Attractant type, Vector3 location, bool nullifyDoors) //return close object ot provide object of attranct type
    {
        numberOfCAlls += 1;
        if (!breakableLists.Exists(i => i.attractant == type))
        {
            Debug.Log("Area Index "+ gameObject.name + " has no list for attranct type of " + type);
            return null;
        }

        BreakableLists temp = breakableLists.Find(i => i.attractant == type);
        if (temp.array == null || temp.array.Length == 0)
        {
            return null;
        }
        else
        {
            return temp.array[CheckingLists(temp.array)];
        }

        int CheckingLists(Breakable[] checkthis)
        {
            int placementTarget = 0;
            float valueTarget = 0;

            for (int i = 0; i < checkthis.Length; i++)
            {
                float distanceModifier= Vector3.Distance(checkthis[i].transform.position, location);
                distanceModifier = 1 - (distanceModifier / 100);
                int checkValue = 0;


                checkValue = checkthis[i].GiveValue(type);

                if (nullifyDoors && checkthis[i].GetComponent<Doorway>() != null)
                {
                    checkValue = 0;
                }

                if ((checkValue + distanceModifier) > valueTarget)
                {
                    placementTarget = i;
                    valueTarget = checkValue + distanceModifier;
                    //Debug.Log("call" + numberOfCAlls + " " + gameObject.name + ", loop" + i + " produces valueTarget of " + valueTarget + " distanceModifier = " + distanceModifier);
                }

            }

            //Debug.Log(placementTarget + " = placmeent target " + energy.Length + " = engery length");
            return placementTarget;
        }

        //return null;
    }
}
