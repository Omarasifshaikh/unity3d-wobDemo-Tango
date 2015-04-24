using UnityEngine;
using System.Collections;

public class PreyMovement : MonoBehaviour {

	public float speed = 6f;            // The speed that the player will move at.
	
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.
	private Vector3 m_targetPosition;
	Vector3 newPosition,oldPos;
	void Awake ()
	{
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");
		
		// Set up references.
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		m_targetPosition = transform.position;
		oldPos = transform.position;
	}
	
	void GetTargetPosition()
	{
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;
		// Perform the raycast and if it hits something on the floor layer...
		if (Physics.Raycast (Camera.main.transform.position,
		                     Camera.main.transform.forward,
		                     out floorHit,
		                     camRayLength, floorMask))
		{
			m_targetPosition = floorHit.point;
		}
	}
	void Update(){
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				newPosition = hit.point;
				transform.position = (Vector3)Vector3.Lerp(oldPos ,newPosition,20f);

			}
		}
		}
	void FixedUpdate ()
	{/*
		GetTargetPosition();
		Vector3 vectorToTargetPosition = m_targetPosition - transform.position;
		
		if(vectorToTargetPosition.magnitude > 0.1f)
		{
			vectorToTargetPosition.Normalize();
			// Move the player around the scene.
			Move (vectorToTargetPosition.x, vectorToTargetPosition.z);
			// Animate the player.
			Animating (vectorToTargetPosition.x, vectorToTargetPosition.z);
		}
		// Turn the player to face the mouse cursor.
		Turning ();*/
	}
	
	void Move (float h, float v)
	{
		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v);
		
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		
		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
	}
	
	void Turning ()
	{
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;
		
		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;
			
			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;
			
			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			
			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (newRotation);
			//Debug.Log(floorHit.point.x + " : " + floorHit.point.y + " : " + floorHit.point.z);
			/*if(Input.GetMouseButtonDown(0)){
				Debug.Log("Pressed left click.");
			Move(floorHit.point.x,floorHit.point.z);
			}*/
		}
	}
	
	void Animating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = h != 0f || v != 0f;
		
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("PlayerWalking", walking);
	}
}
