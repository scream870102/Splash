using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CJStudio.Splash {
    [RequireComponent (typeof (MeshRenderer))]
    class PaintableObj : MonoBehaviour {
        Texture2D paintTex = null;
        Material material = null;
        Texture2D mainTex = null;
        void Awake ( ) {
            material = GetComponent<MeshRenderer> ( ).material;
            mainTex = material.mainTexture as Texture2D;
            gameObject.tag = "Paintable";
        }
        void Start ( ) {
            paintTex = new Texture2D (mainTex.width, mainTex.height);
            Color[ ] cols = paintTex.GetPixels ( );
            for (int i = 0; i < cols.Length; i++) {
                cols[i] = Color.black;
            }
            paintTex.SetPixels (cols);
            paintTex.Apply ( );
            material.SetTexture ("_SplashTex", paintTex);
        }

        /// <summary>
        /// Splash ink on Object
        /// </summary>
        /// <param name="texCoord">center of the ink in textureCoord Range 0f~1f</param>
        /// <param name="color">ink color</param>
        /// <param name="splashTex">shape of splash</param>
        /// <param name="splashColors">All color from splashTex. Try to get color first to reduce the number of calling GetPixels</param>
        public void Paint (Vector2 texCoord, Color color, Texture2D splashTex, Color32[ ] splashColors) {
            //get pixels return an 1d-array and start at left-bottom row by row
            //make start point at left-bottom
            Vector2 pixelUV;
            pixelUV.x = texCoord.x * paintTex.width - splashTex.width * .5f;
            pixelUV.y = texCoord.y * paintTex.height + splashTex.height * .5f;
            for (int i = 0; i < splashTex.height; i++) {
                for (int j = 0; j < splashTex.width; j++) {
                    if (splashColors[i * splashTex.width + j].a == 0)
                        continue;
                    paintTex.SetPixel ((int) pixelUV.x + j, (int) pixelUV.y - i, color);
                }
            }
            paintTex.Apply ( );
        }
    }
}