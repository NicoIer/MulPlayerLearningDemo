using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class KitchenObjIcon : MonoBehaviour
    {
        [SerializeField]private Image iconImage;
        [SerializeField]private Image backgroundImage;
        public KitchenObjEnum objEnum { get; private set; }

        public void SetData(KitchenObjSo dataSo)
        {
            iconImage.sprite = dataSo.sprite;
            objEnum = dataSo.kitchenObjEnum;
        }
    }
}