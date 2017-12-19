using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundCheckScript : MonoBehaviour {

	public Transform playerTransform;
	public Transform reboundCheckPlatform;

	private Vector3 mousePosition;
	public ReboundProperties reboundProperties;
	private GameObject DefaultSurface;
	[HideInInspector] public bool insideSurface;
	[HideInInspector] public float platformRotation;

	void Start () {
		DefaultSurface = Resources.Load ("DefaultSurfaceProperties") as GameObject;
	}

	// Update is called once per frame
	void LateUpdate () {
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector3 playerPosition = playerTransform.position;

		Vector3 adjustedMousePosition = new Vector3 (mousePosition.x, mousePosition.y, 0.0f);

		Vector3 mouseToPlayer = adjustedMousePosition - playerPosition;

		platformRotation = (180 / Mathf.PI) * Mathf.Atan2 (mouseToPlayer.y, mouseToPlayer.x);
		//TODO: Investigate why platformRotation is 90 degrees off rather than leaving this disgusting piece of code as is
		reboundCheckPlatform.eulerAngles = new Vector3(0, 0, platformRotation - 90);

	}

	void OnTriggerEnter2D(Collider2D coll) {
		updateReboundProperties (coll.gameObject);
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			insideSurface = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			insideSurface = false;
			updateReboundProperties (coll.gameObject);
		}
	}

	bool IsInsideSurface() {
		return insideSurface;
	}

	public float getReboundCoefficient() {
		if (reboundProperties != null) {
			return reboundProperties.reboundCoefficient;
		} else {
			return 1.0f;
		}
	}

	void updateReboundProperties(GameObject surface) {
		reboundProperties = surface.gameObject.GetComponent<ReboundProperties> ();
		if (reboundProperties == null) {
			reboundProperties = surface.GetComponentInParent<ReboundProperties> ();
			if (reboundProperties == null) {
				reboundProperties = DefaultSurface.GetComponent<ReboundProperties> ();
			}
		}
	}
}
