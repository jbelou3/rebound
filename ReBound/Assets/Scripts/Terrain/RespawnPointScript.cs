using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointScript : MonoBehaviour {

	public RespawnManager respawnManager;

	void OnTriggerEnter2D(Collider2D coll) {
		respawnManager.updateRespawnPoint (gameObject.transform.position);

		//visual stuff here

		Destroy(gameObject.GetComponent<Collider2D>());
	}
}
