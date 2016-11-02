#pragma strict

var carSelected : int;
var trackSelected : int;
var trackCoordinate : Vector3[];

var r8 : GameObject;
var f458 : GameObject;
var sls : GameObject;
var car : Transform;



function Start () {

	carSelected = GameObject.Find("DataHandler").GetComponent(DataHandler).carSel;
	trackSelected = GameObject.Find("DataHandler").GetComponent(DataHandler).trackSel;
	 
	if (carSelected == 1) {
		r8.SetActive (true);
		f458.SetActive (false);
		sls.SetActive (false);
	}
	
	else if (carSelected == 2) {
	
		r8.SetActive (false);
		f458.SetActive (true);
		sls.SetActive (false);
	}
	
	else if (carSelected == 3) {
	
		r8.SetActive (false);
		f458.SetActive (false);
		sls.SetActive (true);
	}
	
	car.position = trackCoordinate[trackSelected-1];
}

function Update () {

}