//========= Copyright 2014, Valve Corporation, All rights reserved. ===========
//
// Purpose: For controlling in-game objects with tracked devices.
//
//=============================================================================

using UnityEngine;
using Valve.VR;

public class SteamVR_TrackedObjectExtended : SteamVR_TrackedObject
{
    public Vector3 velocity = Vector3.zero;
    public Vector3 angularVelocity = Vector3.zero;

    private void OnNewPoses(params object[] args)
	{
		if (index == EIndex.None)
			return;

		var i = (int)index;

        isValid = false;
		var poses = (Valve.VR.TrackedDevicePose_t[])args[0];
		if (poses.Length <= i)
			return;

		if (!poses[i].bDeviceIsConnected)
			return;

		if (!poses[i].bPoseIsValid)
			return;

        isValid = true;

		var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

		if (origin != null)
		{
			pose = new SteamVR_Utils.RigidTransform(origin) * pose;
			pose.pos.x *= origin.localScale.x;
			pose.pos.y *= origin.localScale.y;
			pose.pos.z *= origin.localScale.z;
			transform.position = pose.pos;
			transform.rotation = pose.rot;
        }
		else
		{
			transform.localPosition = pose.pos;
			transform.localRotation = pose.rot;
		}

        velocity.x = poses[i].vVelocity.v0;
        velocity.y = poses[i].vVelocity.v1;
        velocity.z = poses[i].vVelocity.v2;

        angularVelocity.x = poses[i].vAngularVelocity.v0;
        angularVelocity.y = poses[i].vAngularVelocity.v1;
        angularVelocity.z = poses[i].vAngularVelocity.v2;
    }

    void OnEnable()
    {
        var render = SteamVR_Render.instance;
        if (render == null)
        {
            enabled = false;
            return;
        }

        SteamVR_Utils.Event.Listen("new_poses", OnNewPoses);
    }

    void OnDisable()
    {
        SteamVR_Utils.Event.Remove("new_poses", OnNewPoses);
        isValid = false;
    }
}

