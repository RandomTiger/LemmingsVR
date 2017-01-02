// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// The controller is not available for versions of Unity without the
// GVR native integration.
#if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)

using UnityEngine;
using System.Collections;

/// Implementation of IGvrPointer for a laser pointer visual.
/// This script should be attached to the controller object.
/// The laser visual is important to help users locate their cursor
/// when its not directly in their field of view.
[RequireComponent(typeof(LineRenderer))]
public class GameLaserPointer : GvrBasePointer
{

    /// Small offset to prevent z-fighting of the reticle (meters).
    private const float Z_OFFSET_EPSILON = 0.1f;

    /// Size of the reticle in meters as seen from 1 meter.
    private const float RETICLE_SIZE = 0.01f;

    private LineRenderer lineRenderer;
    private bool isPointerIntersecting;
    private Vector3 pointerIntersection;
    private Ray pointerIntersectionRay;

    /// Color of the laser pointer including alpha transparency
    public Color laserColor = new Color(1.0f, 1.0f, 1.0f, 0.25f);

    /// Maximum distance of the pointer (meters).
    [Range(0.0f, 10.0f)]
    public float laserDistance = 0.75f;

    /// Maximum distance of the reticle (meters).
    [Range(0.4f, 10.0f)]
    public float maxReticleDistance = 2.5f;

    public GameObject reticle;

    void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        Ray ray = GetRay();

        // Set the line renderer positions.
        lineRenderer.SetPosition(0, ray.origin);

        Vector3 lineEndPoint;
        RaycastHit hitInfo;
        int mask = 1 << LayerMask.NameToLayer("Entity") | 1 << LayerMask.NameToLayer("Default");
        if (Physics.Raycast(GetRay(), out hitInfo, float.PositiveInfinity, mask))
        {
            lineEndPoint = hitInfo.point;
            laserDistance = (ray.origin - lineEndPoint).magnitude;
        }
        else
        {
            lineEndPoint = transform.position + (transform.forward * laserDistance);
        }

        lineRenderer.SetPosition(1, lineEndPoint);
        UpdateReticle(lineEndPoint);

        // Adjust transparency
        float alpha = GvrArmModel.Instance.alphaValue;
        lineRenderer.startColor = lineRenderer.endColor = Color.white;
    }

    void UpdateReticle(Vector3 pos)
    {
        // Set the reticle's position and scale
        if (reticle != null)
        {
            reticle.transform.position = pos;

            float reticleDistanceFromCamera = (reticle.transform.position - Camera.main.transform.position).magnitude;
            float scale = RETICLE_SIZE * reticleDistanceFromCamera;
            reticle.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    Ray GetRay()
    {
        return new Ray(transform.position, transform.forward);
    }

    public override void OnInputModuleEnabled()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }
    }

    public override void OnInputModuleDisabled()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    public override void OnPointerEnter(GameObject targetObject, Vector3 intersectionPosition,
        Ray intersectionRay, bool isInteractive)
    {
        pointerIntersection = intersectionPosition;
        pointerIntersectionRay = intersectionRay;
        isPointerIntersecting = true;
    }

    public override void OnPointerHover(GameObject targetObject, Vector3 intersectionPosition,
        Ray intersectionRay, bool isInteractive)
    {
        pointerIntersection = intersectionPosition;
        pointerIntersectionRay = intersectionRay;
    }

    public override void OnPointerExit(GameObject targetObject)
    {
        pointerIntersection = Vector3.zero;
        pointerIntersectionRay = new Ray();
        isPointerIntersecting = false;
    }

    public override void OnPointerClickDown()
    {
        // User has performed a click on the target.  In a derived class, you could
        // handle visual feedback such as laser or cursor color changes here.
    }

    public override void OnPointerClickUp()
    {
        // User has released a click from the target.  In a derived class, you could
        // handle visual feedback such as laser or cursor color changes here.
    }

    public override float GetMaxPointerDistance()
    {
        return maxReticleDistance;
    }

    public override void GetPointerRadius(out float innerRadius, out float outerRadius)
    {
        innerRadius = 0.0f;
        outerRadius = 0.0f;
    }
}

#endif  // UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
