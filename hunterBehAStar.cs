using UnityEngine;
using System.Collections;

public class hunterBehAStar : GameModelAStar {
	
	public missileBeh newObject;
	public GameModelAStar victim;
	public float fireRate = 10;
	float timeToShoot = 0;
	
	public void shoot(){	
		target = victim.getCoord();
		shootSmth(target);
	}
	
	public void shootSmth(Vector3 coord){	
		(Instantiate(newObject, transform.position, transform.rotation) as missileBeh).giveTarget(coord);
	}
	
	// Use this for initialization
	void Start () {
		transform.Translate(Random.Range(-2000,2000),Random.Range(-2000,2000),0);
		Vector3 vect = new Vector3(levelSettings.objectSize,levelSettings.objectSize,levelSettings.objectSize/2);
		transform.localScale = vect;
	}
	
	// Update is called once per frame
	void Update() {
		if(timeToShoot - Time.time < 0.01){
			shoot();
			timeToShoot = Time.time + fireRate;
		}
		if(levelSettings.timer == levelSettings.maxTimer){
			target = victim.getCoord();
			path = findPath(target);
		}
		object[] obst = GameObject.FindObjectsOfType(typeof (obsctacleAStar));
  		foreach (object o in obst){
	    	obsctacleAStar g = (obsctacleAStar) o;
	       	if(this.isCollide(g.getCoord())){
				this.bounce(g);
			}
		}
		x = transform.position.x;
		y = transform.position.y;
		moveTo(path);
	}
}
