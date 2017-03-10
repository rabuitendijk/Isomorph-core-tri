
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version aplha-1
/// 
/// Currently test version for development.
/// 
/// POSSIBLE BUG:
/// Camera transform not snapping to nearest pixel might be causing graphics glitches.
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public class BasicInputControl : InputControl{
    Tile currentTile;
    Material hoverMaterial, lastMaterial;
    IsoObject lastObject;

    Camera camera;

    float speed = 8;
    int pixelPerUnit = 32, zoom = 1;


    /// <summary>
    /// Create the common level input controller
    /// </summary>
    public BasicInputControl() : base()
    {
        hoverMaterial = new Material(Shader.Find("Sprites/Default"));
        hoverMaterial.color = new Color(.7f, .7f, .7f);
        camera = Camera.main;
        cameraSnapMinimum();
    }

    /// <summary>
    /// Input processing, run this once per frame
    /// </summary>
    public override void update()
    {


        if (raycastClick(out currentTile))
        {
            swapSelectedMaterial(currentTile);
        }
        else
            swapSelectedMaterial();

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
    /// Manages mouse hover graphics effects
    /// </summary>
    void swapSelectedMaterial(Tile current = null)
    {
        GraphicsControl.main.selector.SetActive(false);

        //clear selection
        if (lastObject != null)
        {
            foreach(Tile t in lastObject.tiles)
            {
                if (t.graphic != null)
                    t.graphic.GetComponent<SpriteRenderer>().material = lastMaterial;
            }
            lastObject = null;
           
        }

        if (current != null)
        {
            lastObject = current.isoObject;
            foreach (Tile t in lastObject.tiles)
            {
                if (t.graphic != null)
                {
                    lastMaterial = t.graphic.GetComponent<SpriteRenderer>().material;
                    t.graphic.GetComponent<SpriteRenderer>().material = hoverMaterial;
                }
                
            }

            if (lastObject.tiles.Count < 3) {
                GraphicsControl.main.selector.SetActive(true);
                GraphicsControl.main.selector.gameObject.transform.position = lastObject.origin.toPos();
                GraphicsControl.main.selector.GetComponent<SpriteRenderer>().sortingOrder = lastObject.origin.depth + 1;
            }
            return;
        }
        
    }

    /// <summary>
    /// finds first live Tile under mouseclick
    /// </summary>
    bool raycastClick(out Tile tile)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float upper_z = LogicControl.main.height + 1, delta;
        float upper_x = (2f * pos.y - pos.x - .5f * upper_z + 1f), upper_y = (2f * pos.y + pos.x - .5f * upper_z + 1f);
        Iso target = new Iso(pos.x, pos.y, Mathf.FloorToInt(upper_z));
        target.addUnsafe(0, 0, -1); //Start correction

        int count = 0;
        while (target.z >= 0)
        {
            delta = nextCell(upper_x, upper_y, upper_z, target);
            upper_z -= 2 * delta; upper_x += delta; upper_y += delta;
            if (LogicControl.main.inGrid(target) && LogicControl.main.exists(target))
            {
                tile = LogicControl.main.get(target);
                return true;
            }
            count++;
        }

        tile = null;
        return false;
    }

    /// <summary>
    /// Finds next cell in simulated raycast for mouse click
    /// </summary>
    float nextCell(float ox, float oy, float oz, Iso coord)
    {
        ox -= coord.x; ox = 1 - ox;
        oy -= coord.y; oy = 1 - oy;
        oz -= coord.z; oz /= 2;

        if (oz <= ox)
        {
            if (oz <= oy)
            {
                //z transition

                coord.addUnsafe(0, 0, -1);
                return oz;
            }
            //y transtion
            coord.addUnsafe(0, 1, 0);
            return oy;
        }
        else if (ox <= oy)
        {
            //x transtion

            coord.addUnsafe(1, 0, 0);
            return ox;
        }
        //y transition
        coord.addUnsafe(0, 1, 0);
        return oy;
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
