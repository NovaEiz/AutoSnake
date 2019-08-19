using UnityEngine;

namespace AutoSnake.Engine {

public static class Vector3Ext {

	public static Vector3 ConvertToFloat(this Vector3Int point) {
		var x = (float)point.x;
		var y = (float)point.y;
		var z = (float)point.z;
		return new Vector3(x, y, z);
	}
	public static Vector3Int ConvertToInt(this Vector3 point) {
		var x = (int)Mathf.Round(point.x);
		var y = (int)Mathf.Round(point.y);
		var z = (int)Mathf.Round(point.z);
		return new Vector3Int(x, y, z);
	}
	
	public static Vector2Int ConvertToInt(this Vector2 point) {
		var x = (int)Mathf.Round(point.x);
		var y = (int)Mathf.Round(point.y);
		return new Vector2Int(x, y);
	}
	public static Vector3Int ConvertXzToXyz(this Vector2Int point) {
		return new Vector3Int(point.x, 0, point.y);
	}

	public static Vector2 ConvertXyzToXz(this Vector3 point) {
		return new Vector2(point.x, point.z);
	}
}

}