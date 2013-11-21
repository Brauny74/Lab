using UnityEngine;
using System.Collections;

public class levelSettings : MonoBehaviour {
	
	static public int maxTimer = 10;
	static public int timer = maxTimer - 1;
	static public int missileAccel = 5;
	static public int objectSize = 20;	
	static public int lengthOt = 5;
	static public int numberOfObstacle = 10;
	static public int radarRange = 25;
	static public int speed = 3;
	static public int levelSize = 1000;
	static public int obstacleCount = 50;
	static public FindPathClass aStar = new FindPathClass();
	//static public ArrayList allset = aStar.createAllset();
	
	public class point{
		bool current;
		bool passed;
		Vector3 coord;
		
		public point(Vector3 start_coord){
			current = false;
			passed = false;
			coord = start_coord;
		}
		
		public bool isPassed(){
			return passed;
		}
		public bool isCurrent(){
			return current;
		}
		public Vector3 getCoord(){
			return coord;
		}
		public void pass(){
			passed = true;
		}
		public void makeCurrent(){
			current = true;
		}
		public void unmakeCurrent(){
			current = false;
		}
	}
	
	void Start(){
		//allset = aStar.createAllset();
		obsctacleAStar tob = new obsctacleAStar();
		for(int i = 0; i < numberOfObstacle; i++){
			Instantiate(tob, transform.position, transform.rotation);
			tob.transform.Translate(Random.Range(0-levelSize,levelSize),Random.Range(0-levelSize,levelSize),0);
			Vector3 vect = new Vector3(levelSettings.objectSize,levelSettings.objectSize,levelSettings.objectSize/2);
			tob.transform.localScale = vect;
		}
	}
	
	void Update(){
		if(levelSettings.timer == levelSettings.maxTimer){
			levelSettings.timer = 0;
		}else{
			levelSettings.timer++;
		}
	}
}


public class point{
		bool current = false;
		bool passed = false;
		Vector3 coord;
		
		public point(Vector3 start_coord){
			current = false;
			passed = false;
			coord = start_coord;
		}
		
		public point(float x, float y){
			current = false;
			passed = false;
			Vector3 start_coord = new Vector3(x,y,0);
			coord = start_coord;
		}	
	
		public bool isPassed(){
			return passed;
		}
		public bool isCurrent(){
			return current;
		}
		public Vector3 getCoord(){
			return coord;
		}
		public void pass(){
			passed = true;
		}
		public void makeCurrent(){
			current = true;
		}
		public void unmakeCurrent(){
			current = false;
		}
	
		public void recieveCoord(float x, float y){
			coord.x = x;
			coord.y = y;
			coord.z = 0;
		}
	
		public point pointToVector(Vector3 vect){
			coord = vect;
			current = false;
			passed = false;
			return this;
		}
	}