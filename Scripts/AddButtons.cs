using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour {
	[SerializeField] private Transform panel;
	[SerializeField] private GameObject Button;
	GameObject btn;
	//Ham tao cac button
	void Awake() {
		for (int i = 0; i < 20; i++) {
			btn = Instantiate (Button);
			btn.name = "" + i;
			btn.transform.SetParent (panel, false);
		}
	}
}
