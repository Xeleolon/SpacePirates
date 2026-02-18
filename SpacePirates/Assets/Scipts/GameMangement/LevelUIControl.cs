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
    [SerializeField] float gameLength = 100;
    [Range(0, 20)]
    [SerializeField] float progressSpeed = 1;
    private float currentProgress;
    [SerializeField] Slider progressBar;
    [SerializeField] Gradient progressGradient;
    [SerializeField] Image progressFill;

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

    private void OnValidate()
    {
        if (shipRooms.Length <= 0)
        {
            for (int i = 0; i < shipRooms.Length; i++)
            {
                shipRooms[i].SetNames();
            }
        }
    }

    


    private void Update()
    {
        #region Progress
        currentProgress += progressSpeed * Time.deltaTime;

        if (currentProgress >= gameLength)
        {
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
                    damagedroom -= 1;

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

}
