using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour {

	public RespawnManager respawnManager;
	GameObject player;

	void Start ()
	{
		//gets the parent of this gameObject
		player = gameObject.transform.parent.gameObject;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Crusher")) {
			player.transform.position = respawnManager.getRespawnPoint ();
			player.GetComponent<PlayerController>().Reset ();
		}
	}
}
