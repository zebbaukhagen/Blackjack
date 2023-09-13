//////////////////////////////////////////////
//Assignment/Lab/Project: BlackJack
//Name: Zebulun Baukhagen
//Section: 2023SP.SGD.213.2172
//Instructor: Brian Sowers
//Date: 2/25/2023
/////////////////////////////////////////////

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        // get the current scene
        currentScene = SceneManager.GetActiveScene();
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickHelpButton()
    {
        if (currentScene.name == "MainMenu")
        {
            SceneManager.LoadScene("Instructions");
        }
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }
}
