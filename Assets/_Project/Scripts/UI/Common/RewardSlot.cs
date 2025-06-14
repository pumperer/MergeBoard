using alpoLib.Res;
using alpoLib.UI;
using MergeBoard.Data;
using MergeBoard.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI
{
    public class RewardSlotData : SlotDataBase
    {
        public Reward Reward;
    }
    
    [PrefabPath("Addr/UI/Prefabs/Common/RewardSlot.prefab")]
    public class RewardSlot : UISlotBase<RewardSlotData, RewardSlot>
    {
        [SerializeField] protected Image rewardImage;
        [SerializeField] protected TMP_Text rewardCount;
        [SerializeField] protected TMP_Text rewardName;

        private int _requestKey;
        
        protected override void OnSetData(RewardSlotData data)
        {
            UpdateUI(data.Reward);
        }

        private void UpdateUI(Reward reward)
        {
            var key = reward.GetSpriteAtlasKey();
            OnDemandAtlasManager.Instance.GetSprite(new SpriteAtlasRequest
            {
                AtlasKey = key.atlasKey,
                SpriteName = key.spriteKey,
                RequestKey = ++_requestKey,
            }, result =>
            {
                if (result.RequestKey != _requestKey)
                    return;
                if (result.Sprite && rewardImage)
                    rewardImage.sprite = result.Sprite;
            });
            
            if (rewardCount)
                rewardCount.text = $"{reward.Value}";
            if (rewardName)
                rewardName.text = reward.GetName();
        }
    }
}