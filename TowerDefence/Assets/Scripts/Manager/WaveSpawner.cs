using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState{
        spawning,
        waiting,
        counting
    }
    [System.Serializable]
    public class Wave{
        public string name;
        public Enemy [] enemies;
        public int [] enemyCounts;
        public float rate;
        public float timeBeforeWave;
    }
    [SerializeField] Wave [] waves;
    int nextWave = 0;

    [Header("Do Not Touch These Ones")]
    [SerializeField] float waveCountDown = 0;
    [SerializeField] SpawnState state = SpawnState.counting;
    [SerializeField] Text countDownText;
    [SerializeField] Text level;
    [SerializeField] Text wave;

    float searchCountDown = 1;

    private void Start() {
        level.text = "Level " + (GameManager.instance.Stage + 1).ToString();
        if(waves.Length == 0){
            Debug.Log("No waves set in inspector");
        }
        else{
            waveCountDown = waves[0].timeBeforeWave; 
        }
           
    }

    private void Update() {
        if(state == SpawnState.waiting){
            //check if enemies are still alive
            if(!EnemyIsAlive()){
                //begin new wave
                Debug.Log("Wave Complete");
                WaveCompleted();
            }
            else{
                return;
            }
        }

        if(waveCountDown <= 0){
            if(state != SpawnState.spawning){
                //start spawning wave
                countDownText.gameObject.SetActive(false);
                wave.gameObject.SetActive(true);
                wave.text = waves[nextWave].name;
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else{
            if(waveCountDown > 5 && Input.GetKeyDown(KeyCode.G)){
                waveCountDown = 5f;
            }
            countDownText.gameObject.SetActive(true);
            wave.gameObject.SetActive(false);
            waveCountDown -= Time.deltaTime;
            countDownText.text = SecondsToMinutes(waveCountDown);
        }    
    }

    void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		state = SpawnState.counting;
        


		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
            waveCountDown = waves[0].timeBeforeWave;
            switch(GameManager.instance.Stage){
                case 0:
                    SceneController.instance.LoadLevel2();
                break;
                case 1:
                    SceneController.instance.LoadWinScene();
                break;
            }
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
            waveCountDown = waves[nextWave].timeBeforeWave;
		}
	}
    bool EnemyIsAlive(){
        searchCountDown -= Time.deltaTime;
        if(searchCountDown <= 0){
            searchCountDown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null){
                Debug.Log("No more enemies");
                return false;
            }
            else{
                //Debug.Log("still some enemies");
            }
        }
        

        return true;
    }
    IEnumerator SpawnWave(Wave wave){
        Debug.Log("Spawning Wave " + wave.name);
        state = SpawnState.spawning;
        int total = TotalEnemies(wave);

        //spawn
        for(int loop = 0; loop < total; loop++){
            SpawnEnemy(wave);
            yield return new WaitForSeconds(wave.rate);
        }

        state = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy(Wave wave){
        
        int zeroCount = 0;
        for(int loop = 0; loop < wave.enemyCounts.Length; loop++){
            if(wave.enemyCounts[loop] == 0){
                zeroCount++;
            }
        }
        if(zeroCount == wave.enemyCounts.Length){
            return;
        }
        while(true){
            int randomEnemy = Random.Range(0, wave.enemies.Length);
            if(wave.enemyCounts[randomEnemy]>0){
                GameManager.instance.SpawnEnemy(wave.enemies[randomEnemy]);
                wave.enemyCounts[randomEnemy]--;
                return;
            }
        }
        
    }
    int TotalEnemies(Wave wave){
        int total = 0;
        for(int loop = 0; loop < wave.enemyCounts.Length; loop++){
            total += wave.enemyCounts[loop];
        }
        return total;
    }

    string SecondsToMinutes(float time){
        int minutes = (int) (time / 60f);
        int seconds = (int) (time % 60);
        string min = "";
        string sec = "";
        if(minutes == 0){
            min = "00";
        }
        else if(minutes < 10){
            min = "0" + minutes.ToString();
        }
        else
        {
            min = minutes.ToString();
        }
        if(seconds == 0){
            sec = "00";
        }
        else if(seconds < 10){
            sec = "0" + seconds.ToString();
        }
        else{
            sec = seconds.ToString();
        }
        return min + ":" + sec;
    }
}
