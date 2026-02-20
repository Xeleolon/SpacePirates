using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    #region Instance/Awake
    public static LevelManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        //DontDestoryOnLoad(this.gameObject);
    }
    #endregion
    [Tooltip("LengthInMinutes")]
    [SerializeField] public float gameLength = 100;
    private bool winConditionMet;

    [Header("RoomData")]
    [SerializeField] RoomIndex[] rooms;
    [SerializeField] Doorway[] allDoors;
    [SerializeField] int sensorDeperation = 1;
    //spawnVarriables
    [Header("SpawnSystem")]
    [SerializeField] SpawnPattern[] spawnPatterns;
    [SerializeField] LayerMask spawnFilter;
    public bool TestSpawn;

    [Tooltip("LengthInMinutes")]
    [SerializeField] float startRate = 1;
    [SerializeField] float spawnRate = 1;
    private float spawnClock;

    public delegate void onBreakableChange();
    public onBreakableChange updateTargets;

    private int[] spawnableRoom;


    private void Start()
    {
        spawnClock = startRate * 60;
        SetupNagivationLists();


        #region setSpawnableRooms
        int[] tempSpawnableRooms = new int[rooms.Length];
        int useableRooms = 0;

        for (int roomSetLoop = 0; roomSetLoop < rooms.Length; roomSetLoop++) 
        {
            if (rooms[roomSetLoop].spawnLocationParent != null)
            {
                tempSpawnableRooms[useableRooms] = roomSetLoop;
                useableRooms += 1;
            }
        }

        spawnableRoom = new int[useableRooms];

        if (useableRooms != 0)
        {
            for(int spawnSetLoop = 0; spawnSetLoop < spawnableRoom.Length; spawnSetLoop++)
            {
                spawnableRoom[spawnSetLoop] = tempSpawnableRooms[spawnSetLoop];
            }
        }
        else
        {
            Debug.LogWarning("No Spawnable Location");
        }
        #endregion

        
    }

    private void Update()
    {
        if (TestSpawn)
        {
            Spawn();
            TestSpawn = false;
        }

        #region SpawnClock
        if (spawnClock > spawnRate * 60)
        {
            Spawn();
            spawnClock = 0;
        }
        else
        {
            spawnClock += 1 * Time.deltaTime;
        }

        #endregion
    }
    #region AiNavigation
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

                int highScorer = typeBreakable[eachDoor].GiveValue(type); //set check as min orrginal door
                //Debug.Log("door " + eachDoor + " score of " + highScorer);


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


                //int highScorer = typeValues[eachDoor]; //set check as min orrginal door
                int placement = eachDoor; //get placement form orignal door
                //Debug.Log("door " + eachDoor + " placement = " + placement + " score of " + highScorer);



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



                //Debug.Log("door " + eachDoor + " placement = " + placement + " score of " + highScorer);
                if (eachDoor != placement) //don't do anything if door = placement data allready set
                {
                    int[] neighbouringDoors = allDoors[eachDoor].neighbooringDoors;
                    if (neighbouringDoors[placement] == 1) //asign target door if it direct neighbour already
                    {
                        allDoors[eachDoor].AsisgnTarget(type, allDoors[placement], typeValues[placement] - (neighbouringDoors[placement] * sensorDeperation));
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
                else
                {
                    allDoors[eachDoor].AsisgnTarget(type, allDoors[placement], highScorer);
                }
                

            }
        }
    }
    #endregion
    #region UpdateNavigation
    public void UpdateNavigation()
    {
        SetupNagivationLists();
        updateTargets.Invoke();

    }
    #endregion

    #region Spawn System
    public void Spawn()
    {

        SpawnPattern pickSpawnPartern = PickSpawn();
        if (pickSpawnPartern == null)
        {
            Debug.LogWarning("no spawn pattern available");
            return;
        }
        GameObject[] spawnObjects = pickSpawnPartern.spawnObjects;
        int[] numberOfSpawns = pickSpawnPartern.numberOfSpawns;


        if (numberOfSpawns.Length == 0 || numberOfSpawns.Length != spawnObjects.Length) //temp for testing
        {
            Debug.LogWarning(pickSpawnPartern.name + "object and spawns don't have the same amount of catergargies" );
            return;
        }
        //collect rooms
        int pickRoom = spawnableRoom[Random.Range(0, spawnableRoom.Length - 1)];

        Transform[] spawnLocations = rooms[pickRoom].spawnLocations;

        
        //values set to useable spawn locations
        Vector2 areaSize = new Vector2(1, 1);
        List<int> useableLocations = new List<int>(); //use list as able to remove possible options for random generator

        for (int spawnLoopCheck = 0; spawnLoopCheck < spawnLocations.Length; spawnLoopCheck++) //find all Avialable spawn locations
        {
            Vector2 tempLocation = new Vector2(spawnLocations[spawnLoopCheck].position.x, spawnLocations[spawnLoopCheck].position.y);

            if (!Physics2D.OverlapBox(tempLocation, areaSize, 0, spawnFilter))
            {
                useableLocations.Add(spawnLoopCheck);
                //Debug.Log("adding " + spawnLoopCheck );
            }

            //Debug.Log("Loop" + spawnLoopCheck + " has count" + useableLocations.Count);
        }

        if (useableLocations.Count <= 0)
        {
            Debug.Log("No spawnable Location in " + rooms[pickRoom].gameObject.name + " spawn location Length" + spawnLocations.Length);
            return;
        }

        int spawnableSize = 0;

        for (int setSpawnSizeLoop = 0; setSpawnSizeLoop < numberOfSpawns.Length; setSpawnSizeLoop++)
        {
            spawnableSize += numberOfSpawns[setSpawnSizeLoop];
        }

        if (spawnableSize >= useableLocations.Count)
        {
            spawnableSize = useableLocations.Count - 1;
        }

        for (int spawnLoop = 0; spawnLoop < spawnObjects.Length; spawnLoop++)
        {
            if (useableLocations.Count > 0 && numberOfSpawns[spawnLoop] != 0 && spawnObjects[spawnLoop] != null)
            {
                for (int spawnCopy = 0; spawnCopy < numberOfSpawns[spawnLoop]; spawnCopy++)
                {
                    if (useableLocations.Count > 0)
                    {
                        int placement = Random.Range(0, useableLocations.Count - 1);
                        int location = useableLocations[placement];
                        useableLocations.Remove(location);
                        GameObject refence = Instantiate(spawnObjects[spawnLoop], spawnLocations[location].position, Quaternion.identity);
                        GridMovement emenyScript = refence.GetComponent<GridMovement>();
                        if (emenyScript != null)
                        {
                            emenyScript.SpawnObject(rooms[pickRoom]);
                        }
                    }
                }
            }
        }

        SpawnPattern PickSpawn()
        {
            if (spawnPatterns.Length == 0)
            {
                return null;
            }

            return spawnPatterns[Random.Range(0, spawnPatterns.Length - 1)];
        }







    }

    #endregion
    #region GameOverVictory&Reload
    public void GameOver()
    {
        if (!winConditionMet)
        {
            LevelUIControl.instance.LoadGameOverScreen();
            Debug.Log("GameOver You Lost");
            winConditionMet = true;
        }
    }

    public void Victory()
    {
        if (!winConditionMet)
        {
            LevelUIControl.instance.LoadVictoryScreen();
            Debug.Log("GameOver You Won");
            winConditionMet = true;
        }
        
    }

    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    #endregion








}
