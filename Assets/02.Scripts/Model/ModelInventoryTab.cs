using System.IO;
using System.Collections.Generic;

namespace ProjectZ.Model.ModelInventoryTab
{
    public class Data
    {
        public System.Int64 id;
		public ProjectZ.Manager.GameValue.ItemType type;
		public System.String normal_sprite;
		public System.String select_sprite;
		public System.Int32 order;
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
            var jsonData = File.ReadAllText($"{UnityEngine.Application.dataPath}//06.Tables/Json/InventoryTab.json");
#elif UNITY_STANDALONE
            var jsonData = File.ReadAllText($"{UnityEngine.Application.streamingAssetsPath}/Json/InventoryTab.json");
#endif
            Manager.JsonUtil.Deserialize(jsonData, _dataList);

            foreach (var item in _dataList)
                _dataDic.Add(item.id, item);
            
            _isInit = true;
        }
        
        /// <summary>
        /// 아이디 값으로 데이터 가져오기
        /// </summary>
        /// <param name="id">해당하는 테이블 아이디</param>
        public static Data GetData(long id)
        {
            if (_dataDic.ContainsKey(id))
                return _dataDic[id];
            else
                return null;
        }
        
        public static List<Data> DataList => _dataList;
        
    }
}