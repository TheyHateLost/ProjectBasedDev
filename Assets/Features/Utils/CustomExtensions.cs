using System;
using UnityEngine;

public static class CustomExtensions
{
    /// <summary>
    /// Helper method to set the alpha value of a Unity UI Image component.
    /// Needed because Image.color.a = alpha does not work.
    /// </summary>
    public static void SetImageAlpha(this UnityEngine.UI.Image image, float alpha)
    {
        if (image == null)
            return;

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
    
    /// <summary>
    /// Null propogation check for Unity object methods.
    /// Equivalent to 'obj?.Method()' but Unity safe.
    /// </summary>
    public static T IfNotNullThenCall<T>(this T obj, Action<T> action) where T : UnityEngine.Object
    {
        if (obj != null)
            action(obj);
        return obj;
    }

    /// <summary>
    /// Null propogation check for Unity object methods/properties that return a value.
    /// Equivalent to 'obj?.Method()' or 'obj?.Property' but Unity safe.
    /// </summary>
    public static TResult IfNotNullThenGet<T, TResult>(this T obj, Func<T, TResult> func) where T : UnityEngine.Object
    {
        return obj != null ? func(obj) : default;
    }
    
    /// <summary>
    /// Returns a new Vector3 with one component (x, y, or z) set to a new value.
    /// Vector3.x = x does not work as expected in Unity, so this is a workaround.
    /// </summary>
    public static Vector3 WithX(this Vector3 vector, float x) => new Vector3(x, vector.y, vector.z);
    /// <summary>
    /// Returns a new Vector3 with one component (x, y, or z) set to a new value.
    /// Vector3.z = z does not work as expected in Unity, so this is a workaround.
    /// </summary>
    public static Vector3 WithY(this Vector3 vector, float y) => new Vector3(vector.x, y, vector.z);
    /// <summary>
    /// Returns a new Vector3 with one component (x, y, or z) set to a new value.
    /// Vector3.z = z does not work as expected in Unity, so this is a workaround.
    /// </summary>
    public static Vector3 WithZ(this Vector3 vector, float z) => new Vector3(vector.x, vector.y, z);
    
    public static void SetLayerIncludingChildren(this GameObject root, int layer)
    {
        var transforms = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].gameObject.layer = layer;
        }
    }
}