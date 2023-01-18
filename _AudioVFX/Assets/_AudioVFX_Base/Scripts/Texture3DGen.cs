using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Texture3DGen : MonoBehaviour
{
    public Texture2D inputTexture;
    public string outputName;
    public int depth;

    public void CreateTexture3D()
    {
        if (inputTexture == null)
        {
            return;
        }

        // Configure the texture
        // int size = 32;
        int width = inputTexture.width;
        int height = inputTexture.height;

        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        // Create the texture and apply the configuration
        Texture3D texture = new Texture3D(width, height, depth, format, false);
        texture.wrapMode = wrapMode;

        // Create a 3-dimensional array to store color data
        Color[] colors = new Color[width * height * depth];
   
        for(int z = 0; z < depth; z++)
        {
            int zOffset = z * width * height;
            for(int x = 0; x < width; x++)
            {
                int xOffset = x * width;
                for(int y = 0; y < height; y++)
                {
                    colors[y + xOffset + zOffset] = inputTexture.GetPixel(x, y);
                }
            }
        }

        // Copy the color values to the texture
        texture.SetPixels(colors);

        // Apply the changes to the texture and upload the updated texture to the GPU
        texture.Apply();

        // Save the texture to your Unity Project
        AssetDatabase.CreateAsset(texture, "Assets/Example3DTexture.asset");
    }
}



/*
        // Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
        float inverseResolution = 1.0f / (size - 1.0f);
        for (int z = 0; z < size; z++)
        {
            int zOffset = z * size * size;
            for (int y = 0; y < size; y++)
            {
                int yOffset = y * size;
                for (int x = 0; x < size; x++)
                {
                    colors[x + yOffset + zOffset] = new Color(x * inverseResolution,
                        y * inverseResolution, z * inverseResolution, 1.0f);
                }
            }
        }
        */
