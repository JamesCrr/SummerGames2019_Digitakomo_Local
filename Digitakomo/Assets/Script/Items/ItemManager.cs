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
            BaseItem item = EnergyItems[EnergyItemSpawned++ % EnergyItems.Length];
            SpawnItem(item);
            NextEnergySpawn = Time.time + NextEnergyItemSpawn;
        }
        if (Time.time >= NextHPSpawn)
        {
            BaseItem item = HPItems[HPItemSpawned++ % HPItems.Length];
            SpawnItem(item);
            NextHPSpawn = Time.time + NextHPItemSpawn;
        }
    }

    private void SpawnItem(BaseItem item)
    {
        //Random which spawnbox we use
        int spawnbox_index = Random.Range(0, spawnItemZones.Length);

        SpawnItemZone spawnbox = spawnItemZones[spawnbox_index];
        
        //Random position in spawnbox
        float x = Random.Range(0, (int)spawnbox.size.x) - (spawnbox.size.x / 2);
        float y = Random.Range(0, (int)spawnbox.size.y) - (spawnbox.size.y / 2);

        Vector3 spawnPosition = spawnbox.transform.localPosition + spawnbox.center + new Vector3(x, y, 0);
        BaseItem i = ObjectPooler.Instance.FetchGO(item.name).GetComponent<BaseItem>();
        i.transform.position = spawnPosition;
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
