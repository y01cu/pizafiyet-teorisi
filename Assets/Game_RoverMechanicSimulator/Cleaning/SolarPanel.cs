using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using UnityEditor.PackageManager;

public class SolarPanel : MonoBehaviour
{
    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [SerializeField] private TextMeshProUGUI uiText;

    private Texture2D dirtMaskTexture;

    private bool isFlipped;

    // private Animation solarAnimation;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;

    private void Awake()
    {
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_DirtMask", dirtMaskTexture);

        // solarAnimation = GetComponent<Animation>();

        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMaskTextureBase.width; x++)
        {
            for (int y = 0; y < dirtMaskTextureBase.height; y++)
            {
                dirtAmountTotal += dirtMaskTextureBase.GetPixel(x, y).g;
            }
        }

        dirtAmount = dirtAmountTotal;

        FunctionPeriodic.Create(() => { uiText.text = Mathf.RoundToInt(GetDirtAmount() * 100f) + "%"; }, .03f);
    }

    [SerializeField] private SpriteRenderer targetSpriteRenderer;


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray); // , Mathf.Infinity

            if (hit.collider != null)
                // if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                Debug.Log("hit: " + hit.point);
                
                Vector2 textureCoord = transform.InverseTransformPoint(hit.point);

                Debug.Log("texturee coooordinates: " + textureCoord);
                
                int pixelX = Mathf.RoundToInt((textureCoord.x + 0.5f) * dirtMaskTexture.width);
                int pixelY = Mathf.RoundToInt((textureCoord.y + 0.5f) * dirtMaskTexture.height);

                Debug.Log("texture coord: " + textureCoord);
                Debug.Log("pixels: " + pixelX + " " + pixelY);

                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
                //Debug.Log("UV: " + textureCoord + "; Pixels: " + paintPixelPosition);

                int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                int maxPaintDistance = 1;
                if (paintPixelDistance < maxPaintDistance)
                {
                    // Painting too close to last position
                    return;
                }

                lastPaintPixelPosition = paintPixelPosition;
                
                // // Paint Square in Dirt Mask
                // int squareSize = 32;
                // int pixelXOffset = pixelX - (dirtBrush.width / 2);
                // int pixelYOffset = pixelY - (dirtBrush.height / 2);
                //
                // for (int x = 0; x < squareSize; x++)
                // {
                //     for (int y = 0; y < squareSize; y++)
                //     {
                //         dirtMaskTexture.SetPixel(
                //             pixelXOffset + x,
                //             pixelYOffset + y,
                //             Color.black
                //         );
                //     }
                // }


                int pixelXOffset = pixelX - (dirtBrush.width / 2);
                int pixelYOffset = pixelY - (dirtBrush.height / 2);

                for (int x = 0; x < dirtBrush.width; x++)
                {
                    for (int y = 0; y < dirtBrush.height; y++)
                    {
                        Color pixelDirt = dirtBrush.GetPixel(x, y);
                        Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                        dirtAmount -= removedAmount;

                        dirtMaskTexture.SetPixel(
                            pixelXOffset + x,
                            pixelYOffset + y,
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                        );
                    }
                }


                dirtMaskTexture.Apply();
            }
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     isFlipped = !isFlipped;
        //     if (isFlipped)
        //     {
        //         solarAnimation.Play("SolarPanelFlip");
        //     }
        //     else
        //     {
        //         solarAnimation.Play("SolarPanelFlipBack");
        //     }
        // }
    }

    private float GetDirtAmount()
    {
        return this.dirtAmount / dirtAmountTotal;
    }
}