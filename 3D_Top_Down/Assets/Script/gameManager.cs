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
    // Start is called before the first frame update
    private void Awake()
    {
        Service.gameManager = this;
        Service.ServiceInitialize();
        spawnAI();
        spawnItem();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Service.inputManager.UpdateManual();
        Service.playerController.UpdateManual();
        Service.animController.UpdateManual();
        Service.aiManager.UpdateManual();
        
    }

    void spawnAI() //spawn ai at every point
    {
        AIController spawnedAI = new AIController();
        for (int i = 0; i < aiSpawnPoint.Length; i++) 
        {
            spawnedAI = Instantiate(aiPrefab, aiSpawnPoint[i].transform) as AIController;//spawn the ai
            Service.aiManager.AIs.Add(spawnedAI);//add spawned ai into the list in AImanager
        }
    }

    void spawnItem() 
    {
        collectableItems spawnedItem = new collectableItems();
        for (int i = 0; i < itemSpawnPoint.Length; i++) 
        {
            spawnedItem = Instantiate(itemPrefab, itemSpawnPoint[i].transform) as collectableItems;//spawn the item
            Service.collectableManager.itemsList.Add(spawnedItem);//add spawned itmes into list in collectableManager
        }
    }
}
