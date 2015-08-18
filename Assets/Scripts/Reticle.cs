using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

	public GameObject targetObject;
	public Material highlightMaterial;
	public Camera cameraFacing = null;
	public float fixedGazeSeconds = 2f;

	private Vector3 originalScale;
	private float timer;


	// Initialization
	void Start () {
		originalScale = transform.localScale;
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraFacing != null) {
			RaycastHit hit;
			float distance;
			if (Physics.Raycast(new Ray(cameraFacing.transform.position, cameraFacing.transform.forward), 
			                    out hit)) {
				distance = hit.distance;

				// update timer if object encountered with Raycast
				timer += Time.deltaTime;

				// Cross-platform input "Fire2" triggers location saving 
				if( (Input.GetButton("Fire2")) &&
				   ((hit.collider.transform.root == targetObject.transform) || (hit.collider.transform == targetObject.transform)) ) {
					Highlight(hit.collider.gameObject);
				}
			} else {
				// reset timer if no object encountered with Raycast
				timer = 0f;
				distance = cameraFacing.farClipPlane * 0.95f;
			}
		

			transform.position = cameraFacing.transform.position + 
			(cameraFacing.transform.forward * distance);
			
			this.transform.LookAt (cameraFacing.transform.position);
			// rotate around y axis because the above has back of quad facing camera
			transform.Rotate (0.0f, 180.0f, 0.0f);

			// fix screen size of reticle (regardless of distance)
			// if very close to reticle, make reticle appear slightly larger
			if (distance < 10.0f) {
				distance *= 1 + 5*Mathf.Exp(-distance);
			}
			transform.localScale = originalScale * distance;

			// if gaze has fixed on an object for more seconds than fixedGazeSeconds
			if (timer > fixedGazeSeconds) {
				Highlight (hit.collider.gameObject);
				timer = 0f;
			}
		}
	}

	// Animate Extended Gaze
	private void AnimateExtendedGaze(GameObject gazeObject) {

	}

	// Highlight targeted game object
	void Highlight(GameObject highlightObject) {
		highlightObject.GetComponent<MeshRenderer> ().material = highlightMaterial;
	}
}
