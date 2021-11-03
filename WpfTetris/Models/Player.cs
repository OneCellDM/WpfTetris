using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTetris.Models
{
    public class Player
    {
        private List<int> _Scores = new List<int>();
        public  string NickName { get; private set; }
        public Player(string nickName) => NickName = nickName;
        public void AddScore(int score) => _Scores.Add(score);

        public int GetMaxScore() => _Scores.Max();
    }
}
