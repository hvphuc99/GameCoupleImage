using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	[SerializeField] Sprite BackGroudImg;
	private List<Button> btnList = new List<Button>();
	public List<Sprite> GameSprite = new List<Sprite> ();
	[SerializeField]
	public Sprite[] SourceSprite;
	private bool firstGuess, secondGuess;
	string firstName, secondName;
	int firstIndex, secondIndex;
	int correct;
	public Text t_time;
	public GameObject SoundCorrect;
	public GameObject SoundIncorrect;
	AudioSource MyAudioCorrect;
	AudioSource MyAudioIncorrect;
	private float startTime;
	public Button readyButton;
	public Text t_ready;
	bool ready = false;

	void Awake() {
		//lenh load tat cac cac hinh
		SourceSprite = Resources.LoadAll<Sprite> ("Sprites/GameImg");
		MyAudioCorrect = SoundCorrect.GetComponent<AudioSource> ();
		MyAudioIncorrect = SoundIncorrect.GetComponent<AudioSource> ();
	}

	//Ham them sprite vao mang SourceSprite
	void AddSprite() {
		int size = btnList.Count;
		int index = 0;
		for (int i = 0; i < size; i++) {
			if (i == size / 2) {
				index = 0;
			}
			GameSprite.Add (SourceSprite[index]);
			index++;
		}
	}

	public void ClickReady() {
		readyButton.interactable = false;
		readyButton.image.color = new Color (0, 0, 0, 0);
		t_ready.text = "";
		ready = true;
		startTime = Time.time-31;
		AddListener ();
	}

	void Start () {
		GetButtons ();
		AddSprite ();
		Shuffle (GameSprite);
		correct = 0;
		if (ready == false) {
			t_time.text = "01:30";
		}
	}

	void Update() {
		if (ready == true) {
			float t = Time.time - startTime;
			int i_minute = (1 - ((int)t / 60));
			int i_second = (60 - ((int)t % 60));
			string minute = i_minute.ToString ();
			string second = i_second.ToString ("f0");
			if (i_second < 10) {
				t_time.text = "0" + minute + ":0" + second;
			} else if (i_second == 60)
				t_time.text = "01:00";
			else
				t_time.text = "0" + minute + ":" + second;
			if (i_second == 60 && i_minute == -1)
				Application.LoadLevel ("LoseGame");
		}
	}

	//Them hinh vao tat ca cac button
	void GetButtons () {
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("PuzzleButton");
		for (int i = 0; i < objects.Length; i++) {
			btnList.Add (objects[i].GetComponent<Button>());
			btnList [i].image.sprite = BackGroudImg;
		}
	}
	//Ham thuc hien chuc nang PickPuzzle khi button duoc click 
	void AddListener() {
		foreach (Button btn in btnList) {
			btn.onClick.AddListener (() => PickPuzzle ());
		}
	}
	//Ham xu li chuc nang cho button khi duoc click
	void PickPuzzle() {
		if (!firstGuess) {
			firstGuess = true;
			firstIndex = int.Parse (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
			firstName = GameSprite [firstIndex].name;
			btnList [firstIndex].image.sprite = GameSprite [firstIndex];
		} else if (!secondGuess) {
			secondGuess = true;
			secondIndex = int.Parse (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
			secondName = GameSprite [secondIndex].name;
			btnList [secondIndex].image.sprite = GameSprite [secondIndex];
			StartCoroutine(CheckPuzzleMatched ());
			CheckIfFinish ();
		}

	}

	IEnumerator CheckPuzzleMatched() {
		yield return new WaitForSeconds (0.25f);
		if (firstName == secondName && firstIndex != secondIndex) {
			MyAudioCorrect.Play ();
			//Khong cho click vao button nay nua
			btnList [firstIndex].interactable = false;
			btnList [secondIndex].interactable = false;

			//Cho mau hinh cua button trung voi mau background
			btnList [firstIndex].image.color = new Color (0, 0, 0, 0);
			btnList [secondIndex].image.color = new Color (0, 0, 0, 0);
			correct++;
		} else {
			MyAudioIncorrect.Play ();
			btnList [firstIndex].image.sprite = BackGroudImg;
			btnList [secondIndex].image.sprite = BackGroudImg;
		}
		firstGuess = secondGuess = false;
	}
	void Shuffle(List<Sprite> l) {
		Sprite temp;
		int a;
		for (int i = 0; i < l.Count; i++) {
			a = Random.Range (1, l.Count);
			temp = l [i];
			l [i] = l [a];
			l [a] = temp; 
		}
	}
	void CheckIfFinish() {
		if (correct >= btnList.Count/2 - 1)
			Application.LoadLevel ("EndGame");
	}
}
