using UnityEngine;

public class BahaviourSelection : MonoBehaviour
{
    public Entity.Behaviour[] Behaviours;
    public GameObject[] Visuals;
    int selection;

    void OnEnable ()
    {
        SwipeDetector.Instance.OnSwipe += OnSwipe;
    }

    void OnDisable()
    {
        SwipeDetector.Instance.OnSwipe -= OnSwipe;
    }

    void OnSwipe(SwipeDetector.Swipe direction)
    {
        switch (direction)
        {
            case SwipeDetector.Swipe.Left:
                selection = (selection - 1 + Behaviours.Length) % Behaviours.Length;
                break;
            case SwipeDetector.Swipe.Right:
                selection = (selection + 1) % Behaviours.Length;
                break;
            default:
                return;
        }

        for (int i = 0; i < Visuals.Length; i++)
        {
            if (Visuals[i] == null)
            {
                continue;
            }

            Visuals[i].SetActive(i == selection);
        }
    }
}
