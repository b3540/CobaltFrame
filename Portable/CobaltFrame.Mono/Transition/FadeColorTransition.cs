﻿using CobaltFrame.Mono.Context;
using CobaltFrame.Core.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobaltFrame.Core.Progress;
using CobaltFrame.Mono.Position;

namespace CobaltFrame.Mono.Transition
{
    public class FadeColorTransition:ScreenTransition
    {
        private Texture2D _texture;

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private InstantTimeAnimation<int> _animation;

        public InstantTimeAnimation<int> Animation
        {
            get { return _animation; }
            set { _animation = value; }
        }
        public Color TextureColor { get; private set; }

        public FadeColorTransition(Color color,int alphaFrom,int alphaTo,TimeSpan duration)
            :base(new Box2(0,0,0,0))
        {
            this.TextureColor = color;
            this._animation = new InstantTimeAnimation<int>(alphaFrom, alphaTo, duration, (from, to, progress) =>
            {
                if (to > from)
                {
                    return (int)((to - from) * progress);
                }
                else
                {
                    return (int)((from - to) *(1.0 - progress));
                }

                
            });
            this._layerDepth = 0.0f;
            this.AddObject(this._animation);
        }

        public override void Load()
        {
            base.Load();
            int width = this._game.GraphicsDevice.Viewport.Width;
            int height = this._game.GraphicsDevice.Viewport.Height;
            this.Texture = new Texture2D(this._game.GraphicsDevice,width,height);
            this.Texture.SetData<Color>(
                Enumerable.Repeat<Color>(TextureColor, width * height).ToArray()
                );
        }

        public override void Unload()
        {
            base.Unload();
            this.RemoveObject(this._animation);
            Texture.Dispose();
        }

        public override void Update(Core.Context.IFrameContext context)
        {
            base.Update(context);
            
        }

        public override void Draw(Core.Context.IFrameContext context)
        {
            
            base.Draw(context);
            this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, (context as FrameContext).ScreenTrans);
            this._spriteBatch.Draw(this.Texture, new Rectangle(0, 0, this._game.GraphicsDevice.Viewport.Width, this._game.GraphicsDevice.Viewport.Height),
                new Color(TextureColor.R, TextureColor.G, TextureColor.B, Animation.CurrentValue));
            this._spriteBatch.End();
            
        }

        public override void Start()
        {
            this._animation.OnCompleted += () => this.Translate();
            this._animation.Start();
        }
    }
}
