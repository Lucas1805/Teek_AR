#pragma strict

//Loading effect
var loading : boolean = false;
var loadingTexture : Texture;
var size : float = 70.0;
private var rotAngle : float = 0.0;
var rotSpeed : float = 300.0;

function Update () {
	if(loading){
		rotAngle += rotSpeed * Time.deltaTime;
	}	
}

function OnGUI() {
	if(loading){
		var pivot : Vector2 = Vector2(Screen.width/2, Screen.height/2);
		GUIUtility.RotateAroundPivot(rotAngle%360,pivot);
		GUI.DrawTexture(Rect ((Screen.width - size)/2 , (Screen.height - size)/2, size, size), loadingTexture); 
	}
}
