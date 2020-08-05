using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ShopItemSetHeightProperly : MonoBehaviour {
	public float aspectRatio;
	public LayoutElement layoutElement;

	public RectTransform parentTransform;

	public bool isDirty = true;

	Vector2 lastParentSize = Vector2.zero;

	public void Start() {
		layoutElement = GetComponent<LayoutElement>();
		parentTransform = transform.parent.GetComponent<RectTransform>();
	}

	void Update() {
		Vector2 parentSize = parentTransform.rect.size;
		if (lastParentSize != parentSize) {
			lastParentSize = parentSize;
			isDirty = true;
		}
		if (!isDirty) {
			return;
		}
		layoutElement.flexibleHeight = parentTransform.rect.width * aspectRatio;
		layoutElement.preferredHeight = layoutElement.flexibleHeight;
		layoutElement.minHeight = layoutElement.flexibleHeight;
		isDirty = false;
	}

	public void OnValidate() {
		isDirty = true;
	}
}
