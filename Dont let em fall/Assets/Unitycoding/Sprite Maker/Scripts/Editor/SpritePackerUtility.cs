using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class SpritePackerUtility  {

	public static Rect[] AutoPack(ref Texture2D atlas, Texture2D[] textures, int padding, int maxAtlasSize){
		for(int i=0;i< textures.Length;i++){
			textures[i].SetReadable(true);
		}
		if (atlas == null) {
			atlas= new Texture2D(32,32,TextureFormat.ARGB32,false);
			atlas.Clear();
		}
		Rect[] r=atlas.PackTextures(textures,padding,maxAtlasSize);
		List<Rect> rects = new List<Rect> ();
		for(int i=0;i<r.Length;i++){
			rects.Add(new Rect(r[i].x*atlas.width,r[i].y*atlas.height,r[i].width*atlas.width,r[i].height*atlas.height));
		}
		return rects.ToArray ();
	}
	
	public static Rect[] GridPack(ref Texture2D atlas,Texture2D[] textures, int padding){
		for(int i=0;i< textures.Length;i++){
			textures[i].SetReadable(true);
		}
		List<Rect> rects = new List<Rect> ();
		Vector2 gridSize = TextureUtility.GetMaxTextureSize (textures);
		int max=(int)(gridSize.x>gridSize.y?gridSize.x:gridSize.y);

		int rows = (int)Mathf.Ceil (Mathf.Log (textures.Length) / Mathf.Log (2));
		int columns = Mathf.CeilToInt((float)textures.Length / rows);

		int requiredWidth = rows* max+(rows-1)*padding;
		int requiredHeight = columns * max + (columns - 1) * padding;

		int atlasWidth = 32;
		int atlasHeight = 32;
		while (atlasWidth < requiredWidth) {
			atlasWidth*=2;
		}

		while (atlasHeight < requiredHeight) {
			atlasHeight*=2;
		}

		atlas= new Texture2D(atlasWidth,atlasHeight,TextureFormat.ARGB32,false);
		atlas.Clear();
		int cnt = 0;
		for(int y = 0; y < columns; y++)
		{
			for(int x = 0; x < rows; x++)
			{
				Texture2D texture=textures[cnt];
				Rect rect=new Rect (x * max +  x*padding, y * max + y*padding, max, max);
				rects.Add (rect);
				atlas.SetPixels((int)(rect.x + (max-texture.width)*0.5f),
				                (int)(rect.y + (max-texture.height)*0.5f),
				                texture.width,
				                texture.height,
				                texture.GetPixels());
				cnt++;
				if(cnt >= textures.Length){
					break;
				}
			}
		}

		atlas.Apply ();
		return rects.ToArray ();
	}

	public static Rect[] GridPack(ref Texture2D atlas,Texture2D[] textures, int padding, int rows,int columns){
		for(int i=0;i< textures.Length;i++){
			textures[i].SetReadable(true);
		}
		List<Rect> rects = new List<Rect> ();
		Vector2 gridSize = TextureUtility.GetMaxTextureSize (textures);
		int max=(int)(gridSize.x>gridSize.y?gridSize.x:gridSize.y);

		//int rows = (int)Mathf.Ceil (Mathf.Log (textures.Length) / Mathf.Log (2));
		//int columns = Mathf.CeilToInt((float)textures.Length / rows);

		int requiredWidth = rows* max+(rows-1)*padding;
		int requiredHeight = columns * max + (columns - 1) * padding;

		int atlasWidth = 32;
		int atlasHeight = 32;
		while (atlasWidth < requiredWidth) {
			atlasWidth*=2;
		}

		while (atlasHeight < requiredHeight) {
			atlasHeight*=2;
		}

		atlas= new Texture2D(atlasWidth,atlasHeight,TextureFormat.ARGB32,false);
		atlas.Clear();
		int cnt = 0;
		for(int y = 0; y < columns; y++)
		{
			for(int x = 0; x < rows; x++)
			{
				Texture2D texture=textures[cnt];
				Rect rect=new Rect (x * max +  x*padding, y * max + y*padding, max, max);
				rects.Add (rect);
				atlas.SetPixels((int)(rect.x + (max-texture.width)*0.5f),
					(int)(rect.y + (max-texture.height)*0.5f),
					texture.width,
					texture.height,
					texture.GetPixels());
				cnt++;
				if(cnt >= textures.Length){
					break;
				}
			}
		}

		atlas.Apply ();
		return rects.ToArray ();
	}
}
