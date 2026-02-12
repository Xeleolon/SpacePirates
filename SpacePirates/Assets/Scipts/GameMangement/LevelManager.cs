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
                typeBreakable[eachDoor] = allDoors[eachDoor].GiveBreakaable(type); //this is the highestvalue breakable for the door
                typeValues[eachDoor] = allDoors[eachDoor].GiveValue(type);
            }

            for (int eachDoor = 0; eachDoor < allDoors.Length; eachDoor++)
            {
                int[] loopCopyOfTypeValues = typeValues;

                for (int alterCopy = 0; alterCopy < loopCopyOfTypeValues.Length; alterCopy++) //alter the copid list values for the door offeset for neiber
                {
                    //Debug.Log("at start door" + eachDoor + ", has for door" + alterCopy + " a score of " + loopCopyOfTypeValues[alterCopy]);
                    int tempAlter = loopCopyOfTypeValues[alterCopy]; //alter values for the door
                    tempAlter = tempAlter - (allDoors[eachDoor].neighbooringDoors[alterCopy] * sensorDeperation); //alter values for the door


                    if (loopCopyOfTypeValues[alterCopy] == 0)
                    {
                        loopCopyOfTypeValues[alterCopy] = 0;
                    }
                    else if (tempAlter <= 1)
                    {
                        loopCopyOfTypeValues[alterCopy] = 1;
                    }
                    else
                    {
                        loopCopyOfTypeValues[alterCopy] = tempAlter;
                    }
                    //Debug.Log("at end door" + eachDoor + ", has for door" + alterCopy + " a score of " + loopCopyOfTypeValues[alterCopy]);
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


                //error need to assign neighbours only
                if (eachDoor != placement)
                {
                    allDoors[eachDoor].AsisgnTarget(type, allDoors[placement], typeValues[placement]);
                }
                

            }
        }


    }



    




}
