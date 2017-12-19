using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterScript : MonoBehaviour {

	public float deltaForce = 100f;
	public float timeSpan = 3f;
	public Color newColor;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.tag == "Player")
		{
			PlayerController pc = coll.gameObject.GetComponent<PlayerController> ();
			pc.alterJumpForce (deltaForce, timeSpan, newColor);
		}
	}
}
