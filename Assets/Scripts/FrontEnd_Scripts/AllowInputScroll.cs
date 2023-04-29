using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//https://gist.github.com/lanepemberton/23de093d3638f4226651daa8f9c12848
public class AllowInputScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
	public ScrollRect MainScroll;
	private bool isAvaliable = false;
	private bool isHover = false;
	private bool oldInteractive = false;
	private InputField inputField;
	private GameObject textField;
	private Image inputImage;

	void Update()
	{
		if (oldInteractive != this.inputField.interactable)
		{
			if (this.inputField.interactable)
			{
				this.inputImage.color = this.inputField.colors.normalColor;
			}
			else
			{
				this.inputImage.color = this.inputField.colors.disabledColor;
			}

			oldInteractive = this.inputField.interactable;
		}

		if (this.inputField.interactable)
		{
			if (Input.GetMouseButton(0))
			{
				if (!this.isHover)
				{
					this.inputField.enabled = false;
				}
			}
		}
	}

	void Awake()
	{
		//set class reference to input field, text object, and image
		this.inputField = this.gameObject.GetComponent<InputField>();
		this.inputImage = this.gameObject.GetComponent<Image>();
		this.textField = this.inputField.textComponent.gameObject;
		//check if input is not read only
		if (this.inputField.interactable)
		{
			this.inputField.enabled = false;
			this.inputImage.color = this.inputField.colors.normalColor;
		}
		else
		{
			this.inputImage.color = this.inputField.colors.disabledColor;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		MainScroll.OnBeginDrag(eventData);

		if (this.inputField.interactable)
		{
			this.inputImage.color = this.inputField.colors.normalColor;

			this.inputField.enabled = false;
			this.isAvaliable = false;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		MainScroll.OnDrag(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		MainScroll.OnEndDrag(eventData);

		if (this.inputField.interactable)
		{
			this.inputField.enabled = false;
			this.isAvaliable = true;
		}
	}

	public void OnScroll(PointerEventData data)
	{
		MainScroll.OnScroll(data);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.inputField.interactable)
		{
			if (isAvaliable)
			{
				this.inputField.enabled = true;
				this.inputField.Select();
			}
			this.inputImage.color = this.inputField.colors.normalColor;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.inputField.interactable)
		{
			if (eventData.pointerEnter == this.textField)
			{
				this.inputImage.color = this.inputField.colors.pressedColor;
				this.isAvaliable = true;
			}
		}

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.inputField.interactable)
		{
			this.inputImage.color = this.inputField.colors.highlightedColor;
		}
		this.isHover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.inputField.interactable)
		{
			this.inputImage.color = this.inputField.colors.normalColor;
		}
		this.isHover = false;
	}
}