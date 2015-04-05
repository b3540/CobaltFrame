﻿using CobaltFrame.Context;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobaltFrame.Screen
{
    public class GameScreen:CobaltFrame.Core.Screen.Screen
    {
        protected Game _game;
        protected TimeSpan _screenElapsedTime;
        public GameScreen(GameContext context)
            : base(context)
        {
            this._game = context.Game;
        }

        public override void Draw(Core.Context.IFrameContext context)
        {
            
            
            base.Draw(context);
        }

        public override void NavigateTo(object parameter)
        {
            base.NavigateTo(parameter);
            this._screenElapsedTime = this._game.TargetElapsedTime;
        }

        
    }
}
