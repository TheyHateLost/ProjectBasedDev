using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public static class CustomUtils 
{
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
    
    /// <summary>
    /// Checks to see if a layer exists in the layer mask
    /// </summary>
    public static bool Contains(this LayerMask mask, int layer) => (mask.value & (1 << layer)) != 0;
    
    public static string FloatToString(float value, int decimalPlaces = 2)
    {
        string format = "F" + decimalPlaces;
        return value.ToString(format);
    }

    /// <summary>
    /// Calculates the movement input relative to the camera's orientation.
    /// The return result is normalized.
    /// </summary>
    public static Vector3 GetCameraBasedMoveInput(Transform cameraTransform, Vector2 moveInput)
    {
        if (cameraTransform == null)
            return moveInput;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0; // Ignore vertical component
        right.y = 0; // Ignore vertical component

        forward.Normalize();
        right.Normalize();

        return (forward * moveInput.y + right * moveInput.x).normalized;
    }

    /// <summary>
    /// Calculates the jump force from the jump height using the physics equation
    /// </summary>
    public static float GetJumpForce(float jumpHeight, float gravity) => Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);
    
    /// <summary>
    /// Randomly pulls <paramref name="amount"/> from a grab bag. Never pulls null values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static IEnumerable<T> Grab<T>(this List<T> collection, int amount)
    {
        if (amount < 0 || amount > collection.Count) throw new System.IndexOutOfRangeException();
        if (amount == 0) return default;
        int sizeBefore = collection.Count;
        var grabBag = collection.Where(t => t != null).OrderBy(t => Random.value).ToList();
        if (amount > grabBag.Count) throw new System.IndexOutOfRangeException();
        return grabBag.Take(amount);
    }

    public static int RandomIndex<T>(this IList<T> list)
    {
        return Random.Range(0, list.Count);
    }

    public static T RandomElement<T>(this IList<T> list)
    {
        return list[RandomIndex(list)];
    }

    public static void ForEach<T>(this T[] arr, System.Action<T> action)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            action.Invoke(arr[i]);
        }
    }
}