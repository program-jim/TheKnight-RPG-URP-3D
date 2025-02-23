using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PGJ Custom extension method collection
/// </summary>
public static class ExtensionMethod
{
    /// <summary>
    /// The standard dot value.
    /// </summary>
    private const float dotThreshold = 0.5f;
    
    /// <summary>
    /// Check if the target object is faced by us.
    /// </summary>
    /// <param name="transform">Our transform</param>
    /// <param name="target">target's transform</param>
    /// <returns></returns>
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);

        return dot >= dotThreshold;
    }
}
