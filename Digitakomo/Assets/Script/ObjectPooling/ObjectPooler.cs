using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance = null;

    // Class to store the Pooling
    [System.Serializable]
    public class PoolerTemplate
    {
        
        public GameObject prefabToCreate = null;
        public int amountToCreate = 10;
        public int amountToAdd = 5;

        [System.NonSerialized]
        // Key to identify the Object
        public string key = "";
        [System.NonSerialized]
        // Own list to store which objects have been created
        public List<GameObject> listOfCreatedObjects = new List<GameObject>();
    }
    // List to store all the Pools IN THE EDITOR
    public List<PoolerTemplate> listOfPools = new List<PoolerTemplate>();
    // Dictionary to store all the Pools IN THE ACTUAL GAME
    Dictionary<string, PoolerTemplate> dictionaryOfPools = new Dictionary<string, PoolerTemplate>();


    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Transfer from list to dictionary
        foreach (PoolerTemplate item in listOfPools)
        {
            // Set the Key
            item.key = item.prefabToCreate.name;
            // Add into Dictionary
            dictionaryOfPools.Add(item.key, item);

            // Create all the objects
            for (int i = 0; i < dictionaryOfPools[item.key].amountToCreate; ++i)
            {
                GameObject newObj = Instantiate(dictionaryOfPools[item.key].prefabToCreate);
                newObj.SetActive(false);
                // Add to own list to keep track
                dictionaryOfPools[item.key].listOfCreatedObjects.Add(newObj);
            }
        }
    }

    public GameObject FetchGO(string newKey)
    {
        // Does key exists?
        if (!dictionaryOfPools.ContainsKey(newKey))
            return null;


        // Loop through array and find what inactive 
        for (int i = 0; i < dictionaryOfPools[newKey].listOfCreatedObjects.Count; ++i)
        {
            if (dictionaryOfPools[newKey].listOfCreatedObjects[i].activeSelf == false)
            {
                dictionaryOfPools[newKey].listOfCreatedObjects[i].SetActive(true);
                return dictionaryOfPools[newKey].listOfCreatedObjects[i];
            }

        }
        // Create more
        for (int i = 0; i < dictionaryOfPools[newKey].amountToAdd; ++i)
        {
            GameObject newObj = Instantiate(dictionaryOfPools[newKey].prefabToCreate);
            newObj.SetActive(false);
            dictionaryOfPools[newKey].listOfCreatedObjects.Add(newObj);
        }

        // Return last Object
        int maxCount = dictionaryOfPools[newKey].listOfCreatedObjects.Count;
        dictionaryOfPools[newKey].listOfCreatedObjects[maxCount - 1].SetActive(true);
        return dictionaryOfPools[newKey].listOfCreatedObjects[maxCount - 1];
    }
}
