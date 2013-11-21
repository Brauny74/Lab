using UnityEngine;
using System.Collections;

public class obsctacleAStar : GameModelAStar {
	
	public bool isPointIn(Vector3 point){
		bool res;
		if((this.getCoord().x>=point.x)&&(this.getCoord().x<=point.x)){
			if((this.getCoord().y>=point.y)&&(this.getCoord().y<=point.y)){
				res = true;
			}else{res = false;}
		}else{res = false;}
		return res;
	}
	
	void Start() {
		Vector3 vect = new Vector3(levelSettings.objectSize,levelSettings.objectSize,levelSettings.objectSize/2);
		this.transform.localScale = vect;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
