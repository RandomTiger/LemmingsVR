using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedControllerExtended))]
[RequireComponent(typeof(SteamVR_TrackedObjectExtended))]
public class GvrOpenVRController : MonoBehaviour
{
    // extended SteamVR_TrackedController
    private SteamVR_TrackedControllerExtended trackedController;
    // extended SteamVR_TrackedObject
    private SteamVR_TrackedObjectExtended trackedObject;

    void Awake()
    {
        trackedController = GetComponent<SteamVR_TrackedControllerExtended>();
        trackedObject = GetComponent<SteamVR_TrackedObjectExtended>();

        UnityEngine.VR.InputTracking.disablePositionalTracking = true;
    }

    void Update()
    {
        for (int i = 0; i < SteamVR.connected.Length; i++)
        {
            if (SteamVR.connected[i])
            {
                trackedController.controllerIndex = (uint) i;
                trackedObject.index = (SteamVR_TrackedObject.EIndex) i;
            }
        }
    }

    /// Returns the controller's current connection state.
    public GvrConnectionState State
    {
        get
        {
            return trackedController != null ? GvrConnectionState.Connected : GvrConnectionState.Error;
        }
    }

    /// Returns the controller's current orientation in space, as a quaternion.
    /// The space in which the orientation is represented is the usual Unity space, with
    /// X pointing to the right, Y pointing up and Z pointing forward. Therefore, to make an
    /// object in your scene have the same orientation as the controller, simply assign this
    /// quaternion to the GameObject's transform.rotation.
    public Quaternion Orientation
    {
        get
        {
            return trackedController.transform.localRotation;
        }
    }

    /// Returns the controller's gyroscope reading. The gyroscope indicates the angular
    /// about each of its local axes. The controller's axes are: X points to the right,
    /// Y points perpendicularly up from the controller's top surface and Z lies
    /// along the controller's body, pointing towards the front. The angular speed is given
    /// in radians per second, using the right-hand rule (positive means a right-hand rotation
    /// about the given axis).
    public Vector3 Gyro
    {
        get
        {
            return trackedObject.angularVelocity;
        }
    }

    /// Returns the controller's accelerometer reading. The accelerometer indicates the
    /// effect of acceleration and gravity in the direction of each of the controller's local
    /// axes. The controller's local axes are: X points to the right, Y points perpendicularly
    /// up from the controller's top surface and Z lies along the controller's body, pointing
    /// towards the front. The acceleration is measured in meters per second squared. Note that
    /// gravity is combined with acceleration, so when the controller is resting on a table top,
    /// it will measure an acceleration of 9.8 m/s^2 on the Y axis. The accelerometer reading
    /// will be zero on all three axes only if the controller is in free fall, or if the user
    /// is in a zero gravity environment like a space station.
    public Vector3 Accel
    {
        get
        {
            return trackedObject.angularVelocity;
        }
    }

    /// If true, the user is currently touching the controller's touchpad.
    public bool IsTouching
    {
        get
        {
            return trackedController.padTouched;
        }
    }

    /// If true, the user just started touching the touchpad. This is an event flag (it is true
    /// for only one frame after the event happens, then reverts to false).
    public bool TouchDown
    {
        get
        {
            return trackedController.padTouchedDown;
        }
    }

    /// If true, the user just stopped touching the touchpad. This is an event flag (it is true
    /// for only one frame after the event happens, then reverts to false).
    public bool TouchUp
    {
        get
        {
            return trackedController.padTouchedUp;
        }
    }

    public Vector2 TouchPos
    {
        get
        {
            Vector2 converted = trackedController.touchPos;
            // invert
            converted.y *= -1;
            // scale
            converted *= 0.5f;
            // shift centre
            converted.x += 0.5f;
            converted.y += 0.5f;
            return converted;
        }
    }

    /// If true, the click button (touchpad button) is currently being pressed. This is not
    /// an event: it represents the button's state (it remains true while the button is being
    /// pressed).
    public bool ClickButton
    {
        get
        {
            return trackedController.padPressed;
        }
    }

    /// If true, the click button (touchpad button) was just pressed. This is an event flag:
    /// it will be true for only one frame after the event happens.
    public bool ClickButtonDown
    {
        get
        {
            return trackedController.padPressedDown;
        }
    }

    /// If true, the click button (touchpad button) was just released. This is an event flag:
    /// it will be true for only one frame after the event happens.
    public bool ClickButtonUp
    {
        get
        {
            return trackedController.padPressedUp;
        }
    }

    /// If true, the app button (touchpad button) is currently being pressed. This is not
    /// an event: it represents the button's state (it remains true while the button is being
    /// pressed).
    public bool AppButton
    {
        get
        {
            return trackedController.menuPressed;
        }
    }

    /// If true, the app button was just pressed. This is an event flag: it will be true for
    /// only one frame after the event happens.
    public bool AppButtonDown
    {
        get
        {
            return trackedController.MenuButtonDown;
        }
    }

    /// If true, the app button was just released. This is an event flag: it will be true for
    /// only one frame after the event happens.
    public bool AppButtonUp
    {
        get
        {
            return trackedController.MenuButtonUp;
        }
    }
}