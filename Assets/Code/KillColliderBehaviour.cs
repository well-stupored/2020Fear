using Assets.Code;
using UnityEngine;
using System.Collections;

public class KillColliderBehaviour : MonoBehaviour
{

    private EnemyBehaviour _parentMonster;

	// Use this for initialization
	void Start ()
	{
	    _parentMonster = transform.parent.GetComponent<EnemyBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            _parentMonster.RendPlayer();
        }
    }
}
