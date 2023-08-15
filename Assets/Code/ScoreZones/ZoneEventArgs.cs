using System;

namespace Code.ScoreZones
{
    public class ZoneEventArgs : EventArgs
    {
        public ZoneType Zone;

        public ZoneEventArgs(ZoneType zone)
        {
            Zone = zone;
        }
    }
}