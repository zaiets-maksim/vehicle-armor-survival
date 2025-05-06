﻿using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Zenject;

namespace Infrastructure.StateMachine.Game
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(DiContainer container) : base(container)
        {
        }

        protected override Dictionary<Type, Func<IExitable>> BuildStatesRegister(DiContainer container)
        {
            return new Dictionary<Type, Func<IExitable>>()
            {
                [typeof(BootstrapState)] = container.Resolve<BootstrapState>,
                [typeof(LoadProgressState)] = container.Resolve<LoadProgressState>,
                [typeof(LoadLevelState)] = container.Resolve<LoadLevelState>,
                [typeof(MenuLevelState)] = container.Resolve<MenuLevelState>,
                [typeof(GameLoopState)] = container.Resolve<GameLoopState>,
                
            };
        }
    }
}