using UnityEngine;
using System.Collections;
using Assets.Code;

public class ChromeCrusher : MonoBehaviour
{
    private GameController Game;
    public string KillTag = "player";

	void Start ()
	{
	    Game = GameObject.Find("game_controller").GetComponent<GameController>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == KillTag)
        {   //use loose
            Game.OnGameOver(false, "YOU WERE IMPALED BY SPIKES");
        }
    }

}
