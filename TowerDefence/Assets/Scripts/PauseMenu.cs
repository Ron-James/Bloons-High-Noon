using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused;
    GameObject player;
    [SerializeField] GameObject menuUI;

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(IsPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }
    public void Resume(){
        Time.timeScale = 1f;
        IsPaused = false;
        menuUI.SetActive(false);
        player.GetComponent<FirstPersonAIO>().EnableCamera();
        //player.GetComponent<FirstPersonAIO>().playerCanMove = true;
        
        
        
    }

    public void Pause(){
        IsPaused = true;
        player.GetComponent<FirstPersonAIO>().DisableCamera();
        //player.GetComponent<FirstPersonAIO>().playerCanMove = false;
        menuUI.SetActive(true);
        Time.timeScale = 0f;

    }
    public void LoadMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}
