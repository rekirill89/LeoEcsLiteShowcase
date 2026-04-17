using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace LeoEcsShowcase
{
    public class UIInitializer
    {
        private readonly List<BusinessPanel> _businessPanels;
        private readonly UpgradesConfigList _upgradesConfig;
        private readonly BusinessConfigList _businessConfig;

        public UIInitializer(List<BusinessPanel> businessPanels, UpgradesConfigList upgradesConfig, BusinessConfigList businessConfig)
        {
            _businessPanels = businessPanels;
            _upgradesConfig = upgradesConfig;
            _businessConfig = businessConfig;
        }

        public void Init()
        {
            int firstButtonID;
            int secondButtonID;
            for (int i = 0; i < _businessPanels.Count; i++)
            {
                firstButtonID = _businessPanels[i].UpgradeButton1.ID;
                secondButtonID = _businessPanels[i].UpgradeButton2.ID;

                _businessPanels[i].Init(
                    _businessConfig.list.First(x => x.ID == _businessPanels[i].ID), 
                    _upgradesConfig.List.First(x => x.UpgradeButtonID == firstButtonID),
                    _upgradesConfig.List.First(x => x.UpgradeButtonID == secondButtonID));
            }
        }
    }
}