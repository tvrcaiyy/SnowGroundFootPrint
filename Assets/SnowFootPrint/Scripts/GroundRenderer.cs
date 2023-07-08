using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRenderer : MonoBehaviour
{
    public Vector2 MapSize = new Vector2(100, 100);
    public RenderTexture TrackMap;
    public Texture2D FootPrintTex;
    public float FootPrintSize = 1.0f;
    public List<GameObject> NpcList = new List<GameObject>();
    private float SnowResolution = 512;

    private void Start()
    {
        if (TrackMap)
            SnowResolution = TrackMap.width;
        ClearTexture();
    }

    private void OnDisable()
    {
        ClearTexture();
    }

    void ClearTexture()
    {
        if (!TrackMap)
            return;
        Texture2D clearTexture = new Texture2D(1, 1);
        clearTexture.SetPixel(0,0,new Color(255,255,255,0.05f));
        clearTexture.Apply();
        Graphics.Blit(clearTexture,TrackMap);
    }

    void Update()
    {
        for (int i = 0; i < NpcList.Count; i++)
        {
            GameObject go = NpcList[i];
            Vector3 localPos = transform.InverseTransformPoint(go.transform.position) * transform.localScale.x;
            Vector2 pixelUV = new Vector2(1 - (localPos.x + MapSize.x * 0.5f) / MapSize.x,
                1 - (localPos.z + MapSize.y * 0.5f) / MapSize.y);
            pixelUV.y *= SnowResolution;
            pixelUV.x *= SnowResolution;
            DrawSnowMap(pixelUV.x, pixelUV.y);
        }
    }
    
    void DrawSnowMap(float posX, float posY)
    {
        RenderTexture.active = TrackMap;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, SnowResolution, SnowResolution, 0);

        Graphics.DrawTexture(new Rect(posX - FootPrintTex.width / FootPrintSize, (TrackMap.height - posY) - FootPrintTex.height / FootPrintSize, FootPrintTex.width / (FootPrintSize * 0.5f), FootPrintTex.height / (FootPrintSize * 0.5f)), FootPrintTex);
        GL.PopMatrix();
        RenderTexture.active = null;

    }
}
