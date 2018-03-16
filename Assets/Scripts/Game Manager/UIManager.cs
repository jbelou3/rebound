using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public PlayerController playerController;
	public Text jumpModeText;

	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			changeJumpMode(1);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			changeJumpMode(2);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)) {
			changeJumpMode(3);
		}
	}

	void changeJumpMode (int newMode) {
		jumpModeText.text = "" + newMode;
		playerController.setJumpMode(newMode);
	}
}
