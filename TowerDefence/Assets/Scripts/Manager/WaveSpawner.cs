using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public int count;
        public float rate;
    }
    [SerializeField] Wave [] waves;
    int nextWave = 0;
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] float waveCountDown = 0;
    [SerializeField] SpawnState state = SpawnState.counting;

    float searchCountDown = 1;

    private void Start() {
        waveCountDown = timeBetweenWaves;    
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
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else{
            waveCountDown -= Time.deltaTime;
        }    
    }

    void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		state = SpawnState.counting;
		waveCountDown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
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
                Debug.Log("still some enemies");
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
            yield return new WaitForSeconds(1f/wave.rate);
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
}
