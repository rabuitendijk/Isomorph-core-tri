using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {

    public Sprite sp;
    Transform tileFolder;

    // Use this for initialization
    void Start()
    {
        Material mat = new Material(Shader.Find("Sprites/Default"));
        tileFolder = new GameObject() { name = "TileFolder"}.transform;



        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                newOb(new Iso(i, j, Mathf.FloorToInt((i+ j)/3f)), sp, mat);
            }
        }

        Material mat2 = new Material(Shader.Find("Sprites/Default"));
        mat2.color = new Color(.95f, 1f, .90f);

        for (int i = 0; i < 5; i++)
        {
            newOb(new Iso(0, 3, i+2), sp, mat2);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    Tile newOb(Iso coord, Sprite sp, Material mat)
    {
        GameObject ret = new GameObject() { name = "isotile("+coord.x+", "+coord.y+", "+coord.z+")" };
        ret.transform.position = coord.toPos();
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = sp;
        sr.material = mat;
        sr.sortingLayerName = "DepthSort";
        sr.sortingOrder = coord.depth;

        return new Tile(coord, ret);
    }
}
