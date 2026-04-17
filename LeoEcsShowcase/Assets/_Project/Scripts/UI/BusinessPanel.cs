using System;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LeoEcsShowcase
{
    public class BusinessPanel : MonoBehaviour
    {
        [SerializeField] private Image _progressbar;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _lvlText;
        [SerializeField] private TextMeshProUGUI _incomeText;
        [SerializeField] private Button _lvlUpButton;
        [FormerlySerializedAs("_upgradeButton1")] [SerializeField] public UpgradeButton UpgradeButton1;
        [FormerlySerializedAs("_upgradeButton2")] [SerializeField] public UpgradeButton UpgradeButton2;
        public int ID;
        public int Entity;
        
        private EcsWorld _world;
        private EcsPool<IncomeTimerComponent> _incomeTimerPool;
        private EcsPool<LvlUpRequestComponent> _lvlUpRequestPool;

        private void Start()
        {
            UpgradeButton1.OnClick += UpgradeButtonOnOnClick;
            UpgradeButton2.OnClick += UpgradeButtonOnOnClick;
            _lvlUpButton.onClick.AddListener(Call);
        }

        private void Call()
        {
            _lvlUpRequestPool.Add(Entity);
        }

        private void Update()
        {
            ref var timerComp = ref _incomeTimerPool.Get(Entity);
            _progressbar.fillAmount = Mathf.InverseLerp(0, timerComp.DefaultValue, timerComp.CurrentValue);
        }

        private void UpgradeButtonOnOnClick(UpgradeButton button)
        {
            ref var request = ref _world.GetPool<UpgradeRequestComponent>().Add(Entity);
            request.UpgradeButton = button;
        }

        public void Init(BusinessConfig business, UpgradeConfig firstUpgrade, UpgradeConfig secondUpgrade)
        {
            InitUI(business);
            UpgradeButton1.InitUI(firstUpgrade);
            UpgradeButton2.InitUI(secondUpgrade);
        }

        public void SetWorld(EcsWorld world, int entity)
        {
            _world = world;
            Entity = entity;
            
            _incomeTimerPool = _world.GetPool<IncomeTimerComponent>();
            _lvlUpRequestPool = _world.GetPool<LvlUpRequestComponent>();
        }

        private void InitUI(BusinessConfig business)
        {
            SetName(business.Name);
            SetPrice(business.LvlPrice);
            SetIncome(business.Income);
            SetLvl(business.InitLvl);
        }

        public void SetName(string name)
        {
            _nameText.text = name;
        }
        
        public void SetPrice(float price)
        {
            _priceText.text = $"Price: {price}$";
        }
        
        public void SetIncome(float income)
        {
            _incomeText.text = $"Income:\n{income}$";
        }
        
        public void SetLvl(int lvl)
        {
            _lvlText.text = $"Lvl:\n{lvl}";
        }
    }
}

