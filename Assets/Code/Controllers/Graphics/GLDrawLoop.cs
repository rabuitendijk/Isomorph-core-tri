using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The component that randers GL ininstructions.
/// 
/// </summary>
public class GLDrawLoop : MonoBehaviour
{
	List<GLInstruction> instructions = new List<GLInstruction>();
    Material mat;
    public static GLDrawLoop main { get; protected set; }

    public void Start()
    {
        mat = createGLMat();
		main = this;
    }
	
	

    /// <summary>
    /// The render loop
    /// </summary>
    void OnRenderObject()
    {
        if (instructions.Count == 0)
			return;
		
		// Apply the line material
        mat.SetPass(0);
		
		foreach(GLInstruction j in instructions)
			innerloop(j);

		
    }
	
    /// <summary>
    /// The rendering loop for 1 instruction
    /// </summary>
	private void innerloop(GLInstruction j)
	{
		GL.PushMatrix();
		if (j.relativeToCamera)
			GL.LoadOrtho();
		else
			GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
		
		GL.Color(j.color);
        
		if (j.autoconnect)
		{
			for (int i=0; i < j.points.Count-1; i++)
			{
				GL.Vertex(j.points[i]);
				GL.Vertex(j.points[i+1]);
			}
			if (j.loop)
			{
				GL.Vertex(j.points[j.points.Count-1]);
				GL.Vertex(j.points[0]);
			}
		}
		else
		{
			for (int i=0; i < j.points.Count-1; i+=2)
			{
				GL.Vertex(j.points[i]);
				GL.Vertex(j.points[i+1]);
			}
			if (j.loop)
			{
				GL.Vertex(j.points[j.points.Count-1]);
				GL.Vertex(j.points[0]);
			}
		}

        GL.End();
		GL.PopMatrix();
	}

    /// <summary>
    /// Add an instruction
    /// </summary>
	public void add(GLInstruction i)
	{
		instructions.Add(i);
	}
	
    /// <summary>
    /// remonve an instruction
    /// </summary>
	public void remove(GLInstruction i)
	{
		instructions.Remove(i);
	}
	
    /// <summary>
    /// Remove all instructions
    /// </summary>
	public void clear()
	{
		instructions.Clear();
	}
	
	
    /// <summary>
    /// The simple drawing material
    /// Unlit/Color for vertex color
    /// </summary>
    private static Material createGLMat()
    {

        // Unity has a built-in shader that is useful for drawing
        // simple colored things.
        //Material lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        Material lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));



        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off length writes
        lineMaterial.SetInt("_ZWrite", 0);
        //lineMaterial.SetInt("ZTest", 2);

        return lineMaterial;
    }
}
