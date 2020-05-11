using Unity.Mathematics;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public float mass = 100;
    Color landColor, waterColor;
    // Start is called before the first frame update
    void Start()
    {
        landColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        waterColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        GenerateTexture();
    }

    void GenerateTexture()
    {
        int seed = (int)(UnityEngine.Random.value * 100000);
        Texture2D planetLandTexture = new Texture2D(100,100);
        Texture2D planetWaterTexture = new Texture2D(100, 100);
        for (int i = 0; i < planetLandTexture.width; i++)
        {
            for(int j = 0; j < planetLandTexture.height; j++)
            {
                float3 pos = new float3(i/20f,j/20f, seed);
                float val = GetValueAtPosition(pos);
                if (val >= 0.2f)
                {
                    planetLandTexture.SetPixel(i, j, landColor);
                    planetWaterTexture.SetPixel(i, j, Color.clear);
                }
                else
                {
                    planetWaterTexture.SetPixel(i, j, waterColor);
                    planetLandTexture.SetPixel(i, j, Color.clear);
                }
                
            }
        }
        planetLandTexture.Apply();
        planetWaterTexture.Apply();
        GetComponent<SpriteRenderer>().material.SetTexture("_LandTex", planetLandTexture);
        GetComponent<SpriteRenderer>().material.SetTexture("_WaterTex", planetWaterTexture);
    }

    public static float GetValueAtPosition(float3 pos)
    {
        float noiseValue = 0;

        float frequency = 0.91f;
        float amplitude = 1;
        float persistence = 0.54f;
        float roughness = 1.83f;
        float strength = 1;
        int numLayers = 3;
        float recede = 0.7f;

        for (int i = 0; i < numLayers; i++)
        {
            float v = noise.cnoise(pos * frequency);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= roughness;
            amplitude *= persistence;
        }
        noiseValue = Mathf.Max(0, noiseValue - recede);
        return noiseValue * strength;
    }
}
