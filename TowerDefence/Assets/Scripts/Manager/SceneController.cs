using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Quit(){
        Application.Quit();
    }

    public void LoadLevel1(){
        SceneManager.LoadScene("Level_01");
    }

    public void LoadGameOver(){
        SceneManager.LoadScene("GameOver");
    }
    public void LoadMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}
