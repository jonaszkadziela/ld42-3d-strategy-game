using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class TextureUtility
{
	public static Texture2D SaveTexture (this Texture2D texture, string path)
	{
		texture.SetReadable (true);
		texture.SetCompression (TextureImporterCompression.Uncompressed);
		byte[] bytes = texture.EncodeToPNG (); 
		System.IO.File.WriteAllBytes (path, bytes);
		AssetDatabase.Refresh ();
		Texture2D mTexture = (Texture2D)AssetDatabase.LoadAssetAtPath (path.Substring (path.IndexOf ("Assets")), typeof(Texture2D));
		mTexture.SetTextureType (TextureImporterType.Sprite);
		return mTexture;
	}

	public static void Clear (this Texture2D texture)
	{
		texture.SetReadable (true);
		Color32[] colors = new Color32[texture.width * texture.height];
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = Color.clear;
		}
		texture.SetPixels32 (colors);
		texture.Apply ();
	}

	public static Vector2 GetMaxTextureSize (Texture2D[] textures)
	{
		Vector2 maxSize = Vector2.zero;
		for (int i = 0; i < textures.Length; i++) {
			if (maxSize.x < textures [i].width) {
				maxSize.x = textures [i].width;
			}
			if (maxSize.y < textures [i].height) {
				maxSize.y = textures [i].height;
			}
		}
		return maxSize;
	}

	public static void SetCompression (this Texture2D texture, TextureImporterCompression compression)
	{
		TextureImporter tempImporter = (TextureImporter)AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (texture));
		if (tempImporter != null) {
			tempImporter.textureType = TextureImporterType.Default;
			tempImporter.textureCompression = compression;
			AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (texture));
			AssetDatabase.Refresh ();
		}
	}

	public static TextureImporterType GetTextureType (this Texture2D texture)
	{
		TextureImporter tempImporter = (TextureImporter)AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (texture));
		if (tempImporter != null) {
			return tempImporter.textureType;
		}
		return TextureImporterType.Default;
	}

	
	public static SpriteImportMode GetSpriteMode (this Texture2D texture)
	{
		TextureImporter tempImporter = (TextureImporter)AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (texture));
		if (tempImporter != null) {
			return tempImporter.spriteImportMode;
		}
		return SpriteImportMode.None;
	}

	
	public static void SetSpriteMode (this Texture2D texture, SpriteImportMode mode)
	{
		TextureImporter tempImporter = (TextureImporter)AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (texture));
		if (tempImporter != null) {
			Selection.activeObject = null;
			tempImporter.textureType = TextureImporterType.Sprite;
			tempImporter.spriteImportMode = mode;
			AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (texture));
			AssetDatabase.Refresh ();
		}
	}

	public static void SetTextureType (this Texture2D texture, TextureImporterType type)
	{
		TextureImporter tempImporter = (TextureImporter)AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (texture));
		if (tempImporter != null) {
			Selection.activeObject = null;
			tempImporter.textureType = type;
			AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (texture));
			AssetDatabase.Refresh ();
		}
	}

	public static void SetReadable (this Texture2D texture, bool isReadable)
	{
		TextureImporter tempImporter = (TextureImporter)AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (texture));
		if (tempImporter != null) {
			tempImporter.textureType = TextureImporterType.Default;
			tempImporter.isReadable = isReadable;
			AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (texture));
			AssetDatabase.Refresh ();
		}
	}

	public static Texture2D[] TrimSymetric (Texture2D[] textures, int padding)
	{

		int xMin = int.MaxValue;
		int xMax = int.MinValue;
		int yMin = int.MaxValue;
		int yMax = int.MinValue;
		for (int i = 0; i < textures.Length; i++) {
			Texture2D texture = textures [i];
			texture.SetReadable (true);
			int[] bounds = GetBounds (textures [i]);
			if (bounds [0] < xMin) {
				xMin = bounds [0];
			}
			if (bounds [1] > xMax) {
				xMax = bounds [1];
			}

			if (bounds [2] < yMin) {
				yMin = bounds [2];
			}
			if (bounds [3] > yMax) {
				yMax = bounds [3];
			}

		}
	
		List<Texture2D> list = new List<Texture2D> ();
		for (int i = 0; i < textures.Length; i++) {
			Texture2D tex = new Texture2D (xMax - xMin + padding * 2, yMax - yMin + padding * 2, TextureFormat.ARGB32, false);
			for (int y = 0; y < tex.height; ++y) { 
				for (int x = 0; x < tex.width; ++x) { 
					Color color = textures [i].GetPixel (xMin + x, yMin + y);
					tex.SetPixel (x + padding, y + padding, color);
				}
			}
			tex.Apply ();
			tex.name = textures [i].name;
			tex.alphaIsTransparency = true;
			list.Add (tex);
		}
		return list.ToArray ();
	}

	public static Texture2D Trim (this Texture2D texture, int padding)
	{
		texture.SetReadable (true);
		int[] bounds = GetBounds (texture);
		int xMin = bounds [0],
		xMax = bounds [1],
		yMin = bounds [2],
		yMax = bounds [3];
		
		Texture2D tex = new Texture2D (xMax - xMin + padding * 2, yMax - yMin + padding * 2, TextureFormat.ARGB32, false);
		for (int y = 0; y < tex.height; ++y) { 
			for (int x = 0; x < tex.width; ++x) { 
				Color color = texture.GetPixel (xMin + x, yMin + y);
				tex.SetPixel (x + padding, y + padding, color);
			}
		}
		tex.Apply ();
		return tex;
	}

	public static int[] GetBounds (Texture2D texture)
	{
		int width = texture.width;
		int height = texture.height;
		int xMin = int.MaxValue,
		xMax = int.MinValue,
		yMin = int.MaxValue,
		yMax = int.MinValue;
		// Find xMin
		for (int x = 0; x < width; x++) {
			bool stop = false;
			for (int y = 0; y < height; y++) {
				float alpha = texture.GetPixel (x, y).a;   
				if (alpha != 0) {
					xMin = x;
					stop = true;
					break;
				}
			}
			if (stop)
				break;
		}
		
		// Find yMin
		for (int y = 0; y < height; y++) {
			bool stop = false;
			for (int x = xMin; x < width; x++) {
				float alpha = texture.GetPixel (x, y).a;
				if (alpha != 0) {
					yMin = y;
					stop = true;
					break;
				}
			}
			if (stop)
				break;
		}
		
		// Find xMax
		for (int x = width; x >= xMin; x--) {
			bool stop = false;
			for (int y = yMin; y < height; y++) {
				float alpha = texture.GetPixel (x, y).a;
				if (alpha != 0) {
					xMax = x;
					stop = true;
					break;
				}
			}
			if (stop)
				break;
		}
		
		// Find yMax
		for (int y = height; y >= yMin; y--) {
			bool stop = false;
			for (int x = xMin; x <= xMax; x++) {
				float alpha = texture.GetPixel (x, y).a;
				if (alpha != 0) {
					yMax = y;
					stop = true;
					break;
				}
			}
			if (stop)
				break;
		}
		return new int[]{ xMin, xMax, yMin, yMax };
	}
}
