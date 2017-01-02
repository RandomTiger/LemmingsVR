using UnityEngine;

public class SwipeDetector : Singleton<SwipeDetector>
{
    public delegate void OnSwipeCallback(Swipe direction);
    public event OnSwipeCallback OnSwipe;

    public enum Swipe
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    Vector2 startPos;
    Vector2 LastPos;
    public Vector2 velocity = new Vector2();

    float threshold = 0.25f;

    public Swipe bias = Swipe.None;
    public float timer;

    void Update ()
    {
        if(GvrController.TouchDown)
        {
            startPos = LastPos = GvrController.TouchPos;
        }
        else if(GvrController.IsTouching)
        {
            Vector2 diff = GvrController.TouchPos - LastPos;
            velocity += diff;

            if(timer > 0.1f)
            {
                if(Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                {
                    bias = velocity.x > 0 ? Swipe.Right : Swipe.Left;
                }
                else
                {
                    bias = velocity.y < 0 ? Swipe.Up : Swipe.Down;
                }
            }

            timer += Time.deltaTime;
            LastPos = GvrController.TouchPos;
        }
        else if (GvrController.TouchUp)
        {
            float totalDiffMagSqr = (GvrController.TouchPos - startPos).sqrMagnitude;
            if(bias != Swipe.None)
            {
                float maxVelocity = Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y));
                float minVelocity = Mathf.Min(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y));

                if (maxVelocity < minVelocity * 2)
                {
                    Debug.Log("Swipe fail: multiple directions");
                }
                else if (totalDiffMagSqr < threshold * threshold)
                {
                    Debug.Log("Swipe fail: " + bias.ToString() + " " + Mathf.Sqrt(totalDiffMagSqr));
                }
                else
                {
                    if (OnSwipe != null)
                    {
                        OnSwipe(bias);
                    }
                    Debug.Log("Swipe detected: " + bias.ToString());
                }
            }

            bias = Swipe.None;
            timer = 0;

            velocity = Vector2.zero;
        }

    }
}
