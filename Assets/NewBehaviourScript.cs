using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	public float speed = 90f;
	public float turnSpeed = 5f;
	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;
	
	public GameObject plane;
	public Camera mainCamera;
	public Camera secondCamera;
	public bool secondCameraActive = false;
	private float powerInput;
	private float turnInput;
	private Rigidbody carRigidbody;

	public GameObject bullet;




	void Awake () 
	{
		carRigidbody = GetComponent <Rigidbody>();
		Application.targetFrameRate = 60;
	}
	
	void Update () 
	{

		if (Input.GetKeyDown ("space") || (Input.touchCount == 4 && Input.GetTouch(3).phase == TouchPhase.Began)) {
			ChangeCamera();
		}

		if ((Input.touchCount == 3 && Input.GetTouch(2).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.B)) {
			GameObject ball = (GameObject)Instantiate(bullet,transform.position,transform.rotation);
			ball.transform.position = ball.transform.position + (ball.transform.forward * 35);
			ball.GetComponent<Rigidbody>().AddRelativeForce(ball.transform.forward * speed * 2);
		}

		powerInput = 0;
		turnInput = 0;
		Vector2 position;
		if (Input.GetMouseButton (0)) {
			position = Input.mousePosition;
		} else if (Input.touchCount > 0) {
			position = Input.GetTouch (0).position;
		} else
			return;

		if (secondCameraActive) {
			powerInput = (((position.y) / Screen.height)-0.5f) * 2;
			turnInput = ((position.x / Screen.width) - 0.5f) * 2;
			secondCamera.enabled = true;
			mainCamera.enabled = false;
			return;
		}

		var pos = mainCamera.ScreenToWorldPoint (new Vector3(position.x,position.y,10));

		var ypos = transform.position.y;
		if (Input.touchCount == 2 || Input.GetMouseButton (1))
			ypos = 70f;
		else 
			ypos = transform.position.y;

		GetComponent<Transform>().LookAt(new Vector3(pos.x,ypos,pos.z));
		powerInput = 1;
	}

	void ChangeCamera() {
		secondCameraActive = !secondCameraActive;
		secondCamera.enabled = secondCameraActive;
		mainCamera.enabled = !secondCameraActive;
	}
	
	void FixedUpdate()
	{
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, hoverHeight))
		{
			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
			carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
		}
		
		carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);
		carRigidbody.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);
		
	}
}
