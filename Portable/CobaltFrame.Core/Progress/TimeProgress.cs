﻿using CobaltFrame.Core.Context;
using CobaltFrame.Core.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobaltFrame.Core.Progress
{
    public abstract class TimeProgress<T>:UpdatableObject,ITimeProgress<T>
    {
        public event Action OnCompleted;

        public event Action OnStarted;

        private ProgressState _state;
        public ProgressState State
        {
            get { return _state; }
        }
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
        }
        private TimeSpan _elapsedTime;
        public TimeSpan ElapsedTime
        {
            get { return this._elapsedTime; }
        }
        private TimeSpan _beginTime;
        public TimeSpan BeginTime
        {
            get { return _beginTime; }
        }
        private float _currentProgress;
        public float CurrentProgress
        {
            get { return this._currentProgress; }
            set { this._currentProgress = value; }
        }
        private bool _isLoop;
        public bool IsLoop
        {
            get 
            { 
                return this._isLoop; 
            }
            set
            {
                this._isLoop = value;
            }
        }


        private ProgressState _beforeState;



        protected T _beginValue;


        public T BeginValue
        {
            get
            {
                return this._beginValue;
            }
            set
            {
                this._beginValue = value;
            }
        }

        protected T _endValue;
        public T EndValue
        {
            get
            {
                return this._endValue;
            }
            set
            {
                this._endValue = value;
            }
        }

        protected T _currentValue;
        public T CurrentValue
        {
            get
            {
                return this._currentValue;
            }
            set
            {
                this._currentValue = value;
            }
        }
        
        public TimeProgress(TimeSpan duration,T begin,T end)
            :base()
        {
            
            this._beforeState = ProgressState.Stop;
            this._state = ProgressState.Stop;
            this._isLoop = false;
            this._duration = duration;
            this.OnCompleted += () => { };
            this.OnStarted += () => { };
            this._beginValue = begin;
            this._endValue = end;
            this._currentValue = begin;
        }

        public override void Update(IFrameContext frameContext)
        {
            base.Update(frameContext);
            if (this.State == ProgressState.Active)
            {
                if (this._beforeState == ProgressState.Stop && this.State == ProgressState.Active)
                {
                    this._beginTime = frameContext.TotalGameTime;
                    this.OnStarted();
                }

                if (this._beforeState == ProgressState.Pause && this.State == ProgressState.Active)
                {
                    //Pause状態から再開した場合は現在の時間から経過した時間を戻す事によって初期時間を復元
                    this._beginTime = frameContext.TotalGameTime - ElapsedTime;
                }

                this._elapsedTime = frameContext.TotalGameTime - this.BeginTime;
                this._currentProgress = (float)(this.ElapsedTime.TotalSeconds / this.Duration.TotalSeconds);
                this._currentValue = this.UpdateExpression(this._beginValue,this._endValue,this._currentProgress);
                if (this._currentProgress >= 1.0f)
                {
                    if (this.IsLoop)
                    {
                        this._currentProgress = 0.0f;
                        this._beginTime = frameContext.TotalGameTime;
                    }
                    else
                    {
                        this._currentProgress = 1.0f;
                        this._state = ProgressState.Stop;
                        this.OnCompleted();
                    }
                }

                 
            }


            this._beforeState = State;
        }

        public virtual void Start()
        {
            this._currentProgress = 0.0f;
            this._state = ProgressState.Active;
            
        }

        public virtual void Pause()
        {
            this._state = ProgressState.Pause;
        }

        public virtual void Resume()
        {
            this._state = ProgressState.Active;
        }

        public virtual void Stop()
        {
            this._state = ProgressState.Stop;
        }


        protected abstract T UpdateExpression(T begin, T end, float currentProgress);




    }
}
