using UnityEngine;
using UnityEngine.UI;

namespace Splatrika.MobArenaMobile.UI
{
    public class NumberImageView : NumberView
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Orientation _orientation;


        public override void Show(float number)
        {
            var size = _image.sprite.rect.size;
            if (_orientation == Orientation.Horizontal)
            {
                size.x *= number;
            }
            if (_orientation == Orientation.Vertical)
            {
                size.y *= number;
            }
            _image.rectTransform.sizeDelta = size;
        }


        public enum Orientation
        {
            Vertical,
            Horizontal
        }
    }
}
