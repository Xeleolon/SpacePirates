using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] RoomIndex[] rooms;
    [SerializeField] Doorway[] allDoors;
    [SerializeField] int sensorDeperation = 1;


    private void Start()
    {
        SetupNagivationLists();
    }

    void SetupNagivationLists()
    {

        //create initial lists of all objects while doors are vauled at zero;
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].SetBreakableLists();
        }

        //get score from all lists.
        for (int i = 0; i < allDoors.Length; i++)
        {
            allDoors[i].GetScore();
        }

        AttractanctUpdateForDoors(Attractant.energy);

        //reset list to include doors;
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].SetBreakableLists();
        }


        void AttractanctUpdateForDoors(Attractant type)
        {
            Breakable[] typeBreakable = new Breakable[allDoors.Length];
            int[] typeValues = new int[allDoors.Length];
            for (int eachDoor = 0; eachDoor < allDoors.Length; eachDoor++)
            {
                typeBreakable[eachDoor] = allDoors[eachDoor].GiveBreakaable(type);
                typeValues[eachDoor] = allDoors[eachDoor].GiveValue(type);
            }

            for (int eachDoor = 0; eachDoor < allDoors.Length; eachDoor++)
            {
                int[] loopCopyOfTypeValues = typeValues;

                for (int alterCopy = 0; alterCopy < loopCopyOfTypeValues.Length; alterCopy++) //alter the copid list values for the door offeset for neiber
                {
                    loopCopyOfTypeValues[alterCopy] = loopCopyOfTypeValues[alterCopy] - (allDoors[eachDoor].neighbooringDoors[alterCopy] * sensorDeperation); //alter values for the door
                }


                int highScorer = 0;
                int placement = 0;
                //find door highvalue
                for (int highScoreLoop = 0; highScoreLoop < loopCopyOfTypeValues.Length; highScoreLoop++)
                {
                    if (highScoreLoop <= 0)
                    {
                        highScorer = loopCopyOfTypeValues[highScoreLoop];
                    }
                    else if (highScorer < loopCopyOfTypeValues[highScoreLoop])
                    {
                        highScorer = loopCopyOfTypeValues[highScoreLoop];
                        placement = highScoreLoop;
                    }
                    else if (highScorer == loopCopyOfTypeValues[highScoreLoop] && Random.Range(0, 1) > 0.5) //incase has mutiple high value pick one at random
                    {
                        highScorer = loopCopyOfTypeValues[highScoreLoop];
                        placement = highScoreLoop;
                    }
                }

                allDoors[eachDoor].AsisgnTarget(type, typeBreakable[placement]);

            }
        }


    }



    




}
