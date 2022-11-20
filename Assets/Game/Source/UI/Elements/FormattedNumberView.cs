using TMPro;
using UnityEngine;

namespace Splatrika.MobArenaMobile.UI
{
    public class FormattedNumberView : NumberView
    {
        [SerializeField]
        private TextMeshProUGUI _view;

        [SerializeField]
        private string _format = "Value: {0}";


        public override void Show(float number)
        {
            _view.text = string.Format(_format, number);
        }
    }
}
