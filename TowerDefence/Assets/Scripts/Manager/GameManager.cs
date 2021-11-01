using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    
    [Header("Enemy Spawn Points")]
    [SerializeField] Transform spawnPoint;
    
    
    [Header("Enemy Types")]
    [SerializeField] Enemy [] enemies;
    
    [Header("Enemy Parents")]
    [SerializeField] GameObject deadEnemies;
    [SerializeField] GameObject aliveEnemies;

    [Header("UI Elements")]
    [SerializeField] Image healthBar;
    [SerializeField] Text balanceTxt;
    [SerializeField] GameObject buildPrompt;

    [Header("Build Plate Objects and Menu")]
    [SerializeField] BuildPlate [] buildPlates;
    [SerializeField] BuildMenu buildMenu;
    [Header("Camera Positions")]
    [SerializeField] Transform topDownCamPosition;
    [SerializeField] Transform firstPersonCamPosition;
    [SerializeField] Transform playerCamera;

    [Header("Player Reference")]
    [SerializeField] Transform player;
    [SerializeField] Transform mainTower;

    [Header("Game Constants")]
    [SerializeField] float repairCost = 50;
    [Range(0, 1)][SerializeField] float sellPercentage = 0.8f;
    [Range(0, 1)][SerializeField] float enemySlowSpeed = 0.5f;
    [Range(0, 1)][SerializeField] float HelthIndicatorThrsh = 0.9f;
    [SerializeField] float enemyIceDuration = 3f;
    [SerializeField] float towerHealth = 10f;
    [SerializeField] int balance = 200;
    
    float totalHealth;
    bool firstPerson;

    [Header("Level number")]
    [SerializeField] int stage = 0;

    

    public int Balance { get => balance; set => balance = value; }
    public float SellPercentage { get => sellPercentage; set => sellPercentage = value; }
    public GameObject BuildPrompt { get => buildPrompt; set => buildPrompt = value; }
    public bool FirstPerson { get => firstPerson; set => firstPerson = value; }
    public float RepairCost { get => repairCost; set => repairCost = value; }
    public float EnemySlowSpeed { get => enemySlowSpeed; set => enemySlowSpeed = value; }
    public float EnemyIceDuration { get => enemyIceDuration; set => enemyIceDuration = value; }
    public Transform PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public float HelthIndicatorThrsh1 { get => HelthIndicatorThrsh; set => HelthIndicatorThrsh = value; }
    public Transform MainTower { get => mainTower; set => mainTower = value; }
    public int Stage { get => stage; set => stage = value; }


    // Start is called before the first frame update
    void Start()
    {
        firstPerson = true;
        totalHealth = towerHealth;
        UpdateBalanceText();
        if(stage == 0){
            PlayerPrefs.SetInt("Balance", balance);
        }
        else if(stage > 0){
            balance = PlayerPrefs.GetInt("Balance");
        }
        
        //Debug.Log("Dead enemies " + deadEnemies.GetComponentsInChildren<Transform>().Length + deadEnemies.GetComponentsInChildren<Transform>()[0].gameObject.name);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.M)){
            SpawnEnemy(enemies[2]);
        }
        

        if(Input.GetKeyDown(KeyCode.E)){
            SwitchCamera();
        }
        

        /*
        if(Input.GetKeyDown(KeyCode.L)){
            for(int i = 0; i < buildPlates.Length; i++){
                if(buildPlates[i].BuildIndex > 0){
                    buildPlates[i].TakeDamage(1);
                    Debug.Log("damage taken");
                }
            }
        }
        */
    }

    public void SpawnEnemy(Enemy enemy){
        Vector3 position = spawnPoint.position;
        EnemyHealth [] dead = deadEnemies.GetComponentsInChildren<EnemyHealth>();
        if(DeadEnemyOfType(enemy)){
            ReviveEnemyOfType(enemy, position);
        }
        else{
            Instantiate(enemy.prefab, position, Quaternion.identity, aliveEnemies.transform);
        }
        

    }

    public void DamageTower(float amount){
        towerHealth -= amount;
        healthBar.fillAmount = towerHealth/totalHealth;
        if(towerHealth <= 0){
            //GameOver
            SceneController.instance.LoadGameOver();
        }
    }

    public void Purchase(int cost){
        balance -= cost;
        PlayerPrefs.SetInt("Balance", balance);
        UpdateBalanceText();
    }

    public void AddBalance(int amount){
        balance += amount;
        PlayerPrefs.SetInt("Balance", balance);
        UpdateBalanceText();
        buildMenu.UpdateButtons();
    }

    void UpdateBalanceText(){
        balanceTxt.text = "Balance: " + Balance.ToString();
    }

  

    bool DeadEnemyOfType(Enemy enemy){
        EnemyHealth [] enemies = deadEnemies.GetComponentsInChildren<EnemyHealth>(); 
        for(int loop = 1; loop < enemies.Length; loop++){
            if(enemies[loop].Enemy = enemy){
                return true;
            }
        }
        return false;
    }
    
    void ReviveEnemyOfType(Enemy enemy, Vector3 position){
       EnemyHealth [] enemies = deadEnemies.GetComponentsInChildren<EnemyHealth>(); 
        for(int loop = 1; loop < enemies.Length; loop++){
            if(enemies[loop].Enemy = enemy){
                enemies[loop].Revive(position);
                break;
            }
        } 
    }

    public void SwitchCamera(){
        if(firstPerson){
            playerCamera.SetParent(topDownCamPosition);
            playerCamera.localPosition = Vector3.zero;
            playerCamera.localRotation = Quaternion.Euler(0,0,0);
            player.gameObject.GetComponent<FirstPersonAIO>().DisableCamera();
            player.gameObject.GetComponent<FirstPersonAIO>().playerCanMove = false;
            firstPerson = false;

        }
        else{
            firstPerson = true;
            playerCamera.SetParent(firstPersonCamPosition);
            playerCamera.localPosition = Vector3.zero;
            playerCamera.localRotation = Quaternion.Euler(0,0,0);
            player.gameObject.GetComponent<FirstPersonAIO>().EnableCamera();
            player.gameObject.GetComponent<FirstPersonAIO>().playerCanMove = true;

        }
        
    }


}
