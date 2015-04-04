﻿using CobaltFrame.Context;
using CobaltFrame.Position;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobaltFrame.UI
{
    public class ButtonObject:UIObject
    {
        protected ButtonState _state;

        public ButtonState State
        {
            get { return _state; }
        }

        protected ButtonState _beforeState;

        protected Texture2D _pressedTexture;
        protected Texture2D _releasedTexture;

        private string _pressedTexturePath;

        protected string PressedTexturePath
        {
            get { return _pressedTexturePath; }
        }

        private string _releasedTexturePath;

        protected string ReleasedTexturePath
        {
            get { return _releasedTexturePath; }
            set { _releasedTexturePath = value; }
        }

        private Vector2 _textureScale;

        private Vector2 TextureScale
        {
            get { return _textureScale; }
        }

        public event Action<ButtonObject, Vector2> OnClick;
        public ButtonObject(GameContext context,Position2D position,string pressedTexturePath,string releasedTexturePath)
            : base(context,position)
        {
            this._pressedTexturePath = pressedTexturePath;
            this._releasedTexturePath = releasedTexturePath;
            this._state = ButtonState.Released;
            this._beforeState = ButtonState.Released;
            this.OnClick += (s, pos) => { };
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadObject()
        {
            base.LoadObject();
            this._pressedTexture = this._game.Content.Load<Texture2D>(this._pressedTexturePath);
            this._releasedTexture = this._game.Content.Load<Texture2D>(this._releasedTexturePath);
            this._origin = new Vector2(this._releasedTexture.Width / 2.0f, this._releasedTexture.Height / 2.0f);
            this._textureScale = new Vector2((float)this._position.GetPosition().Width / (float)this._releasedTexture.Width, (float)this._position.GetPosition().Height / (float)this._releasedTexture.Height);
        }

        public override void UnloadObject()
        {
            base.UnloadObject();
            this._pressedTexture.Dispose();
            this._releasedTexture.Dispose();
        }

        public override void Update(Core.Context.IFrameContext context)
        {
            base.Update(context);
            
            TouchCollection collection = TouchPanel.GetState();
            
            foreach (TouchLocation state in collection)
            {
                
                int id = state.Id;
                TouchLocationState tLState = state.State;
                var input = Vector2.Transform(state.Position,(context as FrameContext).GetScreenTransInvert());
                if (tLState == TouchLocationState.Pressed || tLState==TouchLocationState.Moved)
                {
                    
                    if (this._position.Contains((int)input.X, (int)input.Y))
                    {
                        
                        if (tLState==TouchLocationState.Pressed)
                        {
                            OnClick(this, state.Position);
                        }
                        this._state = ButtonState.Pressed;
                        break;
                    }
                }
                else
                {
                    this._state = ButtonState.Released;
                }
                
            }

            this._beforeState = this._state;
        }

        public override void Draw(Core.Context.IFrameContext context)
        {
            base.Draw(context);
            this._spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, (context as FrameContext).ScreenTrans);
            
            switch (this._state)
            {
                case ButtonState.Released:
                    this._spriteBatch.Draw(this._releasedTexture, null, this._position.GetPosition(this._origin*this._textureScale), null, this._origin, this._rotation, null, this._drawColor, SpriteEffects.None, 0.0f);
                    
                    break;
                case ButtonState.Pressed:
                    this._spriteBatch.Draw(this._pressedTexture, null, this._position.GetPosition(this._origin*this._textureScale), null, this._origin, this._rotation, null, this._drawColor, SpriteEffects.None, 0.0f);
                    break;
            }
            this._spriteBatch.End();
        }
    }
}
