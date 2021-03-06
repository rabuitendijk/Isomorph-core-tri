﻿
using Camera = UnityEngine.Camera;
using Input = UnityEngine.Input;
using Vector3 = UnityEngine.Vector3;
using Time = UnityEngine.Time;

namespace Input_C
{

    /// <summary>
    /// Moves the camera and does zooming
    /// </summary>
    public class BasicComponentCamera : ComponentCamera
    {


        Camera camera;

        float speed = 8;
        int pixelPerUnit = 32, zoom = 1;

        /// <summary>
        /// Common constructor
        /// </summary>
        public BasicComponentCamera()
        {
            camera = Camera.main;
            cameraSnapMinimum();
        }

        /// <summary>
        /// Run once per frame
        /// </summary>
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
}