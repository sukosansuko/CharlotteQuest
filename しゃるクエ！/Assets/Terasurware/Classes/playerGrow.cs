using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerGrow : ScriptableObject
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
		public double GROWHP;
		public double GROWSP;
		public double GROWATK;
		public double GROWDEF;
		public double GROWSPD;
		public double GROWMAT;
		public double GROWMDF;
		public double GROWLUK;
	}
}

