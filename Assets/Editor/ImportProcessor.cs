using System;
using System.IO;

using System.Collections.Generic;

using UnityEditor;

public class ImportProcessor : AssetPostprocessor
{
    private const string EXCEL_PATH = "Assets/06.Tables/Excel/";

    private static Dictionary<string, DateTime> _previousExcelWriteTime = new();

    private static string _targetAsset;
    private static string _targetAssetName;

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        ImportExcel(importedAssets);
        DeleteExcel(deletedAssets);

        AssetDatabase.Refresh();
    }

    #region About excels
    private static void ImportExcel(string[] importedAssets)
    {
        foreach (string assetPath in importedAssets)
        {
            if (assetPath.Contains(EXCEL_PATH) && assetPath.EndsWith(".xlsx"))
            {
                _targetAsset = assetPath;
                _targetAssetName = Path.GetFileName(_targetAsset).Replace(".xlsx", string.Empty);

                CheckExcels();
            }
        }
    }

    private static void DeleteExcel(string[] deletedAssets)
    {
        foreach (string assetPath in deletedAssets)
        {
            if (assetPath.Contains(EXCEL_PATH) && assetPath.EndsWith(".xlsx"))
                _previousExcelWriteTime.Remove(assetPath);
        }
    }

    private static void CheckExcels()
    {
        var lastWriteTime = File.GetLastWriteTime(_targetAsset);

        // 이전 파일의 마지막 수정 기록이 있음
        if (_previousExcelWriteTime.ContainsKey(_targetAsset))
        {
            // 기존 파일이 수정됨
            if (_previousExcelWriteTime[_targetAsset] != lastWriteTime)
                ProjectZ.Manager.JsonUtil.CreateJsonFileByExcel(_targetAsset, _targetAssetName);
        }
        else
        {
            _previousExcelWriteTime.Add(_targetAsset, File.GetLastWriteTime(_targetAsset));

            // json 파일 생성
            ProjectZ.Manager.JsonUtil.CreateJsonFileByExcel(_targetAsset, _targetAssetName);
        }
    }
    #endregion
}
