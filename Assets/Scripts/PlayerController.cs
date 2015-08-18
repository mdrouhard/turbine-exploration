/* Player Controller targeting Oculus VR
 * Based on UnityStandardAssets First Person Controller */

using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;



[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour {

	[Serializable]
	public class MovementSettings
	{
		public float ForwardSpeed = 8.0f;   // Speed when moving forward
		public float BackwardSpeed = 4.0f;  // Speed when moving backwards
		public float StrafeSpeed = 4.0f;    // Speed when moving sideways
		public float RunMultiplier = 2.0f;   // Multiplier for speed when moving quickly
		public KeyCode RunKey = KeyCode.LeftShift;
		public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
		[HideInInspector] public float CurrentTargetSpeed = 8f;
		
		
		#if !MOBILE_INPUT
		private bool m_Running;
		#endif
		
		public void UpdateDesiredTargetSpeed(Vector2 input)
		{
			if (input == Vector2.zero) return;
			if (input.x > 0 || input.x < 0)
			{
				//strafe
				CurrentTargetSpeed = StrafeSpeed;
			}
			if (input.y < 0)
			{
				//backwards
				CurrentTargetSpeed = BackwardSpeed;
			}
			if (input.y > 0)
			{
				//forwards
				//handled last because if strafing and moving forward at the same time forwards speed should take precedence
				CurrentTargetSpeed = ForwardSpeed;
			}
			
			
			#if !MOBILE_INPUT
			if (Input.GetKey(RunKey))
			{
				CurrentTargetSpeed *= RunMultiplier;
				m_Running = true;
			}
			else
			{
				m_Running = false;
			}
			#endif
		}
		
		#if !MOBILE_INPUT
		public bool Running
		{
			get { return m_Running; }
		}
		#endif
	}

	[Serializable]
	public class AdvancedSettings
	{
		public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
	}

	public Camera cam;
	//public OVRCameraRig cam;
	public GameObject reticle;
	
	public MovementSettings movementSettings = new MovementSettings();
	public MouseLook mouseLook = new MouseLook();
	public AdvancedSettings advancedSettings = new AdvancedSettings();
	
	
	private Rigidbody m_RigidBody;
	private float m_YRotation;
	private bool m_Braking;
	private float m_BrakeVelocity;
	
	
	public Vector3 Velocity
	{
		get { return m_RigidBody.velocity; }
	}
	
	public bool Running
	{
		get
		{
			#if !MOBILE_INPUT
			return movementSettings.Running;
			#else
			return false;
			#endif
		}
	}

	// initialization
	void Start () {
		m_RigidBody = GetComponent<Rigidbody>();
		//mouseLook.Init (transform, cam.centerEyeAnchor);
		mouseLook.Init (transform, cam.transform);
		m_Braking = false;
		m_BrakeVelocity = (100f - advancedSettings.slowDownRate)/100f;
	}
	
	// Update is called once per frame
	void Update () {
		if(!reticle.activeSelf && (Input.GetButton("Fire1"))) {
			reticle.SetActive(!reticle.activeSelf);
		} else if(reticle.activeSelf && (Input.GetButton("Fire3"))) {
			reticle.SetActive(!reticle.activeSelf);
		}
		
		RotateView();
	}

	private void FixedUpdate()
	{
		Vector2 input = GetInput();
		
		// if braking, only brake
		if (m_Braking) {
			m_RigidBody.velocity = m_RigidBody.velocity * m_BrakeVelocity;
			m_RigidBody.angularVelocity = m_RigidBody.angularVelocity * m_BrakeVelocity;
			// else if a move is desired, apply appropriate force
		} else if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) /*&& (advancedSettings.airControl || m_IsGrounded)*/)
		{
			// always move along the camera forward as it is the direction that it being aimed at
			//Vector3 desiredMove = cam.centerEyeAnchor.forward*input.y + cam.centerEyeAnchor.right*input.x;
			Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
			
			desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
			desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
			desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;
			
			if (m_RigidBody.velocity.sqrMagnitude <
			    (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
			{
				m_RigidBody.AddForce(desiredMove, ForceMode.Impulse);
//									Debug.Log("Trying to add force: " + desiredMove);
			}
		}
		
		// If input & previous velocity are nonexistent or negligible, sleep
		if (Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
		{
			m_RigidBody.Sleep();
		}
	}
	
	private Vector2 GetInput()
	{
		
		Vector2 input = new Vector2
		{
			x = CrossPlatformInputManager.GetAxis("Horizontal"),
			y = CrossPlatformInputManager.GetAxis("Vertical")
		};
		
		if (input == Vector2.zero) {
			m_Braking = true;
		} else {
			m_Braking = false;
			movementSettings.UpdateDesiredTargetSpeed(input);
		}
		
		return input;
	}
	
	
	private void RotateView()
	{
		//avoids the mouse looking if the game is effectively paused
		if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;
		
		//TODO: REMOVE?
		// get the rotation before it's changed
		//            float oldYRotation = transform.eulerAngles.y;
		
		//mouseLook.LookRotation (transform, cam.centerEyeAnchor);
		mouseLook.LookRotation (transform, cam.transform);
	}
	
	// Stop on collision (don't bounce)
	void OnCollisionEnter(Collision collision) {
		m_RigidBody.velocity = Vector3.zero;
	}
}
