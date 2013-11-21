using UnityEngine;
using System;
using System.Collections;

	public class cell
	{
		public int x;//êîîðäèíàòû òî÷êè
		public int y;
		public int f;
		public int g;//îöåíêà ïðîéäåííîãî ðàññòîÿíèÿ îò ñòàðòà äî ýòîé òî÷êè
		public int h;//ïðèìåðíàÿ îöåíêà ðàññòîÿíèÿ îò ýòîé òî÷êè äî ôèíèøà
		public bool passable;//èñòèííà, åñëè ÷åðåç òî÷êó ìîæíî ïðîéòè, ëîæü, åñëè â äàííîé òî÷êå íàõîäèòñÿ ïðåïÿòñòâèå
		public bool passed;
		public cell parent;//ðîäèòåëüñêàÿ òî÷êà - ýòî òî÷êà èç êîòîðîé ìû ïðèøëè
	
		public cell(int nx,int ny){
			this.x = nx;
			this.y = ny;
			parent = null;
			passed = false;
		}
		
		public cell(Vector3 c){
			cell conc = new cell();
			conc = conc.toGrid(c);
			this.x = conc.x;
			this.y = conc.y;
			parent = null;
			passed = false;
		}
	
		public cell()
		{
			parent = null;
			passed = false;
		}
		void add_parent(cell p)
		{
			parent = p;
		}
		
		//Сравнение работает другим образом - сравниваются только координаты, без родителей, f, g и h
		public override bool Equals(System.Object obj)
    	{
			if (obj == null)
	        {
	            return false;
	        }
		
			cell p = obj as cell;
	        if ((System.Object)p == null)
	        {
	            return false;
	        }
		
			bool result = false;
			if((this.x == p.x)&&(this.y == p.y)){
				result = true;
			}
			return result;
		}
	
		public bool Equals(cell p)
    	{
	        // If parameter is null return false:
	        if ((System.Object)p == null)
	        {
	            return false;
	        }
	
	        // Return true if the fields match:
	        return (this.x == p.x) && (this.y == p.y);
   		}
		
		public override int GetHashCode()
	    {
	        return x ^ y;
	    }
	
		public float estimate_cost(cell p)//ïðèìåðíîå ðàññòîÿíèå äî öåëåâîé òî÷êè âû÷èñëÿåòñÿ êàê ìàíõýòýíñêîå ðàññòîÿíèå
		{
			int result;
			result = Math.Abs(this.x-p.x)-Math.Abs(this.y-p.y);
			return result;
		}
	
		public cell toGrid(Vector3 point){
			int x = Convert.ToInt32(point.x / levelSettings.objectSize);
			int y = Convert.ToInt32(point.y / levelSettings.objectSize);
			cell res = new cell(x,y);
			return res;
		}
		
		public cell toGrid(float ax, float ay){
			int x = Convert.ToInt32(ax / levelSettings.objectSize);
			int y = Convert.ToInt32(ay / levelSettings.objectSize);
			cell res = new cell(x,y);
			return res;
		}
	
		public point toPoint(cell a){
			point res;
			float x = a.x * levelSettings.objectSize + levelSettings.objectSize/2;
			float y = a.y * levelSettings.objectSize + levelSettings.objectSize/2;
			res = new point(new Vector3(x,y,0));
			return res;
		}
	
		public bool isPassable(){
			bool res = true;
			Vector3 t = new Vector3(0,0,0);
			object[] obst = GameObject.FindObjectsOfType(typeof (obsctacleAStar));
	  		foreach (object o in obst){
		    	obsctacleAStar g = (obsctacleAStar) o;
		       	for(int i=this.x*levelSettings.objectSize;i<this.x*levelSettings.objectSize+levelSettings.objectSize;i++){
					for(int j=this.y*levelSettings.objectSize;j<this.y*levelSettings.objectSize+levelSettings.objectSize;j++){
						t = new Vector3(i,j,0);
						if(g.isPointIn(t)){res = false;}
					}
				}
			}
			return res;
		}
	}
		
	public class FindPathClass
	{
		public ArrayList allset = new ArrayList();//ýòî íàáîð âñåõ òî÷åê, êîòîðûå îáñ÷èòûâàåò àëãîðèòì
		private ArrayList openset = new ArrayList();//ýòî íàáîð âñåõ åùž íå ïðîéäåííûõ, íî óæå îöåíåííûõ òî÷åê
		private ArrayList closedset = new ArrayList();//ýòî íàáîð ïðîéäåííûõ òî÷åê
		public ArrayList path = new ArrayList();//ýòîò íàáîð õðàíèò òî÷êè ïóòè â ïîðÿäêå åãî ïðîõîæäåíèÿ
		
		public ArrayList createAllset(){
			cell conv = new cell();
			int k = 0;
			ArrayList resset = new ArrayList();
			int cell_num = levelSettings.levelSize/levelSettings.objectSize;
			for(int x=0-cell_num;x<cell_num;x++){
				for(int y=0-cell_num;y<cell_num;x++){
					//Vector3 vect = new Vector3(x*levelSettings.objectSize,y*levelSettings.objectSize,0);
					conv = new cell(x,y);
					//if(k==1 || k==4){Debug.Log(conv.x);}
					k = resset.Add(conv);
				}
			}
			Debug.Log(k - allset.Count);
			/*for(int i = 0;i<1000;i++){
				Debug.Log("allset["+Convert.ToString(i)+"]");
				cell c = (cell)resset[i];
				Debug.Log(c.x);
			}*/
			//Debug.Log(resset.Count);
			return resset;
		}
	
		public ArrayList buildPath(cell start, cell goal)
		{
			/*int k = 0;
			foreach(cell d in allset){
				k++;
				if(k>40000){break;}
				Debug.Log(Convert.ToString(d.x)+' '+Convert.ToString(d.y));
			}*/
			return this.algAStar(/*this.allset, */start, goal);
		}
	
		public ArrayList retrievePath()
		{
			return path;
		}
		
		public ArrayList reconstructPath(cell start, cell goal)
		{
			ArrayList result = new ArrayList();
			bool fl = false;
			int k = 0;
			cell current = (cell)allset[allset.Count-1];
			while(!(fl) && k<allset.Count)
			{
				/*result.Add(current);
				if(current.parent == null){
					fl = true;
				}else{
					current = current.parent;
				}*/
				current = (cell)allset[k];
				result.Add(allset[k]);
				k++;
			}
			result.Reverse();
			return result;
		}
		
		cell lowestF(ArrayList goal_set)//ýòîò ìåòîä íàõîäèò òî÷êó ñ íàèìåíüøèì F â ïåðåäàííîì åìó ìàññèâå òî÷åê
		{
			cell resultP = new cell();
			int min_f = int.MaxValue;
			foreach(cell x in goal_set)
			{
				if(min_f>x.f)
				{
					min_f = x.f;
					resultP = x;
				}
			}
			return resultP;
		}
		
		bool isInSet(cell x, ArrayList rset){
			foreach(cell b in rset){
				if(b.x == x.x && b.y == x.y){
					return true;
				}
			}
			return false;
		}
	
		int indexOfCell(cell x, ArrayList rset){
			int i = 0;
			foreach(cell b in rset){
				if(b.x == x.x && b.y == x.y){
					return i;
				}
				i++;
			}
			return -1;
		}
	
		ArrayList allcellsNearX(ArrayList openset, ArrayList closedset, cell a)//ýòîò ìåòîä íàõîäèò âñå òî÷êè, ðàñïîëîæåííûå ðÿäîì ñ öåëåâîé òî÷êîé
		{
			ArrayList result = new ArrayList();
			cell b = new cell(0,0);
			/*foreach(cell b in allset)
			{
				if((b.x == a.x - 1)&&(b.y == a.y + 1)){result.Add(b);}
				if((b.x == a.x)&&(b.y == a.y + 1)){result.Add(b);}
				if((b.x == a.x + 1)&&(b.y == a.y + 1)){result.Add(b);}
				if((b.x == a.x - 1)&&(b.y == a.y)){result.Add(b);}
				if((b.x == a.x + 1)&&(b.y == a.y)){result.Add(b);}
				if((b.x == a.x - 1)&&(b.y == a.y - 1)){result.Add(b);}
				if((b.x == a.x)&&(b.y == a.y - 1)){result.Add(b);}
				if((b.x == a.x - 1)&&(b.y == a.y - 1)){result.Add(b);}
			}*/
			point p = a.toPoint(a);
			Vector3 vect = p.getCoord();
			int ob = levelSettings.objectSize;
			cell t;
			t = b.toGrid(vect.x+ob,vect.y);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			t = b.toGrid(vect.x+ob,vect.y+ob);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			t = b.toGrid(vect.x,vect.y+ob);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			t = b.toGrid(vect.x-ob,vect.y);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			t = b.toGrid(vect.x-ob,vect.y-ob);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			t = b.toGrid(vect.x,vect.y-ob);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			t = b.toGrid(vect.x+ob,vect.y-ob);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			t = b.toGrid(vect.x-ob,vect.y+ob);
			if(!(isInSet(t,openset)) && !(isInSet(t,closedset))){
				result.Add(t);
			}else{
				if(isInSet(t,openset)){result.Add(openset[indexOfCell(t,openset)]);}
			}
			return result;
		}
		
		public ArrayList algAStar(/*ArrayList allset, */cell start, cell goal)//ýòî ðåàëèçàöèÿ àëãîðèòìà À*
		{
			cell p = start;
			int tentative_g_score;//îöåíêà ñòîèìîñòè ïóòè ÷åðåç òî÷êó
			bool tentative_is_better;//èñòèííî, åñëè ïóòü ÷åðåç òî÷êó ëó÷øå óæå ïðîéäåííîãî ïóòè
			ArrayList nearcells  = new ArrayList();
			//closedset = null;
			openset.Add(start);
            start.g = 0;
			start.h = Convert.ToInt32(start.estimate_cost(goal));
			nearcells = allcellsNearX(openset,closedset,start);
			p = lowestF(nearcells);
			start.f = start.g + start.h;
			int k = 0;
			while(openset.Count>0)
			{
				k++;
				//Debug.Log ("P:"+Convert.ToString(p.x)+' '+Convert.ToString(p.y));
				nearcells = allcellsNearX(openset,closedset,p);
				p = lowestF(nearcells);
				if(k==100 || p == goal){
					return reconstructPath(start,goal);
				}
				/*foreach(cell c in nearcells){
					Debug.Log (Convert.ToString(c.x)+' '+Convert.ToString(c.y));
				}*/
				openset.Remove(p);
				closedset.Add(p);
				foreach(cell y in nearcells)//ïðîâåðÿåì âñå òî÷êè, ïðèìûêàþùèå ê õ
				{
					if((closedset.IndexOf(y)!=-1)||(y.isPassable()==false))
					{
						continue;//åñëè òî÷êà y óæå ïðîéäåíà èëè íåïðîõîäèìà, ïðîïóñêàåì åž
					}
					tentative_g_score = p.g + 1;//îöåíèâàåì ðàññòîÿíèå äëÿ ïðîõîæäåíèÿ ÷åðåç òî÷êó
					if(openset.IndexOf(y)==-1)
					{
						openset.Add(y);
						tentative_is_better = true;
					}
					else
					{
						if(tentative_g_score < y.g)
						{tentative_is_better = true;}
						else{tentative_is_better = false;}
					}
					if(tentative_is_better)
					{
						y.parent = p;
						y.g = tentative_g_score;
						y.h = Convert.ToInt32(y.estimate_cost(goal));
						y.f = y.h + y.g;
						allset.Add(y);
					}
				}
			}
			return null;
		}
		
	}


