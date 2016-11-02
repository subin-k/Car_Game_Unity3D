using UnityEngine;
using System.Collections;

public class DataHandler : MonoBehaviour {

	public int carSel;
	public int trackSel;

	void Awake () {

		GameObject.DontDestroyOnLoad (this.gameObject);
	}

	void Start () {
	
		carSel = 1;
		trackSel = 1;
	}

	public void selectCar (int a) {
	
		carSel = a;
	}

	public void selectTrack (int b) {

		trackSel = b;
	}
}
