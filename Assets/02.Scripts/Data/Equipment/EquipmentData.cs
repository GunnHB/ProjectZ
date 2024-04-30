namespace ProjectZ.Data
{
    public class EquipmentData
    {
        public Model.ModelItem.Data _equipItemData;
        public bool _isInHand = false;

        public EquipmentData()
        {

        }

        public EquipmentData(Model.ModelItem.Data itemData)
        {
            _equipItemData = itemData;
            _isInHand = false;
        }

        public void ClearData()
        {
            _equipItemData = null;
            _isInHand = false;
        }
    }
}
