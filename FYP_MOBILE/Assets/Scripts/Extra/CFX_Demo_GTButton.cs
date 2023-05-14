using UnityEngine;
using UnityEngine.UI;

public class CFX_Demo_GTButton : MonoBehaviour
{
	public Color NormalColor = new Color32(128, 128, 128, 128);

	public Color HoverColor = new Color32(128, 128, 128, 128);

	public string Callback;

	public GameObject Receiver;

	private Rect CollisionRect;

	private bool Over;

	private void Awake()
	{
	}

	private void Update()
	{
		if (CollisionRect.Contains(Input.mousePosition))
		{
			GetComponent<Image>().color = HoverColor;
			if (Input.GetMouseButtonDown(0))
			{
				OnClick();
			}
		}
		else
		{
			GetComponent<Image>().color = NormalColor;
		}
	}

	private void OnClick()
	{
		Receiver.SendMessage(Callback);
	}
}
