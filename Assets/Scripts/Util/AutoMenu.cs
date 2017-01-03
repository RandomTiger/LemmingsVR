using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ReadSceneNames))]
public class AutoMenu : MonoBehaviour
{
    public Font Font;

	void Start ()
    {
        Image [] images = GetComponentsInChildren<Image>(false);
        ReadSceneNames readNames = GetComponent<ReadSceneNames>();
        string [] names = readNames.scenes;
        int imageCount = 0;

        for(int i = 0; i < names.Length; i++)
        {
            Image image = images[imageCount++];
            GameObject newObj = new GameObject(names[i]);
            newObj.transform.parent = image.transform;
            newObj.transform.localPosition = Vector3.zero;

            Text text = newObj.AddComponent<Text>();
            text.text = names[i];
            text.color = Color.black;
            text.font = Font;
            text.fontSize = 15;
            text.transform.localScale = Vector3.one * 10f;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;

            text.GetComponent<RectTransform>().sizeDelta = new Vector2(45, 45);
        }
	}
}

