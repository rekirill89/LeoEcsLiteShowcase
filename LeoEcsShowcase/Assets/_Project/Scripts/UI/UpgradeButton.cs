using System;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LeoEcsShowcase
{
    public class UpgradeButton : MonoBehaviour
    {
        public event Action<UpgradeButton> OnClick;
        
        [SerializeField] private TextMeshProUGUI _priceText;
        [FormerlySerializedAs("_incomeText")] [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _nameText;
        
        public int ID;
        public Button Button;
        
        private void Awake()
        {
            Button = GetComponent<Button>();
        }

        private void Start()
        {
            Button.onClick.AddListener(OnClickHandler);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClickHandler);
        }

        public void InitUI(UpgradeConfig upgradeConfig)
        {
            _nameText.text = upgradeConfig.Name;
            _multiplierText.text = (upgradeConfig.IncomeMultiplier * 100f) + "%";
            _priceText.text = upgradeConfig.Price.ToString();
        }
        
        public void LockButton()
        {
            Button.interactable = false;
            _priceText.text = "Bought";
        }
        
        private void OnClickHandler()
        {
            OnClick?.Invoke(this);
        }
    }
}