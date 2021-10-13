using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] BuildMenu buildMenu;
    bool isHitIsland = false;
    [SerializeField] GameObject buildPrompt;
    private bool _CameraEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_CameraEnabled)
        {
            CheckIslandMenu();
        }
        if (isHitIsland)
        {
            buildPrompt.SetActive(true);
        }
        else
        {
            buildPrompt.SetActive(false);
        }
    }
    public void CheckIslandMenu()
    {
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red, 10000f);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5f) && hit.collider.tag == "MenuTrigger")
        {
            //Debug.Log("menu thing");
            isHitIsland = true;
            if (Input.GetKeyDown(KeyCode.B))
            {
                BuildPlate buildPlate = hit.collider.gameObject.GetComponentInParent<BuildPlate>();
                buildMenu.OpenMenu(buildPlate);

            }
        }
        else
        {
            isHitIsland = false;
        }
    }
}
