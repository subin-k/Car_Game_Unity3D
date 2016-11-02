using UnityEngine;
using System.Collections;

public class CarSelector : MonoBehaviour {

	public GameObject r8;
	public GameObject f458;
	public GameObject sls;

	public int carSelected;

	void Start () {
	
		r8.SetActive (true);
		f458.SetActive (false);
		sls.SetActive (false);

		carSelected = 1;
	}
	
	public void loadR8 () {

		r8.SetActive (true);
		f458.SetActive (false);
		sls.SetActive (false);

		carSelected = 1;
	}

	public void load458 () {

		r8.SetActive (false);
		f458.SetActive (true);
		sls.SetActive (false);
		
		carSelected = 2;
	}

	public void loadSLS () {

		r8.SetActive (false);
		f458.SetActive (false);
		sls.SetActive (true);
		
		carSelected = 3;
	}
}
