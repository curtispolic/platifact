﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class MinerPanel : UIElement
{
    public Miner miner;

    // Constructor
    public MinerPanel(Miner m)
    {
        position = new Vector2(100, 100);
        miner = m;
    }
}
