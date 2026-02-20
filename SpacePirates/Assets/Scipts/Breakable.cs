using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class Breakable : MonoBehaviour
{
    [System.Serializable]
    public class OnBrokeEvent : UnityEvent<GameObject>
    {
        public GameObject gameObject;
    }
    [System.Serializable]
    public class OnFixEvent : UnityEvent<GameObject>
    {
        public GameObject gameObject;
    }



    public int health = 1;
    int currentHeath;
   
    public List<Attranctiveness> attractants = new List<Attranctiveness>();

    [SerializeField] OnBrokeEvent onBrokeEvent = new OnBrokeEvent();
    [SerializeField] OnFixEvent onFixEvent = new OnFixEvent();
    //Sprite[] sprite;

    bool broken = false;
    bool damage = false;
    RoomIndex currentRoom;

    private void OnValidate()
    {
        if (attractants.Count > 0)
        {
            for (int i = 0; i < attractants.Count; i++)
            {

                attractants[i].SetNames();
            }
        }
    }

    void Start()
    {
        if (attractants.Count > 0)
        {
            for (int i = 0; i < attractants.Count; i++)
            {
                attractants[i].SetAttranct();
            }
        }

        currentHeath = health;
    }
    public void SetRoom(RoomIndex newRoom)
    {
        currentRoom = newRoom;
    }
    
    public bool AlterHealth(int alter)
    {
        currentHeath += alter;
        LevelUIControl.instance.RoomDamage(currentRoom, alter);
        if (currentHeath <= 0)
        {
            Broken();
            return false;
        }

        Repaired();

        if (currentHeath >= health)
        {
            currentHeath = health;
            damage = false;
            broken = false;
        }
        else if (currentHeath < health)
        {
            damage = true;
            if (currentHeath <= 0)
            {
                currentHeath = 0;
                broken = true;
            }
            else
            {
                broken = false;
            }
        }
        return true;
    }

    public void Broken()
    {
        if (!broken)
        {
            onBrokeEvent.Invoke(gameObject);
            //Asign broken object
            for (int attranctLoop = 0; attranctLoop < attractants.Count; attranctLoop++)
            {
                attractants[attranctLoop].value = 0;
            }
            LevelManager.instance.UpdateNavigation();
            broken = true;
        }
    }
    public void Repaired()
    {
        if (broken)
        {
            onFixEvent.Invoke(gameObject);
            for (int attranctLoop = 0; attranctLoop < attractants.Count; attranctLoop++)
            {
                attractants[attranctLoop].SetAttranct();
            }
            LevelManager.instance.UpdateNavigation();
            broken = false;
        }
    }

    public bool CheckAttractanctMatch(Attractant matchCheck)
    {
        if (attractants.Count <= 0)
        {
            return false;
        }

        if (attractants.Exists(i => i.attractant == matchCheck))
        {
            return true;
        }


        return false;
    }
    public Attranctiveness returnAttractantValues(Attractant type)
    {

        if (!attractants.Exists(i => i.attractant == type))
        {
            return null;
        }

        return attractants.Find(i => i.attractant == type);
    }

    public int GiveValue(Attractant type)
    {

        if (!attractants.Exists(i => i.attractant == type))
        {
            return 0;
        }

        Attranctiveness temp = attractants.Find(i => i.attractant == type);
        return temp.value;
    }
}
