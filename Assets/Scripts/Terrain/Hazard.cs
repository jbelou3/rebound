using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	public RespawnManager respawnManager;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.name.Equals ("Player")) {
			coll.gameObject.transform.position = respawnManager.getRespawnPoint ();
			coll.gameObject.GetComponent<PlayerController>().Reset ();
		}
	}
}
