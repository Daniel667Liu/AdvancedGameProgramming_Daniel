using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    [SerializeField]
    private AIController aiPrefab;
    [SerializeField]
    private collectableItems itemPrefab;
    public GameObject[] aiSpawnPoint;
    public GameObject[] itemSpawnPoint;
    [HideInInspector]
    public bool is_AI_Tracking=true;
    // Start is called before the first frame update
    private void Awake()
    {
        Service.ServiceInitialize();
        Service.gameManager = this;
        Service.ServiceStart();
        
    }

    void Start()
    {
        
        spawnItem();
        spawnAI();
    }

    // Update is called once per frame
    void Update()
    {
        
        Service.inputManager.UpdateManual();
        Service.collectableManager.UpdateManual();

        if (is_AI_Tracking)
        {

            Service.aiManager.UpdateManual();
        }

        }

        void spawnAI() //spawn ai at every point
    {
        
        for (int i = 0; i < aiSpawnPoint.Length; i++) 
        {
            AIController spawnedAI ;
            spawnedAI = Instantiate(aiPrefab, aiSpawnPoint[i].transform) as AIController;//spawn the ai
            Service.aiManager.AIs.Add(spawnedAI);//add spawned ai into the list in AImanager
        }
    }

    void spawnItem() 
    {
       
        for (int i = 0; i < itemSpawnPoint.Length; i++) 
        {
            collectableItems spawnedItem ;
            spawnedItem = Instantiate(itemPrefab, itemSpawnPoint[i].transform) as collectableItems;//spawn the item
            Service.collectableManager.itemsList.Add(spawnedItem);//add spawned itmes into list in collectableManager
        }
    }
}
