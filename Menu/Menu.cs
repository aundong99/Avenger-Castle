using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Sample_Scene()
    {
        SceneManager.LoadScene(2);
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(1);
    }
    public void Main_Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void end_game()
    {
        Application.Quit();
    }

}
