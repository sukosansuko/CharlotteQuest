using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class player_chara_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/ExcelData/player_chara.xls";
	private static readonly string exportPath = "Assets/Resources/ExcelData/player_chara.asset";
	private static readonly string[] sheetNames = { "player_statusList", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			player_charaList data = (player_charaList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(player_charaList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<player_charaList> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					player_charaList.Sheet s = new player_charaList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						player_charaList.Param p = new player_charaList.Param ();
						
					cell = row.GetCell(0); p.ID = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.HP = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.SP = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.ATK = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.DEF = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.SPD = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.MAT = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.MDF = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.LUK = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.AttackSkill1 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.AttackSkill2 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.AttackSkill3 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.AttackSkill4 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.AttackSkill5 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.AttackSkill6 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.AttackSkill7 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.AttackSkill8 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.SupportSkill1 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.SupportSkill2 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.SupportSkill3 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.SupportSkill4 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.SupportSkill5 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.SupportSkill6 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.SupportSkill7 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.SupportSkill8 = (cell == null ? 0.0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
