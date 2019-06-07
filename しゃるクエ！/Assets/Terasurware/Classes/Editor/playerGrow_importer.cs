using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class playerGrow_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/ExcelData/playerGrow.xls";
	private static readonly string exportPath = "Assets/Resources/ExcelData/playerGrow.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			playerGrow data = (playerGrow)AssetDatabase.LoadAssetAtPath (exportPath, typeof(playerGrow));
			if (data == null) {
				data = ScriptableObject.CreateInstance<playerGrow> ();
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

					playerGrow.Sheet s = new playerGrow.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						playerGrow.Param p = new playerGrow.Param ();
						
					cell = row.GetCell(0); p.ID = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.GROWHP = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.GROWSP = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.GROWATK = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.GROWDEF = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.GROWSPD = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.GROWMAT = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.GROWMDF = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.GROWLUK = (cell == null ? 0.0 : cell.NumericCellValue);
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
