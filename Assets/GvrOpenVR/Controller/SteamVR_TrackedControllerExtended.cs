using UnityEngine;
using Valve.VR;

public class SteamVR_TrackedControllerExtended : SteamVR_TrackedController
{
    public bool padTouchedDown;
    public bool padTouchedUp;

    public bool MenuButtonDown;
    public bool MenuButtonUp;

    public bool padPressedUp;
    public bool padPressedDown;

    public bool grippedUp;
    public bool grippedDown;

    public bool triggerPressedUp;
    public bool triggerPressedDown;

    public Vector2 touchPos = Vector2.zero;

    void Update()
    {
		var system = OpenVR.System;
		if (system != null && system.GetControllerState(controllerIndex, ref controllerState))
		{
            touchPos.x = controllerState.rAxis0.x;
            touchPos.y = controllerState.rAxis0.y;

            ulong trigger = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Trigger));
            if (trigger > 0L && !triggerPressed)
            {
                triggerPressedDown = true;
                triggerPressed = true;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnTriggerClicked(e);
            }
            else if (trigger == 0L && triggerPressed)
            {
                triggerPressedUp = true;
                triggerPressed = false;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnTriggerUnclicked(e);
            }
            else
            {
                triggerPressedUp = false;
                triggerPressedDown = false;
            }

            ulong grip = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_Grip));
            if (grip > 0L && !gripped)
            {
                grippedDown = true;
                gripped = true;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnGripped(e);
            }
            else if (grip == 0L && gripped)
            {
                grippedUp = true;
                gripped = false;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnUngripped(e);
            }
            else
            {
                grippedUp = false;
                grippedDown = false;
            }

            ulong pad = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Touchpad));
            if (pad > 0L && !padPressed)
            {
                padPressedDown = true;
                padPressed = true;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnPadClicked(e);
            }
            else if (pad == 0L && padPressed)
            {
                padPressedUp = true;
                padPressed = false;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnPadUnclicked(e);
            }
            else
            {
                padPressedUp = false;
                padPressedDown = false;
            }

            ulong menu = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_ApplicationMenu));
            if (menu > 0L && !menuPressed)
            {
                MenuButtonDown = true;
                menuPressed = true;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnMenuClicked(e);
            }
            else if (menu == 0L && menuPressed)
            {
                MenuButtonUp = true;
                menuPressed = false;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnMenuUnclicked(e);
            }
            else
            {
                MenuButtonDown = false;
                MenuButtonUp = false;
            }

            pad = controllerState.ulButtonTouched & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Touchpad));
            if (pad > 0L && !padTouched)
            {
                padTouchedDown = true;
                padTouched = true;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnPadTouched(e);
            }
            else if (pad == 0L && padTouched)
            {
                padTouchedUp = true;
                padTouched = false;
                ClickedEventArgs e;
                e.controllerIndex = controllerIndex;
                e.flags = (uint)controllerState.ulButtonPressed;
                e.padX = controllerState.rAxis0.x;
                e.padY = controllerState.rAxis0.y;
                OnPadUntouched(e);
            }
            else
            {
                padTouchedDown = false;
                padTouchedUp = false;
            }
        }
    }
}
