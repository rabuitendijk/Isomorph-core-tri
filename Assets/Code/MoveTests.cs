using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTests : MonoBehaviour {

    public float mass = 1f;
    public float friction = 4f;
    public float force = .3f;

    public float cutoff = .0005f;

    public float vx = 0;
    public float vy = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 i = parseInput();

        //Forward euler
        vx = (vx + Time.deltaTime * i.x / mass) / (1f + Time.deltaTime * friction / mass);
        vy = (vy + Time.deltaTime * i.y / mass) / (1f + Time.deltaTime * friction / mass);

        if (Mathf.Abs(vx) < cutoff)
            vx = 0;
        if (Mathf.Abs(vy) < cutoff)
            vy = 0;

        //Transform
        //Vector3 pos = transform.position;
        transform.position += new Vector3(.5f*(-vx+vy), -.25f*(vx+vy), 0);
    }

    Vector2 parseInput()
    {
        int hor = 0, ver = 0;
        if (Input.GetKey(KeyCode.A)){ hor -= 1; }
        if (Input.GetKey(KeyCode.D)) { hor += 1; }
        if (Input.GetKey(KeyCode.W)) { ver += 1; }
        if (Input.GetKey(KeyCode.S)) { ver -= 1; }

        if (Mathf.Abs(hor) == 1 && Mathf.Abs(ver) == 1)
        {
            //Dual keys
            return force * new Vector2(0.7071f * (-hor-ver), 0.7071f * (-ver+hor));
        }

        //Single keys
        return force * new Vector2((-hor-ver),  (-ver+hor));
    }
}
