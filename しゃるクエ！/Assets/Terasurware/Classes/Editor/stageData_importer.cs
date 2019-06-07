using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class stageData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/ExcelData/stageData.xls";
	private static readonly string exportPath = "Assets/Resources/ExcelData/stageData.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			stageData data = (stageData)AssetDatabase.LoadAssetAtPath (exportPath, typeof(stageData));
			if (data == null) {
				data = ScriptableObject.CreateInstance<stageData> ();
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

					stageData.Sheet s = new stageData.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						stageData.Param p = new stageData.Param ();
						
					cell = row.GetCell(0); p.stage = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.enemy_count = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.extra = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.enemy1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.enemy2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.enemy3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.enemy4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.enemy5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.enemy6 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.enemy7 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.enemy8 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.BGM = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.EXP = (cell == null ? 0.0 : cell.NumericCellValue);
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
