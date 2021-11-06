using System.Collections.Generic;
using System.Linq;

namespace WpfTetris.Models
{
    public class Player
    {
        public readonly List<int> Scores = new List<int>();

        public Player(string nickName)
        {
            NickName = nickName;
        }

        public string NickName { get; }

        public void AddScore(int score)
        {
            Scores.Add(score);
        }

        public int GetMaxScore()
        {
            return Scores.Max();
        }
    }
}