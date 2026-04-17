using System;
using System.Collections.Generic;
using System.Linq;
//using LeoEcsShowcase.Components;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeoEcsShowcase
{
    public sealed class Game : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private List<BusinessPanel> _businessPanels; 
        [SerializeField] private BusinessConfigList _businessConfigs;
        [SerializeField] private UpgradesConfigList _upgradesConfigs;

        public static int BalanceEntity = 0;
        
        private IEcsSystems _systems;
        private IEcsSystems _requestSystems;
        private IEcsSystems _lateSystems;

        private EcsWorld _world;
        private SharedData _sharedData;

        private void Start()
        {            
            var sharedData = new SharedData(_upgradesConfigs);
            
            _requestSystems = new EcsSystems(_world, sharedData);
            _systems = new EcsSystems(_world, sharedData);
            _lateSystems = new EcsSystems(_world);

            _requestSystems
                .Add(new UpgradeRequestSystem())
                .Add(new LvlUpRequestSystem());
            _systems
                .Add(new IncomeTimerSystem())
                .Add(new IncomeGainSystem());
            _lateSystems
                .Add(new UpdateUILateSystem());
            
            _requestSystems.Init();
            _systems.Init();
            _lateSystems.Init();

            var uiInitializer = new UIInitializer(_businessPanels, _upgradesConfigs, _businessConfigs);
            
            uiInitializer.Init();
            
            InitEntities();
        }

        private void Update()
        {
            _requestSystems.Run();
            _systems.Run();
            _lateSystems.Run();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
            }
            if (_requestSystems != null) {
                _requestSystems.Destroy ();
                _requestSystems = null;
            }
            if (_lateSystems != null) {
                _lateSystems.Destroy ();
                _lateSystems = null;
            }
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }

        public void Init(EcsWorld world)
        {
            _world = world;
        }
        
        private void InitEntities()
        {
            CreateBalanceEntity();
            CreateBusinessEntities();
        }

        private void CreateBusinessEntities()
        {
            BusinessConfig businessConfig;
            foreach (var businessPanel in _businessPanels)
            {
                int entity = _world.NewEntity();
                
                businessConfig = _businessConfigs.list.First(x => x.ID == businessPanel.ID);
                businessPanel.SetWorld(_world, entity);

                ref var lvl = ref _world.GetPool<LevelComponent>().Add(entity);
                lvl.Value = businessConfig.InitLvl;
                lvl.Price = businessConfig.LvlPrice;
                lvl.InitPrice = businessConfig.LvlPrice;
                
                ref var incomeTimer = ref _world.GetPool<IncomeTimerComponent>().Add(entity);
                incomeTimer.CurrentValue = 0f;
                incomeTimer.DefaultValue = businessConfig.IncomeTimer;
                incomeTimer.IsReady = false;
                
                ref var income = ref _world.GetPool<IncomeComponent>().Add(entity);
                income.InitValue = businessConfig.Income;
                income.Value = businessConfig.Income;
                
                ref var upgrades = ref _world.GetPool<UpgradeComponent>().Add(entity);
                upgrades.IncomeMultiplier = 1;

                ref var updateUI = ref _world.GetPool<UpdateUIComponent>().Add(entity);
                updateUI.BusinessPanel = businessPanel;
                updateUI.IsReady = false;
                
                businessPanel.Entity = entity;
            }
        }

        private void CreateBalanceEntity()
        {
            int entity = _world.NewEntity();
            ref var balance = ref _world.GetPool<BalanceComponent>().Add(entity);
            balance.Value = 0f;

            _balanceText.text = balance.Value.ToString();
            _world.GetPool<BalanceUIComponent>().Add(entity).Value = _balanceText;
            
            BalanceEntity = entity;
        }
    }
}
