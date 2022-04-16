
using UnityEngine;

namespace ColourBlast.Helpers
{
    public struct OnClickEventHandler
    {
        public CellPosition Positon { get; private set; }
        public OnClickEventHandler(CellPosition position)
        {
            Positon = position;
        }
    }
}