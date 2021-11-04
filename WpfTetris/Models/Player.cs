using System.Collections.Generic;
using System.Linq;

namespace WpfTetris.Models
{
    public class Player
    {
        private readonly List<int> _Scores = new List<int>();

        public Player(string nickName)
        {
            NickName = nickName;
        }

        public string NickName { get; }

        public void AddScore(int score)
        {
            _Scores.Add(score);
        }

        public int GetMaxScore()
        {
            return _Scores.Max();
        }
    }
}