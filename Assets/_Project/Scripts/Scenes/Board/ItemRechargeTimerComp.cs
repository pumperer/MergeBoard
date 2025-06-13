using UnityEngine;

namespace MergeBoard.Scenes.Board
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemRechargeTimerComp : MonoBehaviour
    {
        public string PropertyName = string.Empty;
        [Range(0f, 1f)]
        public float Amount = 1f;

        private SpriteRenderer _spriteRenderer = null;
        private int _degree = 0;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            SetSpriteData();
        }

        public void SetAmount(float amount)
        {
            Amount = amount;
        }

        private void Update()
        {
            _degree = (int)(Amount * 360);
            _spriteRenderer.sharedMaterial.SetInt("_Arc1", _degree);
        }

        private void SetSpriteData()
        {
            var sprite = _spriteRenderer.sprite.textureRect;

            Vector4 spriteData = new(
                sprite.x / _spriteRenderer.sprite.texture.width,
                sprite.y / _spriteRenderer.sprite.texture.height,
                sprite.width / _spriteRenderer.sprite.texture.width,
                sprite.height / _spriteRenderer.sprite.texture.height
            );

            _spriteRenderer.sharedMaterial.SetVector(PropertyName, spriteData);
        }
    }
}