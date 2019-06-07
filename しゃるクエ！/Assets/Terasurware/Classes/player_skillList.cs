using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player_skillList : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public double ID;
		public string charName;
		public string skillName;
		public string effectName;
		public double Lv;
		public double power;
		public string target;
		public string useChara;
		public double hpCtl;
		public double attackType;
		public double sp;
		public double period;
		public string influence1;
		public string influence2;
		public string Time;
		public string effect;
		public string etc;
	}
}

