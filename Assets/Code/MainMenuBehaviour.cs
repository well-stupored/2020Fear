using UnityEngine;
using System.Collections;

public class MainMenuBehaviour : MonoBehaviour {

    public void StartGame()
    {
        Application.LoadLevel("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
