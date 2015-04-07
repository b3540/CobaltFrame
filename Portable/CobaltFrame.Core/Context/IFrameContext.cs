﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobaltFrame.Core.Context
{
    public interface IFrameContext
    {
        TimeSpan TotalGameTime { get; }

        TimeSpan ElapsedGameTime { get; }
    }
}
