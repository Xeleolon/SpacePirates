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
    [Tooltip("control variable for the like hood of reboting need an extra hit")]
    [Range (0,1)]
    [SerializeField] float reboutFalueChance = 0;
    [SerializeField] Sprite destoryedSprite;
    [SerializeField] Sprite damagedSprite;
    private SpriteRenderer spriteRenderer;
    private Sprite fuctionalSprite;
   
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            
            fuctionalSprite = spriteRenderer.sprite;
        }
        currentHeath = health;
    }
    public void SetRoom(RoomIndex newRoom)
    {
        currentRoom = newRoom;
    }

    private int extraDamage = 0; // variable in charge of potional ship needing a few more hits to activate
    
    public bool AlterHealth(int alter)
    {
        currentHeath += alter;
        LevelUIControl.instance.RoomDamage(currentRoom, alter);
        if (currentHeath <= 0)
        {
            currentHeath = extraDamage;
            Broken();
            return false;
        }

        if (currentHeath >= health)
        {
            currentHeath = health;
            //damage = false;
            //broken = false;
            Repaired();
        }
        else if (currentHeath < health)
        {
            
            Damage();
            
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
        

            if (destoryedSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = destoryedSprite;
            }
        }
    }
    public void Damage()
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

        if (damagedSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = damagedSprite;
        }
    }
    public void Repaired()
    {
        Debug.Log("attemping repair for " + gameObject.name);
        if (broken || damage)
        {
            onFixEvent.Invoke(gameObject);
            for (int attranctLoop = 0; attranctLoop < attractants.Count; attranctLoop++)
            {
                attractants[attranctLoop].SetAttranct();
            }
            LevelManager.instance.UpdateNavigation();

            if (fuctionalSprite != null)
            {
                Debug.Log(gameObject.name + "activating it repair icon");
                spriteRenderer.sprite = fuctionalSprite;
            }
            broken = false;
            damage = false;

            if (Random.Range(0, 1) < reboutFalueChance) //cause rebot falue
            {
                extraDamage = 0;
            }
            else
            {
                extraDamage = -1;
            }
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
