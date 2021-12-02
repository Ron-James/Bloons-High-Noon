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
        Resume();
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
        IsPaused = false;
        Time.timeScale = 1f;
        menuUI.SetActive(false);
        if(GameManager.instance.FirstPerson && !BuildMenu.MenuIsOpen){
            player.GetComponent<FirstPersonAIO>().EnableCamera();
        }
        
        //player.GetComponent<FirstPersonAIO>().playerCanMove = true;
        
        
        
    }

    public void Pause(){
        IsPaused = true;
        Time.timeScale = 0f;
        player.GetComponent<FirstPersonAIO>().DisableCamera();
        //player.GetComponent<FirstPersonAIO>().playerCanMove = false;
        menuUI.SetActive(true);
        

    }
    public void LoadMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}
