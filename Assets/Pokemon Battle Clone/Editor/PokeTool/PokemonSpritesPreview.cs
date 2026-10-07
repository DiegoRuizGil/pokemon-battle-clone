using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class PokemonSpritesPreview : VisualElement
    {
        private readonly SpritePreview _frontPreview;
        private readonly SpritePreview _backPreview;
        private readonly SpritePreview _iconPreview;

        public PokemonSpritesPreview()
        {
            _frontPreview = new SpritePreview("front");
            _backPreview = new SpritePreview("back");
            _iconPreview = new SpritePreview("icon");
            
            this.AddToClassList("sprites-preview");
            this.Add(_frontPreview);
            this.Add(_backPreview);
            this.Add(_iconPreview);
        }

        public void SetSprites(Sprite front, Sprite back, Sprite icon)
        {
            _frontPreview.SetSprite(front);
            _backPreview.SetSprite(back);
            _iconPreview.SetSprite(icon);
        }
    }

    public class SpritePreview : VisualElement
    {
        private readonly Image _preview;
        private readonly Label _noSpriteLabel;

        public SpritePreview(string type)
        {
            _preview = new Image();
            _noSpriteLabel = new Label { text = "No Sprite" };
            var typeLabel = new Label { text = type };
            
            this.AddToClassList("sprite-preview-container");
            typeLabel.AddToClassList("sprite-preview-label");
            
            this.Add(_preview);
            this.Add(_noSpriteLabel);
            this.Add(typeLabel);
        }
        
        public void SetSprite(Sprite sprite)
        {
            _preview.sprite = sprite;
            
            _preview.style.display = sprite == null ? DisplayStyle.None : DisplayStyle.Flex;
            _noSpriteLabel.style.display = sprite == null ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}