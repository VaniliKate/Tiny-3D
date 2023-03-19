using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;

    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}
