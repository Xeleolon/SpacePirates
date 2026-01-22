using UnityEngine;
using UnityEngine.UI;

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
    [Header("HealthUI")]
    [SerializeField] Transform healthBarParent;





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
    }
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

}
