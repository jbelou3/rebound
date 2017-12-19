using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour {

	private Vector2 currentRespawnPoint = Vector2.zero;

	public void updateRespawnPoint(Vector2 newRespawnPoint)
	{
		currentRespawnPoint = newRespawnPoint;
	}

	public Vector2 getRespawnPoint()
	{
		return currentRespawnPoint;
	}
}
