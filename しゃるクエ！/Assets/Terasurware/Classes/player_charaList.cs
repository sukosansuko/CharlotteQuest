using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player_charaList : ScriptableObject
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
		public double AttackSkill1;
		public double AttackSkill2;
		public double AttackSkill3;
		public double AttackSkill4;
		public double AttackSkill5;
		public double AttackSkill6;
		public double AttackSkill7;
		public double AttackSkill8;
		public double SupportSkill1;
		public double SupportSkill2;
		public double SupportSkill3;
		public double SupportSkill4;
		public double SupportSkill5;
		public double SupportSkill6;
		public double SupportSkill7;
		public double SupportSkill8;
	}
}

