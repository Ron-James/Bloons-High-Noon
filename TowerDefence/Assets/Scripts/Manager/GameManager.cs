using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

public class GameManager : Singleton<GameManager>
{
    public enum View
    {
        firstPerson = 0,
        objective = 1,
        topDown = 2
    }

    [Header("Enemy Spawn Points")]
    [SerializeField] Transform spawnPoint;


    [Header("Enemy Types")]
    [SerializeField] Enemy[] enemies;

    [Header("Enemy Parents")]
    [SerializeField] GameObject deadEnemies;
    [SerializeField] GameObject aliveEnemies;

    [Header("UI Elements")]
    [SerializeField] Image healthBar;
    [SerializeField] Text balanceTxt;
    [SerializeField] GameObject viewPrompt;

    [SerializeField] GameObject buildPrompt;

    [Header("Build Plate Objects and Menu")]
    [SerializeField] BuildPlate[] buildPlates;
    [SerializeField] BuildMenu buildMenu;
    [Header("Camera Positions")]
    [SerializeField] Transform topDownCamPosition;
    [SerializeField] Transform firstPersonCamPosition;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform FullTopDownCamera;
    [SerializeField] GameObject topDownLane;

    [Header("Player Reference")]
    [SerializeField] Transform player;
    [SerializeField] Transform mainTower;
    [SerializeField] GameObject playerPin;

    [Header("Game Constants")]
    public static float repairCost = 50;
    [Range(0, 1)] public static float sellPercentage = 0.8f;
    [Range(0, 1)] public static float enemySlowSpeed = 0.5f;
    [Range(0, 1)] public static float HelthIndicatorThrsh = 0.9f;
    public static float UnlockPrice;
    public static float enemyIceDuration = 5f;
    [SerializeField] float towerHealth = 10f;

    [SerializeField] float balance = 200;

    float totalHealth;

    [Header("Lane")]
    [SerializeField] PathCreator lane;

    [Header("Level number")]
    [SerializeField] int stage = 0;
    [SerializeField] bool reversedPath = false;
    [SerializeField] GameObject levelComplete;
    [SerializeField] View view = View.firstPerson;
    [Header("Tower Damage")]
    [SerializeField] GameObject[] damageEffects;
    [Header("Tutorial things")]
    [SerializeField] bool tutorial = false;

    public static bool gameOver = false;
    bool rangeIndicators;


    public float Balance { get => balance; set => balance = value; }
    public float SellPercentage { get => sellPercentage; set => sellPercentage = value; }
    public GameObject BuildPrompt { get => buildPrompt; set => buildPrompt = value; }

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
    public int ViewIndex { get => (int)view; set => view = (View)value; }


