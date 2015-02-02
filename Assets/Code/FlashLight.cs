using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
        GetComponent<RectTransform>().position = new Vector3(Screen.width /2 , Screen.height / 2, 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<Image>().sprite.textureRect.width, GetComponent<Image>().sprite.textureRect.height);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
