using SpeedrunComSharp;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Speedrun_Olympics
{
    public class PointsReference
    {
        public float first { get; private set; }
        public float second { get; private set; }
        public float third { get; private set; }
        public float fourth { get; private set; }
        public PointsReference()
        {
            {
            }
        }
        public void CreateReference(float first, float second, float third, float fourth)
        {
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
        }
    }
    public class GameHandle
    {
        public Game game { get; }
        public Gametype type { get; }
        public PointsReference PointsReference = new PointsReference();
        public GameHandle(Game game, Gametype type)
        {
            this.game = game;
            this.type = type;
        }

    }
    public enum Gametype
    {
        levels,
        fullgame
    }
    public class PlayerHandle
    {
        public float points { get; set; }
        public byte[] placements = new byte[] { 0, 0, 0, 0 };
        GameHandle GH;

        PointsReference PR;
        public Player player { get; }
        public PlayerHandle(Player player, GameHandle GH)
        {
            this.player = player;
            this.GH = GH;

            PR = GH.PointsReference;
        }
        public override int GetHashCode()
        {
            return player.UserID.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null) return false;
            var other = obj as PlayerHandle;
            return player.UserID == other.player.UserID;
        }
        public short firstplaces()
        {
            return placements[0];
        }
        public short secondplaces()
        {
            return placements[1];
        }
        public short thirdplaces()
        {
            return placements[2];
        }
        public short fourthplaces()
        {
            return placements[3];
        }
        public void UpdatePoints(Run run)
        {
            if (run.Player.UserID == player.UserID)
            {
                switch (placement(run))
                {
                    case 0:
                        points += PR.first;
                        placements[0] += 1;
                        break;
                    case 1:
                        points += PR.second;
                        placements[1] += 1;
                        break;
                    case 2:
                        points += PR.third;
                        placements[2] += 1;
                        break;
                    case 3:
                        points += PR.fourth;
                        placements[3] += 1;
                        break;
                }
            }
        }
        private byte placement(Run run)
        {
            List<Run> categoryruns = new List<Run>();
            foreach (var runbuffer in GH.game.Runs)
            {
                try
                {
                    if (GH.type == Gametype.fullgame && runbuffer.Level == null)
                    {
                        if (run.Category.ID == runbuffer.Category.ID)
                        {
                            categoryruns.Add(runbuffer);
                        }
                    }
                    if (GH.type == Gametype.levels && runbuffer.Level != null)
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
