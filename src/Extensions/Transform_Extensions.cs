using UnityEngine;

namespace visible_passengers.Extensions;

public static class Transform_Extensions
{
	public static string GetPath(this Transform transform) {
		if (transform.parent == null)
			return "/" + transform.name;
		return transform.parent.GetPath() + "/" + transform.name;
	}
}