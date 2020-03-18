using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Monochromize")]
public class Monochromize : MonoBehaviour{
	[SerializeField] public Shader shader;
	
	[Range(0.0f, 1.0f)] [SerializeField] public float intensity = 0.0f;
	[Range(0.0f, 2.0f)] [SerializeField] public float brightnessFactor = 1.0f;
	
	private Material material;
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if(material == null) {
		    material = new Material(shader);
	    }
	    
		material.SetFloat("_Intensity", intensity);
		material.SetFloat("_BrightnessFactor", brightnessFactor);
		
		Graphics.Blit(source, destination, material);
	}
	
	void OnDisable()
	{
		DestroyImmediate(material);
	}
}
