using SpeedrunComSharp;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Speedrun_Olympics
{
    public class PointsReference
    {
        public float First { get; private set; }
        public float Second { get; private set; }
        public float Third { get; private set; }
        public float Fourth { get; private set; }
        public PointsReference()
        {
            {
            }
        }
        public void CreateReference(float first, float second, float third, float fourth)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Fourth = fourth;
        }
    }
    public class GameHandle
    {
        public Game Game { get; }
        public Gametype Type { get; }
        public PointsReference PointsReference = new PointsReference();
        public GameHandle(Game game, Gametype type)
        {
            this.Game = game;
            this.Type = type;
        }

    }
    public enum Gametype
    {
        levels,
        fullgame
    }
    public class PlayerHandle
    {
        public float Points { get; set; }
        public byte[] placements = new byte[] { 0, 0, 0, 0 };
        GameHandle GH;
        public string Name { get; private set; }
        PointsReference PR;
        public Player Player { get; }
        public PlayerHandle(Player player, GameHandle GH)
        {
            Player = player;
            this.GH = GH;
            Name = player.Name;
            PR = GH.PointsReference;
        }
        public override int GetHashCode()
        {
            return Player.UserID.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null) return false;
            var other = obj as PlayerHandle;
            return Player.UserID == other.Player.UserID;
        }
        public short Firstplaces()
        {
            return placements[0];
        }
        public short Secondplaces()
        {
            return placements[1];
        }
        public short Thirdplaces()
        {
            return placements[2];
        }
        public short Fourthplaces()
        {
            return placements[3];
        }
        public void UpdatePoints(Run run)
        {
            if (run.Player.UserID == Player.UserID)
            {
                switch (Placement(run))
                {
                    case 0:
                        Points += PR.First;
                        placements[0] += 1;
                        break;
                    case 1:
                        Points += PR.Second;
                        placements[1] += 1;
                        break;
                    case 2:
                        Points += PR.Third;
                        placements[2] += 1;
                        break;
                    case 3:
                        Points += PR.Fourth;
                        placements[3] += 1;
                        break;
                }
            }
        }
        private byte Placement(Run run)
        {
            List<Run> categoryruns = new List<Run>();
            foreach (var runbuffer in GH.Game.Runs)
            {
                try
                {
                    if (GH.Type == Gametype.fullgame && runbuffer.Level == null)
                    {
                        if (run.Category.ID == runbuffer.Category.ID)
                        {
                            categoryruns.Add(runbuffer);
                        }
                    }
                    if (GH.Type == Gametype.levels && runbuffer.Level != null)
                    {
                        if ((run.Level.ID == runbuffer.Level.ID) && (run.Category.ID == runbuffer.Category.ID))
                        {
                            categoryruns.Add(runbuffer);
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                }
            }
            categoryruns = categoryruns.OrderBy(c => c.Times.Primary.Value.TotalSeconds).ToList();
            return (byte)(categoryruns.IndexOf(run));
        }
    }
}
