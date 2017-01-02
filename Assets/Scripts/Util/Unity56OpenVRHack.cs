using UnityEngine;

public class Unity56OpenVRHack : MonoBehaviour
{
	void Awake ()
    {
        // https://forum.unity3d.com/threads/steamvr-5-61-gl-end-requires-material-setpass-before.446167/
#if UNITY_5_6_OPENVR_HACK
        gameObject.AddComponent<SteamVR_UpdatePoses>();
#endif
    }
}
