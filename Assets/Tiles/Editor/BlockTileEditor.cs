using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(BlockTile))]
public class BlockTileEditor : Editor {
	public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) { // I have almost no idea what's actually going on here, but it works*
		Tile t = AssetDatabase.LoadAssetAtPath<Tile>(assetPath);
		if (t != null && t.sprite != null) {
			Texture2D spritePreview = AssetPreview.GetMiniThumbnail(t.sprite.texture);

			RenderTexture workaround = new RenderTexture(spritePreview.width, spritePreview.height, 8);
			Graphics.Blit(spritePreview, workaround);

			Texture2D texture = new Texture2D(spritePreview.width, spritePreview.height, TextureFormat.RGB24, false);

			Rect rectReadPicture = new Rect(0, 0, spritePreview.width, spritePreview.height);

			RenderTexture.active = workaround;

			// Read pixels
			texture.ReadPixels(rectReadPicture, 0, 0);
			texture.Apply();

			RenderTexture.active = null; // added to avoid errors 

			Color[] pixels = texture.GetPixels();

			for (int i = 0; i < pixels.Length; i++) {
				pixels[i] = pixels[i] * t.color; // Tint
			}

			Texture2D preview = new Texture2D(spritePreview.width, spritePreview.height);

			preview.SetPixels(pixels);
			preview.Apply();

			return preview;
		}
		return null;
	} // It doesn't work very well
}
