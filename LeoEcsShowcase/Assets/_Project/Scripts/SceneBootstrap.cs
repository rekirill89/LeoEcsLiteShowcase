using System;
//using LeoEcsShowcase.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcsShowcase
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private Game _game;
        [SerializeField] private LogInfoButton _button;

        private void Awake()
        {
            var world = new EcsWorld();
            
            _button.Init(world);
            _game.Init(world);
        }
    }
}