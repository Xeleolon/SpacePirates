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
            Breakable[] typeBreakable = new Breakable[allDoors.Length]; //temp storage for alternation
            int[] typeValues = new int[allDoors.Length]; //temp storage for alternation



            for (int eachDoor = 0; eachDoor < allDoors.Length; eachDoor++) //grab highest value
            {
                typeBreakable[eachDoor] = allDoors[eachDoor].GiveBreakaable(type); //this is the highestvalue breakable for the door
                typeValues[eachDoor] = typeBreakable[eachDoor].GiveValue(type);
            }



            for (int eachDoor = 0; eachDoor < allDoors.Length; eachDoor++) //create a list of all available doors with their values
            {
                int[] loopCopyOfTypeValues = new int[typeValues.Length];
                for (int copyArraryLoop = 0; copyArraryLoop < typeValues.Length; copyArraryLoop ++)
                {
                    loopCopyOfTypeValues[copyArraryLoop] = typeValues[copyArraryLoop];
                }


                for (int alterCopy = 0; alterCopy < loopCopyOfTypeValues.Length; alterCopy++) //alter the copid list values for the door offeset for neiber
                {
                    //Debug.Log("at start door" + eachDoor + ", has for door" + alterCopy + " a score of " + loopCopyOfTypeValues[alterCopy]);

                    int tempAlter = loopCopyOfTypeValues[alterCopy]; //alter values for the door
                    tempAlter = tempAlter - (allDoors[eachDoor].neighbooringDoors[alterCopy] * sensorDeperation); //alter values for the door

                    //Debug.Log("stage 3," + alterCopy + " door" + eachDoor + " has a value of " + typeValues[alterCopy]);
                    //Debug.Log("stage 3," + alterCopy + " doorCopy" + eachDoor + " has a value of " + loopCopyOfTypeValues[alterCopy]);

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


                int highScorer = typeValues[eachDoor]; //set check as min orrginal door
                int placement = eachDoor; //get placement form orignal door

                

                //find door highvalue
                for (int highScoreLoop = 0; highScoreLoop < loopCopyOfTypeValues.Length; highScoreLoop++)
                {
                    if (highScorer < loopCopyOfTypeValues[highScoreLoop])
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


                
                
                if (eachDoor != placement) //don't do anything if door = placement data allready set
                {
                    int[] neighbouringDoors = allDoors[eachDoor].neighbooringDoors;
                    if (neighbouringDoors[placement] == 1) //asign target door if it direct neighbour already
                    {
                        allDoors[eachDoor].AsisgnTarget(type, allDoors[placement], typeValues[placement]);
                    }
                    else //isn't a neighbouring door find the door closest to final target
                    {
                        int neighboursplacement = findNeighbouringDoor();
                        int neighboursValue = typeValues[placement] - (neighbouringDoors[placement] * sensorDeperation); //create values depened on the deperation as this is not a direct neigbour
                        allDoors[eachDoor].AsisgnTarget(type, allDoors[neighboursplacement], neighboursValue);

                    }

                    int findNeighbouringDoor()
                    {
                        int targetValue = neighbouringDoors[placement];
                        int placeOfTargetValue = placement;
                        for (int neighbourLoop = 0; neighbourLoop < neighbouringDoors.Length; neighbourLoop++)
                        {
                            if (neighbouringDoors[neighbourLoop] == 1 && (targetValue > allDoors[neighbourLoop].neighbooringDoors[placement] || (targetValue == allDoors[neighbourLoop].neighbooringDoors[placement] && 0.5 > Random.Range(0, 1))))
                            {
                                targetValue = allDoors[neighbourLoop].neighbooringDoors[placement];
                                placeOfTargetValue = neighbourLoop;
                            }
                        }
                        return placeOfTargetValue;
                    }
                }
                

            }
        }


    }



    




}
