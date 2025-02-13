using UnityEngine;

public class Utils
{
    public static bool TryGetComponentInChildren<T>(GameObject ctx, out T result) where T : Component
    {
        foreach (Transform child in ctx.transform)
        {
            if (child.TryGetComponent(out T component))
            {
                result = component;
                return true;
            }
        }
        result = null;
        return false;
    }
}
