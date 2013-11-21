using UnityEngine;
using System.Collections;

public class victimBehAStar : GameModelAStar {
	
	public GameModelAStar hunter;
	
	public void dodgeMissile(){
		bool fl = true;
		Vector3 randVect = new Vector3(0,0,0);
		object[] obj = GameObject.FindObjectsOfType(typeof (missileBeh));
  		foreach (object o in obj){
	    	missileBeh g = (missileBeh) o;
	       	if(levelSettings.radarRange >= Vector3.Distance(this.getCoord(),g.getCoord())){
				fl = true;
				while(fl){
					float x = 0;
					float y = 0;
					while((x<5)&&(y<5)){
						x =  Random.Range(-30,30);
						y =  Random.Range(-30,30);
					}
					randVect = new Vector3(x,y,0);
					if(Vector3.Distance(randVect,g.getCoord())>Vector3.Distance(this.getCoord(),g.getCoord())){
						fl = false;
					}
				}
				this.transform.Translate(randVect);
			}
		}
	}
	
 	Vector3 runFromHunter(Vector3 target){
		bool fl = true;
		float dX = 0; 
		float dY = 0;
		float rX = this.getCoord().x;
		float rY = this.getCoord().y;
		float border = levelSettings.levelSize / 2;
		Vector3 res = new Vector3(rX,rY,0);
		while(fl){
			if(this.getCoord().x < target.x){
				dX = Random.Range(-1000,5);
			}
			if(this.getCoord().x > target.x){
				dX = Random.Range(-5,1000);
			}
			if(this.getCoord().y < target.y){
				dY = Random.Range(-1000,5);
			}
			if(this.getCoord().y > target.y){
				dY = Random.Range(-5,1000);
			}
			rX = this.getCoord().x + dX;
			rY = this.getCoord().x+ dY;
			res = new Vector3(rX, rY, 0);
			if(((rX<border)&&(rX>0-border))&&((rY<border)&&(rY>0-border))){
				if(Vector3.Distance(res,target)>Vector3.Distance(this.getCoord(),target)){
					fl = false;
				}
			}else{
				rX = rX / 2;
				rY = rY / 2;
				res = new Vector3(rX, rY, 0);
				fl = false;
			}
		}
		return res;
	}
	
	void Start (){
		transform.Translate(Random.Range(-1000,1000),Random.Range(-1000,1000),0);
		Vector3 vect = new Vector3(levelSettings.objectSize,levelSettings.objectSize,levelSettings.objectSize/2);
		transform.localScale = vect;
		target = runFromHunter(hunter.getCoord());
		path = findPath(target);
	}
	
	// Update is called once per frame
	void Update() {
		if(levelSettings.timer == levelSettings.maxTimer){
			target = runFromHunter(hunter.getCoord());
			path = findPath(target);
		}
		dodgeMissile();
		object[] obj = GameObject.FindObjectsOfType(typeof (missileBeh));
  		foreach (object o in obj){
	    	missileBeh g = (missileBeh) o;
	       	if(this.isCollide(g.getCoord())){
				this.transform.localScale -= transform.localScale;
				Debug.Log("Victim was killed, hunter wins!");
			}
		}
		object[] obst = GameObject.FindObjectsOfType(typeof (obsctacleAStar));
  		foreach (object o in obst){
	    	obsctacleAStar g = (obsctacleAStar) o;
	       	if(this.isCollide(g.getCoord())){
				this.bounce(g);
			}
		}
		if(isCollide(hunter.getCoord())){
			this.transform.localScale -= transform.localScale;
			Debug.Log("Victim was killed, hunter wins!");
		}
		x = transform.position.x;
		y = transform.position.y;
		moveTo(path);
		transform.Translate(Input.GetAxis("Horizontal")*15, Input.GetAxis("Vertical")*15, 0);
	}
}
