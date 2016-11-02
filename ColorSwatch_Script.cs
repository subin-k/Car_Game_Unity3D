using UnityEngine;
using System.Collections;

public class ColorSwatch_Script : MonoBehaviour {
	
	public bool trackUIState;
	public GameObject trackUI;

	public bool swatch_isOpen;
	public GameObject colorSwatch;
	public Color colorSel;

	public Material[] vehPaint;
	public int carSelected;

	void Start () {
	
		swatch_isOpen = false;
		trackUIState = false;
		trackUI.SetActive (false);
	}

	public void setCarNo (int a) {

		carSelected = a;
	}

	public void swatch_changeState () {

		if (!swatch_isOpen) {

			colorSwatch.GetComponent<Animator> ().Play ("ColorSwatch_Open");
			swatch_isOpen = true;
		} else if (swatch_isOpen) {
			colorSwatch.GetComponent<Animator>().Play("ColorSwatch_Close");
			swatch_isOpen = false;
		}
	}

	public void trackUI_changeState () {
		
		if (!trackUIState) {

			trackUI.SetActive(true);
			trackUIState = true;

		} else if (trackUIState) {

			trackUI.SetActive(false);
			trackUIState = false;
		}
	}

	public void selectColor (string hex) {
		
		hex = hex.Replace ("0x", "");
		hex = hex.Replace ("#", "");
		byte a = 255;
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		}
		colorSel = new Color32 (r, g, b, a);
		vehPaint [carSelected].color = colorSel;
	}
}
