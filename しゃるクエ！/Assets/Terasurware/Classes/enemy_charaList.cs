using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy_charaList : ScriptableObject
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
		public string Name;
		public double HP;
		public double SP;
		public double ATK;
		public double DEF;
		public double SPD;
		public double MAT;
		public double MDF;
		public double LUK;
		public string AResistance;
		public string MResistance;
		public string weakAttribute;
		public int Skill1;
		public int Skill2;
		public int Skill3;
		public int Skill4;
	}
}

