using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorManager : NetworkBehaviour
{
	public static ColorManager globalInstance;

	public List<Color> colorPool = new List<Color> { Color.red, Color.blue, Color.yellow, Color.green, Color.cyan, Color.magenta
        , new Color(1, 0.5f, 0), new Color(0, 0, 0.6f) }; // Orange, Indigo
	public List<Color> usedColors;

	private void Start() {
		if (globalInstance == null) {
			globalInstance = this;
		} else if (globalInstance != this) {
			Debug.LogError("Multiple instances of ColorManager found");
		}
	}

	public Color GetColor() {
		if (colorPool.Count >= 1) {
			int index = Random.Range(0, colorPool.Count-1);
			Color color = colorPool[index];
			usedColors.Add(color);
			colorPool.Remove(color);
			return color;
		} else {
			Debug.LogError("No more colors available");
			return Color.clear;
		}
	}
}
