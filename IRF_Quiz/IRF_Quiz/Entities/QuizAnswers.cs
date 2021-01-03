using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRF_Quiz.Entities
{
    class QuizAnswers
    {
        public long GameID { get; set; }
        public int PlayerID { get; set; }
        public long QuestionID { get; set; }
        public bool Result { get; set; }
        public int AnswerID { get; set; }
        public DateTime Date { get; set; }
    } 
}
