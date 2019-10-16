using UnityEngine;
using UnityEditor;
using System.Collections;

public static class SpriteMakerMenu  {
	[MenuItem("Tools/Unitycoding/Sprite Maker/Sprite Baker",false)]
	public static void OpenSpriteBakerEditor()
	{
		SpriteBakerEditor.ShowWindow ();
	}

	[MenuItem("Tools/Unitycoding/Sprite Maker/Sprite Packer",false)]
	public static void OpenSpritePackerEditor()
	{
		SpritePackerEditor.ShowWindow ();
	}
}
