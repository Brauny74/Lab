using UnityEngine;
using System.Collections;

public class GameModelAStar : MonoBehaviour {
	
	protected Vector3 target;
	protected point[] path;
	protected float x;
	protected float y;	
	
	public void bounce(obsctacleAStar Ob){
		bool fl = true;
		Vector3 randVect = new Vector3(0,0,0);
		fl = true;
		while(fl){
			float x = 0;
			float y = 0;
			while((x<5)&&(y<5)){
				x =  Random.Range(-20,20);
				y =  Random.Range(-20,20);
			}
			randVect = new Vector3(x,y,0);
			if(!(Ob.isPointIn(randVect))){
				fl = false;
			}
		}
		this.transform.Translate(randVect);
	}
	
	public bool isCollide(Vector3 coord){
		bool collision;
		if ((levelSettings.objectSize * 1.41/2) >= Vector3.Distance(this.getCoord(),coord)){
 			collision = true;
		}else{
			collision = false;
		}
		return collision;
	}
	
	public point[] findPath(Vector3 tC){
		/*float dist = Vector3.Distance(this.getCoord(),target);
		int disp = System.Convert.ToInt32(dist) / levelSettings.lengthOt;
		if(disp == 0) disp++;
		point[] result = new point[disp];
		result[0] = new point(this.getCoord().x,this.getCoord().y);
		float tmpX = this.getCoord().x;
		float difX = tmpX - tC.x;
		float tmpY = this.getCoord().y;
		float difY = tmpY - tC.y;
		for(int i = 1; i<disp; i++){
			result[i] = new point(tmpX+(difX/disp),tmpY+(difY/disp));
		}
		result[disp-1] = new point(tC.x,tC.y);
		return result;*/
		cell conv = new cell();
		ArrayList t = levelSettings.aStar.algAStar(conv.toGrid(this.getCoord()),conv.toGrid(tC));
		point[] result = new point[t.Count];
		for(int i = 0; i<t.Count; i++){
			conv = (cell)t[i];
			result[i] = conv.toPoint(conv);
		}
		return result;
	}
	
	public Vector3 getCoord(){
		return transform.position;
	}
	
	protected void stepTo(Vector3 goal){
		if(goal.x < x){
			transform.Translate(levelSettings.speed,0,0);
		}else{
			transform.Translate(0-levelSettings.speed,0,0);
		}
		if(goal.y < y){
			transform.Translate(0,levelSettings.speed,0);
		}else{
			transform.Translate(0,0-levelSettings.speed,0);
		}
	}
	
		
	public void moveTo(point[] goal){
		bool fl = false;
		foreach(point tC in goal){
			if(tC.isPassed()){
				continue;
			}else{
				if(fl){
					tC.makeCurrent();
					this.stepTo(tC.getCoord());
					fl = false;
				}else{
					if(tC.isCurrent()){
						this.stepTo(tC.getCoord());
					}
					if(Vector3.Distance(tC.getCoord(),this.getCoord())<0.01){
						tC.pass();
						tC.unmakeCurrent();
						fl = true;
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
}
