using UnityEngine;


[CreateAssetMenu(fileName = "New Spawn Pattern", menuName = "Cattells/SpawnPattern")]
public class SpawnPattern : ScriptableObject
{
    public GameObject[] spawnObjects;
    public int[] numberOfSpawns;
}
