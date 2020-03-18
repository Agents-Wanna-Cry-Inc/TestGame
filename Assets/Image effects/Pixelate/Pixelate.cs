using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Pixelate")]
public class Pixelate : MonoBehaviour{
	[SerializeField] public Shader shader;
	
	[Range(1, 20)] [SerializeField] public int pixelSizeX = 1;
	[Range(1, 20)] [SerializeField] public int pixelSizeY = 1;
	[SerializeField] public bool lockXY = true;
	
	private int _pixelSizeX = 1;
	private int _pixelSizeY = 1;
	
	private Material material;
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (material == null) {
		    material = new Material(shader);
	    }
	    
		material.SetInt("_PixelateX", pixelSizeX);
		material.SetInt("_PixelateY", pixelSizeY);
		
		Graphics.Blit(source, destination, material);
	}
	
	void OnDisable()
	{
		DestroyImmediate(material);
	}

	void Update()
	{
		if (pixelSizeX != _pixelSizeX)
		{
			_pixelSizeX = pixelSizeX;
			
			if (lockXY)
			{
			    _pixelSizeY = pixelSizeY = _pixelSizeX;
		    }
		}
		
		if (pixelSizeY != _pixelSizeY)
		{
			_pixelSizeY = pixelSizeY;
			
			if (lockXY)
			{
			    _pixelSizeX = pixelSizeX = _pixelSizeY;
		    }
		}
	}

}
