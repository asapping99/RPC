using UnityEngine;
using System.Collections;

public class CsGameManager : MonoBehaviour {

	public GameObject _player1;
	public GameObject _player2;
	public SpriteRenderer spr1;
	public SpriteRenderer spr2;

	// Use this for initialization
	void Start () {
		spr1 = _player1.GetComponent<SpriteRenderer>();
		spr2 = _player2.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (spr1.enabled && spr2.enabled) {
			int w = Screen.width / 2;
			int h = Screen.height / 2;

			if (GUI.Button (new Rect (w * 0.5f - 50, h * 1.6f, 100, 50), "가위")) {
				Debug.Log("Scissor");
			}
			if (GUI.Button (new Rect (w - 50, h * 1.6f, 100, 50), "바위")) {
				Debug.Log("Rock");
			}
			if (GUI.Button (new Rect (w * 1.5f - 50, h * 1.6f, 100, 50), "보")) {
				Debug.Log("Paper");
			}
		}
	}
}
