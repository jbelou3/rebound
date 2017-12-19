using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[HideInInspector] public bool jump = false; //set to true to make the player jump

	public GameObject reboundCheck;
	private ReboundCheckScript reboundCheckScript;

	public float maxReboundForce = 1200f;
	public float minReboundForce = 700f;
	public float coolDown = 1f;
	public float maxChargeTime = 0.75f;

	public Canvas arrowCanvas;
	public Slider chargeSlider;

	public GameObject jumpPuff;

	private Color rcColorReady = Color.red;
	private Color rcColorExhausted = Color.black;

	private Rigidbody2D rb2d;
	private float chargeTime = 0f;
	private bool canRebound = false;
	private float coolDownTimer = 0f;
	private bool splattered = false;
	private bool rcShifting = false;
	private AudioSource aud;
	private bool doubleJumpAvailable = true;
	private Renderer rcRenderer;
	private Renderer playerRenderer;
	public int jumpMode;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();

		reboundCheckScript = reboundCheck.GetComponent<ReboundCheckScript> ();


		chargeSlider.maxValue = maxChargeTime;

		aud = gameObject.GetComponent<AudioSource> ();

		playerRenderer = GetComponent<Renderer> ();
		rcRenderer = reboundCheck.GetComponent<Renderer> ();
	}

	// Update is called once per frame
	void Update ()
	{

		//checks using trigger colliders
		canRebound = reboundCheckScript.insideSurface && coolDownTimer <= Time.time;
		
		if (canRebound && !doubleJumpAvailable) {
			doubleJumpAvailable = true;
		}

		if ((canRebound || doubleJumpAvailable) && !rcShifting) {
			StartCoroutine (changeReflectCheckMaterial(rcColorReady, .6f));	
		} else if(!rcShifting) {
			StartCoroutine (changeReflectCheckMaterial(rcColorExhausted, .3f));
		}
		//controls the angle of the arrow
		//TODO: Investigate why the angle is 90 degrees off rather than leaving this disgusting piece of code as is
		arrowCanvas.transform.eulerAngles = new Vector3(0, 0, reboundCheckScript.platformRotation + 90);
		//controls the length of the arrow
		chargeSlider.value = chargeTime;

		//TODO: make it so you can only hold charge for so long
		if (Input.GetMouseButton (0) && chargeTime < maxChargeTime) {
			chargeTime += Time.deltaTime;
		}

		if (Input.GetMouseButtonUp (0) && chargeTime != 0) {
			if (canRebound) {
				Rebound (CalculateReboundPower (), reboundCheckScript.getReboundCoefficient(), true);
				Instantiate(jumpPuff, transform.position, reboundCheckScript.reboundCheckPlatform.transform.localRotation);
			} else if (doubleJumpAvailable) {
				Rebound (CalculateReboundPower (), .5f, false);
				doubleJumpAvailable = false;
			} else {
				chargeTime = 0f;
			}
		}

	}

	void FixedUpdate()
	{
		//nothing right now, but probably stuff later
	}

	float CalculateReboundPower()
	{
		return minReboundForce + (maxReboundForce - minReboundForce) * (chargeTime / maxChargeTime);
	}

	void Rebound (float reboundForce, float reboundCoefficient, bool mainJump)
	{
		chargeTime = 0f;

		SetCoolDown ();

		if (mainJump || jumpMode == 1) {
			rb2d.velocity = new Vector2 (0, 0);
		}

		Vector3 goal1 = transform.position - reboundCheck.transform.position;
		goal1 = Vector3.Normalize (goal1);
		Vector2 goal2 = new Vector2 (goal1.x, goal1.y);

		if (!mainJump && jumpMode == 2) {
			if(Mathf.Sign(goal2.x) != Mathf.Sign(rb2d.velocity.x)) {
				rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
			}
			if(Mathf.Sign(goal2.y) != Mathf.Sign(rb2d.velocity.y)) {
				rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
			}
		}

		rb2d.AddForce(goal2 * reboundForce * reboundCoefficient);

		aud.Play();
	}

	void SetCoolDown(){
		coolDownTimer = Time.time + coolDown;
	}
	
	public void Reset()
	{
		rb2d.velocity = Vector2.zero;
		rb2d.angularVelocity = 0f;
		chargeTime = 0f;
		coolDown = 0f;
	}
	
	public void alterJumpForce(float deltaPower, float time, Color newColor)
	{
		if (splattered) 
		{
			return;
		} else 
		{
			splattered = true;
			StartCoroutine (setJumpForce (deltaPower, time, newColor));
		}
	}

	public void setJumpMode(int newMode) {
		jumpMode = newMode;
	}
	
	IEnumerator setJumpForce (float deltaPower, float time, Color newColor)
	{
		float ogMax = maxReboundForce;
		float ogMin = minReboundForce;
		//Material ogMaterial = playerRenderer.material;
		Color original = playerRenderer.material.color;
		playerRenderer.material.color = newColor;

		maxReboundForce += deltaPower;
		minReboundForce += deltaPower;

		float timeElapsed = 0.0f;

		while (playerRenderer.material.color != original) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed > time) {
				timeElapsed = time;
			}

			playerRenderer.material.color = Color.Lerp(newColor, original, timeElapsed/time);
			yield return null;
		}
		
		//playerRenderer.material = newMaterial;
		//yield return new WaitForSeconds(time);

		

		maxReboundForce = ogMax;
		minReboundForce = ogMin;
		//playerRenderer.material = ogMaterial;
		splattered = false;

		yield return 0;
	}
	
	IEnumerator changeReflectCheckMaterial(Color newColor, float time) {
	
		rcShifting = true;
		float timeElapsed = 0.0f;
		Color original = rcRenderer.material.color;
		while (rcRenderer.material.color != newColor) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed > time) {
				timeElapsed = time;
			}
			rcRenderer.material.color = Color.Lerp (original, newColor, timeElapsed/time);
			yield return null;
		}

		rcShifting = false;
		yield return 0;
		
	}

}