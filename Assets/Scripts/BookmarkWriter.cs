using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Boomlagoon.JSON;

public class BookmarkWriter : MonoBehaviour {

	public string outputFileName = "";

	private StreamWriter sw;
	private string outputPath;
	private JSONObject jsonOutput;
	private JSONArray bookmarks;

	// Use this for initialization
	void Start () {
		string format = "yyyyMMdd-hhmm";
		string resourceDir = "Assets/Resources/";
		// create resources directory if it doesn't exist
		try {
			DirectoryInfo di = Directory.CreateDirectory(resourceDir);
			outputPath = resourceDir  + outputFileName + "-" + DateTime.Now.ToString (format);
			if (File.Exists (outputPath)) {
				Debug.Log ("ERROR: " + outputPath + " already exists");
				return;
			}
		} catch (Exception e) {
			Console.WriteLine ("Error creating bookmarks file: {0}", e.ToString());
		}

		bookmarks = new JSONArray ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire2")) {
		//if (Input.GetKeyDown ("b")) {
			AddBookmark ();
		}
	}

	// add bookmark at 
	void AddBookmark() {
		JSONObject newBookmark = CreateJSONBookmark (this.gameObject);
		bookmarks.Add (newBookmark);
	}

	// write bookmark file before application close
	void OnApplicationQuit() {
		WriteJSONToFile ();
	}

	// create bookmark composed of position and rotation objects
	private JSONObject CreateJSONBookmark(GameObject gameObject) {
		JSONObject bookmark = new JSONObject ();

		// Add position
		JSONObject position = new JSONObject();
		position.Add ("x", gameObject.transform.position.x);
		position.Add ("y", gameObject.transform.position.y);
		position.Add ("z", gameObject.transform.position.z);
		bookmark.Add ("position", position);

		// Add rotation
		JSONObject rotation = new JSONObject();
		rotation.Add ("x", gameObject.transform.rotation.x);
		rotation.Add ("y", gameObject.transform.rotation.y);
		rotation.Add ("z", gameObject.transform.rotation.z);
		rotation.Add ("w", gameObject.transform.rotation.w);
		bookmark.Add ("rotation", rotation);

		return bookmark;
	}

	// save bookmarks in JSON format to output file in Resources directory
	private void WriteJSONToFile() {
		if (bookmarks.Length != 0) {
			jsonOutput = new JSONObject ();
			jsonOutput.Add ("bookmarks", bookmarks);

			sw = new StreamWriter (outputPath);
			sw.Write (jsonOutput.ToString ());
			sw.Close ();
		}
	}
}