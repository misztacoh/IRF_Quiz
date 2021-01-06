using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRF_Quiz.Entities
{
    class ChartValues
    {
        public int Answers { get; set; }
        public DateTime Date { get; set; }

        public int PlayerID { get; set; }

        public bool Result { get; set; }
    }
}
