using System.IO;
using System.Collections.Generic;

namespace ProjectZ.Model.ModelFood
{
    public class Data
    {
        public System.Int64 id;
		public ProjectZ.Manager.GameValue.StatsType target_stats;
		public System.Single increase_value;
    }

    public class Model
    {
        private static List<Data> _dataList = new();
        private static Dictionary<long, Data> _dataDic = new();

        private static bool _isInit = false;
        
        /// <summary>
        /// 초기화하기
        /// </summary>
        public static void Initialize()
        {
            if (_isInit)
                return;

            #if UNITY_EDITOR
            var jsonData = File.ReadAllText($"{UnityEngine.Application.dataPath}//06.Tables/Json/Food.json");
            #elif UNITY_STANDALONE
            var jsonData = File.ReadAllText($"{UnityEngine.Application.streamingAssetsPath}/Json/Food.json");
            #endif
            Manager.JsonUtil.Deserialize(jsonData, _dataList);

            foreach (var item in _dataList)
                _dataDic.Add(item.id, item);
            
            _isInit = true;
        }

        public static List<Data> DataList => _dataList;
        public static Dictionary<long, Data> DataDic => _dataDic;
    }
}