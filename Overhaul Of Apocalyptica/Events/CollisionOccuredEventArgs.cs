using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Overhaul_Of_Apocalyptica.Events;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Events
{
    class CollisionOccuredEventArgs : EventArgs
    {
        public IEntity Reciving { get; set; }
    }
}
