using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class stageData : ScriptableObject
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
		
		public int stage;
		public int enemy_count;
		public string extra;
		public int enemy1;
		public int enemy2;
		public int enemy3;
		public int enemy4;
		public int enemy5;
		public int enemy6;
		public int enemy7;
		public int enemy8;
		public int BGM;
		public double EXP;
	}
}

