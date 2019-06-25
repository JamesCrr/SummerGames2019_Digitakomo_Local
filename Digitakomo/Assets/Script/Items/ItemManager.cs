using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Energy Items")]
    public BaseItem[] EnergyItems;
    private int EnergyItemSpawned = 0;
    public float FirstEnergyItemSpawn = 60f;
    public float NextEnergyItemSpawn = 30f;
    private float NextEnergySpawn;

    [Header("HP Items")]
    public BaseItem[] HPItems;
    private int HPItemSpawned = 0;
    public float FirstHPItemSpawn = 60f;
    public float NextHPItemSpawn = 30f;
    private float NextHPSpawn;

    [Header("Where to spawn")]
    public SpawnItemZone[] spawnItemZones;
    // Start is called before the first frame update
    void Start()
    {
        NextEnergySpawn = FirstEnergyItemSpawn + Time.time;
        NextHPSpawn = FirstHPItemSpawn + Time.time;

        Reshuffle(EnergyItems);
        Reshuffle(HPItems);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= NextEnergySpawn)
        {
            BaseItem item = EnergyItems[EnergyItemSpawned++];
            SpawnItem(item);
            NextEnergySpawn = Time.time + NextEnergyItemSpawn;
        }
        if (Time.time >= NextHPSpawn)
        {
            BaseItem item = HPItems[HPItemSpawned++];
            SpawnItem(item);
            NextHPSpawn = Time.time + NextHPItemSpawn;
        }
    }

    private void SpawnItem(BaseItem item)
    {
        //Get Posible spawnbox

        //Random which spawnbox we use

        //Random position in spawnbox

    }

    void Reshuffle(BaseItem[] items)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < items.Length; t++)
        {
            BaseItem tmp = items[t];
            int r = Random.Range(t, items.Length);
            items[t] = items[r];
            items[r] = tmp;
        }
    }
}
