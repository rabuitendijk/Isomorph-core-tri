
using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicComponentCamera : ComponentCamera {


    Camera camera;

    float speed = 8;
    int pixelPerUnit = 32, zoom = 1;

    public BasicComponentCamera()
    {
        camera = Camera.main;
        cameraSnapMinimum();
    }

    public override void update()
    {
        if (Input.GetKey("up"))
            camera.transform.position += new Vector3(0, Time.deltaTime * speed / zoom, 0);

        if (Input.GetKey("down"))
            camera.transform.position += new Vector3(0, -Time.deltaTime * speed / zoom, 0);

        if (Input.GetKey("right"))
            camera.transform.position += new Vector3(Time.deltaTime * speed / zoom, 0, 0);

        if (Input.GetKey("left"))
            camera.transform.position += new Vector3(-Time.deltaTime * speed / zoom, 0, 0);

        if (Input.GetKey(KeyCode.R))
            Application.LoadLevel(Application.loadedLevel);


        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            cameraSnapIn();

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            cameraSnapOut();
    }
    /// <summary>
    /// Snap to minimum zoom allowed
    /// </summary>
    void cameraSnapMinimum()
    {
        camera.orthographicSize = ((float)camera.pixelHeight / (float)pixelPerUnit) / (float)4;
        zoom = 1;
    }

    /// <summary>
    /// Snap camera in one level
    /// </summary>
    void cameraSnapIn()
    {
        if (zoom == 16)
            return;

        camera.orthographicSize /= 2;
        zoom++;
    }

    /// <summary>
    /// Snap camera out one level
    /// </summary>
    void cameraSnapOut()
    {
        if (zoom == 1)
            return;

        camera.orthographicSize *= 2;
        zoom--;
    }

}
