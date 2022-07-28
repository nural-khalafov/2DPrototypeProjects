using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace StatsBoosterPrototype.InventorySystem
{
    [CreateAssetMenu(menuName = "Create Item/New Item", fileName = "NewItem")]
    public class ItemSO : ScriptableObject
    {
        public int ItemId;
        public string ItemName;
        [PreviewField] public Sprite ItemImage;
        public ItemType ItemType;
        [TextArea(2,6)]
        public string ItemDescription;


        public Sprite GetSprite() 
        {
            return ItemImage;
        }
    }
}