using UnityEngine;
using System.Collections;

/**
 * version alfa-1
 * Needs to be attaced to the Camera object
 * Set to be used with pixel art (forced multiple of world-unit zooming).
 *
 * Notes: 
 * I dont think this is the updated version with 1x zoon enabled.
 * Also need update for outdated reloading code.
 * Add somthing to make the camera not spazz with controler config.
 *
 *
 * POSSIBLE BUG:
 * Camera transform not snapping to nearest pixel might be causing graphics glitches.
 *
 * Robin Apollo Buitendijk
 * Late Febuary 2017
 **/
[RequireComponent(typeof(Camera))]
public class SimpleWASD : MonoBehaviour {

	public float speed = 1;
	public int pixelPerUnit = 16;

	private Camera cam;
	private int zoom = 1;

	void Start(){
		cam = gameObject.GetComponent<Camera> () as Camera;
		snapMinimum ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("up"))
			transform.position += new Vector3(0, Time.deltaTime * speed / zoom, 0);
		
		if (Input.GetKey("down"))
			transform.position += new Vector3(0, -Time.deltaTime * speed / zoom, 0);

		if (Input.GetKey ("right"))
			transform.position += new Vector3(Time.deltaTime * speed / zoom, 0, 0);
		
		if (Input.GetKey("left"))
			transform.position += new Vector3(-Time.deltaTime * speed / zoom, 0, 0);

		if (Input.GetKey(KeyCode.R))
			Application.LoadLevel(Application.loadedLevel);


		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
			snapIn ();
		
		if(Input.GetAxis("Mouse ScrollWheel")< 0)
			snapOut();
	}

    /// <summary>
    /// Snap to minimum zoom allowed
    /// </summary>
	void snapMinimum(){
		cam.orthographicSize = ((float)cam.pixelHeight / (float)pixelPerUnit) / (float)4;
		zoom = 1;
	}
	
    /// <summary>
    /// Snap camera in one level
    /// </summary>
	void snapIn(){
		if (zoom == 16)
			return;

		cam.orthographicSize /= 2;
		zoom++;
	}

    /// <summary>
    /// Snap camera out one level
    /// </summary>
	void snapOut(){
		if (zoom == 1)
			return;

		cam.orthographicSize *= 2;
		zoom--;
	}
}
