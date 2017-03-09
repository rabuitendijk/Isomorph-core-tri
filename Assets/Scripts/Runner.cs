using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {


    public bool rebuildAliasses = false;

    // Use this for initialization
    void Start()
    {
        if (rebuildAliasses)
            new BuildAliasTextures();

        
        new AliasXMLLoader();

        Map map = new Map(12, 12, 12);
        new GraphicsControl();

        map.makeLevel();
       
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            onClick();
            return;
        }
    }

    void onClick()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Get position on lowest plane
        //Debug.Log(new Iso(pos.x, pos.y, 1).ToString());

        //First get position on highest plane
        
        float upper_z = Map.main.height+1, delta;
        float upper_x = (2f * pos.y - pos.x - .5f * upper_z + 1f), upper_y = (2f * pos.y + pos.x - .5f * upper_z + 1f);
        Iso target = new Iso(pos.x, pos.y, Mathf.FloorToInt(upper_z));
        target.addUnsafe(0,0,-1); //Start correction
        //Debug.Log("Above start: "+target.ToString() + "\t<" + upper_x + ", " + upper_y + ", " + upper_z + ">");

        int count = 0;
        while (target.z >= 0)
        {
            delta = nextCell(upper_x, upper_y, upper_z, target);
            upper_z -= 2 * delta; upper_x += delta; upper_y += delta;
            if (Map.main.inGrid(target) && Map.main.exists(target))
            {
                Debug.Log("["+count+"]Found: "+Map.main.get(target).isoObject.name+", "+target.ToString());
                return;
            }
            count++;
        }

        //Debug.Log("1st: " + target.ToString()+"\t<"+upper_x+", "+upper_y+", "+upper_z+">");
        Debug.Log("["+count+"]Nothing found");
    }

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
}
