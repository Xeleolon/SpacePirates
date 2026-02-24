using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Instance/Awake
    public static GameManager instance;

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

    public RoomData[] shipRooms;




}
