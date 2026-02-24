using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUIControl : MonoBehaviour
{
    #region Instance/Awake
    public static LevelUIControl instance;

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
    [SerializeField] bool test;

    [Header("ProgressBar/GameLength")]
    private float gameLength = 100;
    [Range(0, 20)]
    [SerializeField] float progressSpeed = 1;
    private float currentProgress;
    [SerializeField] Slider progressBar;
    [SerializeField] Gradient progressGradient;
    [SerializeField] Image progressFill;

    bool engineDamage;

    [Header("Room Damage")]
    [SerializeField] RoomData[] shipRooms;
    [SerializeField] Color roomUnDamaged;
    [SerializeField] Gradient roomDamaged;
    [SerializeField] Color roomDead;
    private int availableRooms;
    private int damagedroom;

    [Header("HealthUI")]
    [SerializeField] Transform healthBarParent;
    [SerializeField] GameObject statusAlive;
    [SerializeField] TMP_Text respawnNumbers;

    [Header("Respawn")]
    [SerializeField] float respawnTime = 1;
    private float respawnClock;
    private GameObject player;
    bool playerDied;
    [Header("Victory&DefeatUI")]
    [SerializeField] GameObject respawnScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject victoryScreen;

    [Header("Broken Event")]
    [SerializeField] GameObject engineDamageIcon;
    [SerializeField] GameObject thrusterPluse;

    private void OnValidate()
    {
        if (shipRooms.Length > 0)
        {
            for (int i = 0; i < shipRooms.Length; i++)
            {
                
                shipRooms[i].SetNames();
            }
        }
    }
    private void Start()
    {
        gameLength = LevelManager.instance.gameLength * 60;

        if (respawnScreen.activeSelf)
        {
            respawnScreen.SetActive(false);
        }
    }

    public void LoadGameOverScreen()
    {
        if (!respawnScreen.activeSelf)
        {
            respawnScreen.SetActive(true);
        }

        if (!gameOverScreen.activeSelf)
        {
            gameOverScreen.SetActive(true);
        }

        if (victoryScreen.activeSelf)
        {
            victoryScreen.SetActive(false);
        }
    }

    public void LoadVictoryScreen()
    {
        if (!respawnScreen.activeSelf)
        {
            respawnScreen.SetActive(true);
        }

        if (!victoryScreen.activeSelf)
        {
            victoryScreen.SetActive(true);
        }

        if (gameOverScreen.activeSelf)
        {
            gameOverScreen.SetActive(false);
        }
    }

    


    private void Update()
    {
        #region Progress
        if (!engineDamage)
        {
            currentProgress += progressSpeed * Time.deltaTime;
        }

        if (currentProgress >= gameLength)
        {
            LevelManager.instance.Victory();
            Debug.Log("Victory game one");
        }
        else
        {
            float progress = Mathf.Clamp01(currentProgress/gameLength);
            progressBar.value = progress;
            progressFill.color = progressGradient.Evaluate(progress);
        }

        if (test)
        {
            test = false;
            //test function;
            ChangeHeath(3);
        }


        #endregion
        if (playerDied)
        {
            if (respawnClock >= respawnTime)
            {
                if (player != null)
                {
                    Respawn();
                }
            }
            else
            {
                respawnClock += 1 * Time.deltaTime;
                float tempClock = Mathf.Round((respawnTime - respawnClock) * 10) / 10;
                respawnNumbers.SetText(tempClock.ToString());
            }
        }
    }
    #region PlayerHealthAlternation
    public void ChangeHeath(int healthToken)
    {
        Debug.Log("test cool" + healthToken);
        if (healthToken > 0)
        {
            GameObject changeToken = healthBarParent.GetChild(healthToken - 1).gameObject;
            Debug.Log("altering health token " + changeToken.name);
            if (changeToken.activeSelf == true)
            {
                changeToken.SetActive(false);
            }
            else
            {
                changeToken.SetActive(true);
            }
        }
    }
    public void PlayerDied(GameObject newPLayer)
    {
        playerDied = true;
        if (player == null)
        {
            player = newPLayer;
        }

        respawnClock = 0;
        respawnNumbers.SetText(respawnClock.ToString());
        if (statusAlive.activeSelf)
        {
            statusAlive.SetActive(false);
        }

        if (player.activeSelf)
        {
            player.SetActive(false);
        }

    }

    private void Respawn()
    {
        playerDied = false;
        player.GetComponent<Player>().RespawnHealth();
        if (!statusAlive.activeSelf)
        {
            statusAlive.SetActive(true);
        }
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
    }
    #endregion
    #region RoomHealth
    public void SetRoomHealth(RoomIndex room, int totalHealth)
    {
        for (int roomLoop = 0; roomLoop < shipRooms.Length; roomLoop++)
        {
            if (shipRooms[roomLoop].roomIndex == room)
            {
                shipRooms[roomLoop].SetSystems(totalHealth);
                shipRooms[roomLoop].damageIcon.color = roomUnDamaged;

                if (totalHealth > 0)
                {
                    availableRooms += 1;
                    damagedroom += 1;
                }
                return;
            }
        }
    }
    public void RoomDamage(RoomIndex room, int damage)
    {
        if (shipRooms.Length == 0)
        {
            Debug.LogWarning("No Ship Room assigned to UI");
            return;
        }
        for (int roomLoop = 0; roomLoop < shipRooms.Length; roomLoop ++)
        {
            if (shipRooms[roomLoop].roomIndex == room)
            {
                float damagePercent = shipRooms[roomLoop].DamageSystem(damage);
                //Debug.Log("room " + (roomLoop + 1) + " has a damage percent of " + damagePercent);
                if (damagePercent <= 0)
                {
                    shipRooms[roomLoop].damageIcon.color = roomDead;
                    if (shipRooms[roomLoop].damaged != true)
                    {
                        damagedroom -= 1;

                        shipRooms[roomLoop].damaged = true;
                    }

                    if (damagedroom <= 0)
                    {
                        Debug.Log("GameOver");
                    }
                }
                else if (damagePercent >= 1)
                {
                    shipRooms[roomLoop].damageIcon.color = roomUnDamaged;
                }
                else
                {
                    shipRooms[roomLoop].damageIcon.color = roomDamaged.Evaluate(damagePercent);
                }
                return;
            }
        }
    }
    #endregion
    public void EngineDamage(bool damagedStatus)
    {
        engineDamage = damagedStatus;

        if (engineDamageIcon != null)
        {
            if (engineDamage && !engineDamageIcon.activeSelf)
            {
                engineDamageIcon.SetActive(true);
            }
            else if (!engineDamage && engineDamageIcon.activeSelf)
            {
                engineDamageIcon.SetActive(false);
            }

        }
        if (thrusterPluse != null)
        {
            if (!engineDamage && !thrusterPluse.activeSelf)
            {
                thrusterPluse.SetActive(true);
            }
            else if (engineDamage && thrusterPluse.activeSelf)
            {
                thrusterPluse.SetActive(false);
            }

        }
    }

}
