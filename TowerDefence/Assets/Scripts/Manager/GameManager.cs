using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

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
    [SerializeField] Transform FullTopDownCamera;

    [Header("Player Reference")]
    [SerializeField] Transform player;
    [SerializeField] Transform mainTower;

    [Header("Game Constants")]
    public static float repairCost = 50;
    [Range(0, 1)]public static float sellPercentage = 0.8f;
    [Range(0, 1)]public static float enemySlowSpeed = 0.5f;
    [Range(0, 1)]public static float HelthIndicatorThrsh = 0.9f;
    public static float UnlockPrice;
    public static float enemyIceDuration = 5f;
    [SerializeField] float towerHealth = 10f;
    [SerializeField] float balance = 200;
    
    float totalHealth;
    public static bool firstPerson;
    [Header("Level number")]
    [SerializeField] PathCreator lane;

    [Header("Level number")]
    [SerializeField] int stage = 0;
    [SerializeField] bool reversedPath = false;
    [SerializeField] GameObject levelComplete;

    public static bool gameOver = false;
    bool rangeIndicators;
    bool fullTopDown;

    

    public float Balance { get => balance; set => balance = value; }
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
    public PathCreator Lane { get => lane; set => lane = value; }
    public GameObject DeadEnemies { get => deadEnemies; set => deadEnemies = value; }
    public GameObject AliveEnemies { get => aliveEnemies; set => aliveEnemies = value; }
    public bool RangeIndicators { get => rangeIndicators; set => rangeIndicators = value; }


    // Start is called before the first frame update
    void Start()
    {
        firstPerson = true;
        totalHealth = towerHealth;
        UpdateBalanceText();
        gameOver = false;
        RangeIndicators = false;
        
        //Debug.Log("Dead enemies " + deadEnemies.GetComponentsInChildren<Transform>().Length + deadEnemies.GetComponentsInChildren<Transform>()[0].gameObject.name);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.M)){
            balance = 10000;
        }
        

        if(Input.GetKeyDown(KeyCode.E)){
            SwitchCamera();
            if(RangeIndicators){
                DisableRangeIndicators();
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Q) && !RangeIndicators && firstPerson){
            EnableRangeIndicators();
        }
        else if(Input.GetKeyDown(KeyCode.Q) && RangeIndicators){
            DisableRangeIndicators();
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

    public void SpawnRangedEnemy(){
        SpawnEnemy(enemies[1]);
    }
    public void CompleteLevel(){
        gameOver = true;
        levelComplete.SetActive(true);
        player.GetComponent<FirstPersonAIO>().playerCanMove = false;
        player.GetComponent<FirstPersonAIO>().DisableCamera();
    }
    public void SpawnEnemy(Enemy enemy){
        Vector3 position = lane.path.GetPoint(0);
        if(reversedPath){
            position = lane.path.GetPoint(1);
        }
        EnemyHealth [] dead = deadEnemies.GetComponentsInChildren<EnemyHealth>();
        if(DeadEnemyOfType(enemy)){
            ReviveEnemyOfType(enemy, position);
        }
        else{
            GameObject newEnemy = Instantiate(enemy.prefab, position, Quaternion.identity, aliveEnemies.transform);
            newEnemy.GetComponent<EnemyFollower>().StopAllParticles();
            if(fullTopDown){
                newEnemy.GetComponent<EnemyHealth>().Invisible();
            }
            else{
                newEnemy.GetComponent<EnemyHealth>().Visible();
            }
        }
        

    }

    public void DamageTower(float amount){
        towerHealth -= amount;
        healthBar.fillAmount = towerHealth/totalHealth;
        if(towerHealth <= 0){
            //GameOver
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneController.instance.LoadGameOver();
        }
    }

    public void Purchase(float cost){
        balance -= cost;
        UpdateBalanceText();
    }

    public void AddBalance(float amount){
        balance += amount;
        UpdateBalanceText();
        buildMenu.UpdateButtons();
    }

    void UpdateBalanceText(){
        balanceTxt.text = ""+Balance.ToString();
    }

  

    bool DeadEnemyOfType(Enemy enemy){
        bool ret = false;
        EnemyHealth [] enemies = deadEnemies.GetComponentsInChildren<EnemyHealth>();
        Debug.Log("total enemies " + enemies.Length); 
        for(int loop = 0; loop < enemies.Length; loop++){
            Debug.Log(enemy.name);
            if(enemies[loop].Enemy.name == enemy.name){
                ret = true;
            }
        }
        Debug.Log("Dead enemy of type:" + ret);
        return ret;
    }
    
    void ReviveEnemyOfType(Enemy enemy, Vector3 position){
       EnemyHealth [] enemies = deadEnemies.GetComponentsInChildren<EnemyHealth>(); 
        for(int loop = 0; loop < enemies.Length; loop++){
            if(enemies[loop].Enemy == enemy){
                enemies[loop].Revive(position);
                enemies[loop].GetComponent<EnemyFollower>().StopAllParticles();
                if(fullTopDown){
                    enemies[loop].Invisible();
                }
                else{
                    enemies[loop].Visible();
                }
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

    public void MakeEnemiesInvisible(){
        EnemyHealth [] enemies;
        enemies = aliveEnemies.GetComponentsInChildren<EnemyHealth>();
        if(enemies.Length == 0){
            return;
        }
        else{
            for(int loop = 0; loop < enemies.Length; loop++){
                enemies[loop].Invisible();
            }
        }
    }
    public void MakeEnemiesVisible(){
        EnemyHealth [] enemies;
        enemies = aliveEnemies.GetComponentsInChildren<EnemyHealth>();
        if(enemies.Length == 0){
            return;
        }
        else{
            for(int loop = 0; loop < enemies.Length; loop++){
                enemies[loop].Visible();
            }
        }
    }
    public void SwitchFullTopDown(){
        if(!fullTopDown){
            playerCamera.SetParent(FullTopDownCamera);
            playerCamera.localPosition = Vector3.zero;
            playerCamera.localRotation = Quaternion.Euler(0,0,0);
            player.gameObject.GetComponent<FirstPersonAIO>().DisableCamera();
            player.gameObject.GetComponent<FirstPersonAIO>().playerCanMove = false;
            firstPerson = false;
            fullTopDown = true;
            MakeEnemiesInvisible();

        }
        else{
            firstPerson = true;
            fullTopDown = false;
            playerCamera.SetParent(firstPersonCamPosition);
            playerCamera.localPosition = Vector3.zero;
            playerCamera.localRotation = Quaternion.Euler(0,0,0);
            player.gameObject.GetComponent<FirstPersonAIO>().EnableCamera();
            player.gameObject.GetComponent<FirstPersonAIO>().playerCanMove = true;
            MakeEnemiesVisible();

        }
        
    }
    public void EnableRangeIndicators(){
        SwitchFullTopDown();
        RangeIndicators = true;
        for(int loop = 0; loop < buildPlates.Length; loop++){
            if(buildPlates[loop].BuildIndex > 0){
                buildPlates[loop].GetComponentInChildren<TurretTargetTrigger>().EnableRangeIndicator();
            }
            else{
                continue;
            }
        }
    }

    public void DisableRangeIndicators(){
        SwitchFullTopDown();
        RangeIndicators = false;
        for(int loop = 0; loop < buildPlates.Length; loop++){
            if(buildPlates[loop].BuildIndex > 0){
                buildPlates[loop].GetComponentInChildren<TurretTargetTrigger>().DisableRangeIndicator();
            }
            else{
                continue;
            }
        }
    }


}