    // Start is called before the first frame update
    void Start()
    {
        playerPin = GameObject.Find("PlayerPin");
        playerPin.SetActive(false);
        totalHealth = towerHealth;
        UpdateBalanceText();
        gameOver = false;
        RangeIndicators = false;
        Time.timeScale = 1;
        DisableTowerEffects();

        //Debug.Log("Dead enemies " + deadEnemies.GetComponentsInChildren<Transform>().Length + deadEnemies.GetComponentsInChildren<Transform>()[0].gameObject.name);
        if(stage == 0)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && !tutorial)
        {
            CycleViews();

        }

    }

    public void SpawnRangedEnemy()
    {
        SpawnEnemy(enemies[1]);
    }
    public void CompleteLevel()
    {
        gameOver = true;
        levelComplete.SetActive(true);
        player.GetComponent<FirstPersonAIO>().playerCanMove = false;
        player.GetComponent<FirstPersonAIO>().DisableCamera();
    }
    public void SpawnEnemy(Enemy enemy)
    {
        Vector3 position = lane.path.GetPoint(0);
        if (reversedPath)
        {
            position = lane.path.GetPoint(1);
        }
        EnemyHealth[] dead = deadEnemies.GetComponentsInChildren<EnemyHealth>();
        if (DeadEnemyOfType(enemy))
        {
            ReviveEnemyOfType(enemy, position);
        }
        else
        {
            GameObject newEnemy = Instantiate(enemy.prefab, position, Quaternion.identity, aliveEnemies.transform);
            newEnemy.GetComponent<EnemyFollower>().StopAllParticles();
            if (view == View.topDown)
            {
                newEnemy.GetComponent<EnemyHealth>().Invisible();
            }
            else
            {
                newEnemy.GetComponent<EnemyHealth>().Visible();
            }
        }


    }

    public void DamageTower(float amount)
    {
        towerHealth -= amount;
        healthBar.fillAmount = towerHealth / totalHealth;
        if (!tutorial)
        {
            if (towerHealth < (0.9f * totalHealth) && damageEffects.Length > 0)
            {
                damageEffects[0].SetActive(true);
            }
            if (towerHealth < (0.75f * totalHealth) && damageEffects.Length > 1)
            {
                damageEffects[1].SetActive(true);
            }
            if (towerHealth < (0.5f * totalHealth) && damageEffects.Length > 2)
            {
                damageEffects[2].SetActive(true);
            }
        }

        if (towerHealth <= 0)
        {
            //GameOver
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameOver = true;
            SceneController.instance.LoadGameOver();
        }
    }
    public void DisableTowerEffects()
    {
        for (int loop = 0; loop < damageEffects.Length; loop++)
        {
            damageEffects[loop].SetActive(false);
        }
    }

    public void Purchase(float cost)
    {
        balance -= cost;
        UpdateBalanceText();
    }

    public void AddBalance(float amount)
    {
        balance += amount;
        UpdateBalanceText();
        buildMenu.UpdateButtons();
    }

    void UpdateBalanceText()
    {
        balanceTxt.text = "" + Balance.ToString();
    }



    bool DeadEnemyOfType(Enemy enemy)
    {
        bool ret = false;
        EnemyHealth[] enemies = deadEnemies.GetComponentsInChildren<EnemyHealth>();
        Debug.Log("total enemies " + enemies.Length);
        for (int loop = 0; loop < enemies.Length; loop++)
        {
            Debug.Log(enemy.name);
            if (enemies[loop].Enemy.name == enemy.name)
            {
                ret = true;
            }
        }
        Debug.Log("Dead enemy of type:" + ret);
        return ret;
    }

    void ReviveEnemyOfType(Enemy enemy, Vector3 position)
    {
        EnemyHealth[] enemies = deadEnemies.GetComponentsInChildren<EnemyHealth>();
        for (int loop = 0; loop < enemies.Length; loop++)
        {
            if (enemies[loop].Enemy == enemy)
            {
                enemies[loop].Revive(position);
                enemies[loop].GetComponent<EnemyFollower>().StopAllParticles();
                if (view == View.topDown)
                {
                    enemies[loop].Invisible();
                }
                else
                {
                    enemies[loop].Visible();
                }
                break;
            }
        }
    }


    public void FullTopDownView()
    {
        topDownLane.SetActive(true);
        playerPin.SetActive(true);
        view = View.topDown;
        playerCamera.SetParent(FullTopDownCamera);
        playerCamera.localPosition = Vector3.zero;
        playerCamera.localRotation = Quaternion.Euler(0, 0, 0);
        player.gameObject.GetComponent<FirstPersonAIO>().DisableCamera();
        player.gameObject.GetComponent<FirstPersonAIO>().playerCanMove = false;
        MakeEnemiesInvisible();
        EnableRangeIndicators();
    }
    public void ObjectiveView()
    {
        playerPin.SetActive(true);
        topDownLane.SetActive(false);
        view = View.objective;
        playerCamera.SetParent(topDownCamPosition);
        playerCamera.localPosition = Vector3.zero;
        playerCamera.localRotation = Quaternion.Euler(0, 0, 0);
        player.gameObject.GetComponent<FirstPersonAIO>().DisableCamera();
        player.gameObject.GetComponent<FirstPersonAIO>().playerCanMove = false;
        MakeEnemiesVisible();
        DisableRangeIndicators();

    }

    public void FirstPersonView()
    {
        playerPin.SetActive(false);
        topDownLane.SetActive(false);
        view = View.firstPerson;
        playerCamera.SetParent(firstPersonCamPosition);
        playerCamera.localPosition = Vector3.zero;
        playerCamera.localRotation = Quaternion.Euler(0, 0, 0);
        player.gameObject.GetComponent<FirstPersonAIO>().EnableCamera();
        player.gameObject.GetComponent<FirstPersonAIO>().playerCanMove = true;
        MakeEnemiesVisible();
        DisableRangeIndicators();
    }
    public void CycleViews()
    {
        int current = ViewIndex;
        switch (current)
        {
            case 0:
                ObjectiveView();
                break;
            case 1:
                FullTopDownView();
                break;
            case 2:
                FirstPersonView();
                viewPrompt.SetActive(false);
                break;
        }
    }

    public void MakeEnemiesInvisible()
    {
        EnemyHealth[] enemies;
        enemies = aliveEnemies.GetComponentsInChildren<EnemyHealth>();
        if (enemies.Length == 0)
        {
            return;
        }
        else
        {
            for (int loop = 0; loop < enemies.Length; loop++)
            {
                enemies[loop].Invisible();
            }
        }
    }
    public void MakeEnemiesVisible()
    {
        EnemyHealth[] enemies;
        enemies = aliveEnemies.GetComponentsInChildren<EnemyHealth>();
        if (enemies.Length == 0)
        {
            return;
        }
        else
        {
            for (int loop = 0; loop < enemies.Length; loop++)
            {
                enemies[loop].Visible();
            }
        }
    }

    public void EnableRangeIndicators()
    {
        RangeIndicators = true;
        for (int loop = 0; loop < buildPlates.Length; loop++)
        {
            if (buildPlates[loop].BuildIndex > 0)
            {
                buildPlates[loop].GetComponentInChildren<TurretTargetTrigger>().EnableRangeIndicator();
            }
            else
            {
                continue;
            }
        }
    }

    public void DisableRangeIndicators()
    {
        RangeIndicators = false;
        for (int loop = 0; loop < buildPlates.Length; loop++)
        {
            if (buildPlates[loop].BuildIndex > 0)
            {
                buildPlates[loop].GetComponentInChildren<TurretTargetTrigger>().DisableRangeIndicator();
            }
            else
            {
                continue;
            }
        }
    }


}
