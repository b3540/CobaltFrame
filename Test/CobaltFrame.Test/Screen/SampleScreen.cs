﻿using CobaltFrame.Animation;
using CobaltFrame.Context;
using CobaltFrame.Object;
using CobaltFrame.Position;
using CobaltFrame.Screen;
using CobaltFrame.Sound;
using CobaltFrame.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobaltFrame.Test.Screen
{
    public class SampleScreen:GameScreen
    {
        protected Texture2DObject _texture2DObj;
        
        public SampleScreen(GameContext context)
            : base(context)
        {
            //2Dオブジェクトの作成
            this._texture2DObj = new Texture2DObject(context,new Position2D(new Rectangle(100,100,100,100)),"Texture/face");
            //var sound = new SoundObject(context, "warp2");
            //this.AddObject(sound);
            //2秒間(0,0)→(200,200)に移動するアニメーション
            var animation = new Position2DTimeAnimation(context, TimeSpan.FromSeconds(2), new Position2D(new Rectangle(0, 0, 100, 100)), new Position2D(new Rectangle(200, 200, 100, 100)));
            //1秒間(200,200)で待つアニメーションをチェイン
            animation.Chain(new WaitPosition2DAnimation(context, TimeSpan.FromSeconds(1), new Position2D(new Rectangle(200, 200, 100, 100))), (progress) =>
            {
                Debug.WriteLine("Wait");
            })
            //3秒間(200,200)→(400,100)に移動するアニメーションをチェイン
            .Chain(new Position2DTimeAnimation(context, TimeSpan.FromSeconds(3), new Position2D(new Rectangle(200, 200, 100, 100)), new Position2D(new Rectangle(400, 100, 100, 100))), (progress) =>
            {
                //sound.Play();
            })
            //すべてのアニメーション終了時の処理をチェイン
            .Chain(() =>
            {
                Debug.WriteLine("Complete");
            });

            //アニメーションをオブジェクトにアタッチ
            this._texture2DObj.AttachAnimation(animation);
            //オブジェクトをスクリーンに追加
            this.AddDrawableObject(this._texture2DObj);
            
            var button = new ButtonObject(context,new RelativePosition2D(new Rectangle(120,120,120,80),this._texture2DObj.Position),"Texture/button_on","Texture/button_off");
            this.AddDrawableObject(button);
            button.OnClick += (btn,pos) =>
            {
                //アニメーションを開始
                animation.Start();
            };

            BitmapTextObject text = new BitmapTextObject(context,new Position2D(new Rectangle(0,300,0,0)),"Font/meiryo","今日も1日がんばるぞい!",0.8f,Color.Red);
            this.AddDrawableObject(text);
            BitmapTextObject text1 = new BitmapTextObject(context, new Position2D(new Rectangle(0, 350, 0, 0)), "Font/meiryo", "今日も1日がんばるぞい!", 1.0f, Color.White);
            this.AddDrawableObject(text1);
            BitmapTextObject text2 = new BitmapTextObject(context, new Position2D(new Rectangle(0, 450, 0, 0)), "Font/meiryo", "今日も1日がんばるぞい!", 4.0f, Color.Green);
            this.AddDrawableObject(text2);
            
        }

        
    }
}