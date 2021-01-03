using IRF_Quiz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_Quiz
{
    public partial class Quizes : UserControl
    {
        private CheckedListBox cbxCategories;
        private Splitter splitter1;
        private ComboBox cbUser;
        private Button btnStart;
        private Splitter splitter2;
        private Timer timer1;
        private System.ComponentModel.IContainer components;
        private Label lblQuestion;
        private Label lblAnswer1;
        private Label lblAnswer2;
        private Label lblAnswer3;
        private Button btnAnswer1;
        private Button btnAnswer2;
        private Button btnAnswer3;
        private Label lblResult;
        private Label lblCouner;

        QuizEntities context = new QuizEntities();
        List<QuizQuestions> quizQuestions = new List<QuizQuestions>();

        private int CurrentCorrectAnswer;
        private int CurrentPlayer;

        public Quizes()
        {
            InitializeComponent();

            QuizHide();
            cbxCategories.Visible = false;

          //QuizShow();
        }

        private void Quizes_Load_1(object sender, EventArgs e)
        {
            context.Players.Load();
            context.Questions.Load();
            context.Answers.Load();
            context.Categories.Load();

            cbxCategories.DataSource = context.Categories.Local;
            cbxCategories.DisplayMember = "CategoryName";

            cbUser.DataSource = context.Players.Local;
            cbUser.DisplayMember = "PlayerName";

        }



        private void btnStart_Click(object sender, EventArgs e)
        {
            string playerName = cbUser.SelectedItem.ToString();
            var player = from x in context.Players
                         where x.PlayerName.Equals(playerName)
                         select new { playerID = x.PlayerID };

            foreach (var item in player)
            {
                CurrentPlayer = item.playerID;
            }

            for (int i = 1171; i < 1172; i++)
            {
                var question = from x in context.Questions
                               where x.QuestionID.Equals(i)
                               select new { QuestionText = x.QuestionText };
                var answer = from x in context.Answers
                               where x.QuestionID.Equals(i)
                               select new { Answer1 = x.A1Text, Answer2 = x.A2Text, Answer3 = x.A3Text, Solution = x.CorrectAnswer  };

                QuizQuestions qq = new QuizQuestions();
                qq.QuestionText = question.ToString();

                foreach (var item in question)
                {
                    qq.QuestionText = item.QuestionText;
                }

                foreach (var item in answer)
                {
                    qq.Answer1Text = item.Answer1;
                    qq.Answer2Text = item.Answer2;
                    qq.Answer3Text = item.Answer3;
                    qq.AnswerID = item.Solution;
                }

                quizQuestions.Add(qq);

                foreach (var item in quizQuestions)
                {
                    lblQuestion.Text = item.QuestionText;
                    lblAnswer1.Text = item.Answer1Text;
                    lblAnswer2.Text = item.Answer2Text;
                    lblAnswer3.Text = item.Answer3Text;
                    CurrentCorrectAnswer = item.AnswerID;
                    QuizShow();


                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        private void QuizShow()
        {
            lblCouner.Visible = true;
            lblQuestion.Visible = true;
            lblAnswer1.Visible = true;
            lblAnswer2.Visible = true;
            lblAnswer3.Visible = true;
            btnAnswer1.Visible = true;
            btnAnswer2.Visible = true;
            btnAnswer3.Visible = true;
        }

        private void QuizHide()
        {
            lblCouner.Visible = false;
            lblQuestion.Visible = false;
            lblAnswer1.Visible = false;
            lblAnswer2.Visible = false;
            lblAnswer3.Visible = false;
            lblResult.Visible = false;
            btnAnswer1.Visible = false;
            btnAnswer2.Visible = false;
            btnAnswer3.Visible = false;
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbxCategories = new System.Windows.Forms.CheckedListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblCouner = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblQuestion = new System.Windows.Forms.Label();
            this.lblAnswer1 = new System.Windows.Forms.Label();
            this.lblAnswer2 = new System.Windows.Forms.Label();
            this.lblAnswer3 = new System.Windows.Forms.Label();
            this.btnAnswer1 = new System.Windows.Forms.Button();
            this.btnAnswer2 = new System.Windows.Forms.Button();
            this.btnAnswer3 = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbxCategories
            // 
            this.cbxCategories.FormattingEnabled = true;
            this.cbxCategories.Location = new System.Drawing.Point(161, 20);
            this.cbxCategories.Name = "cbxCategories";
            this.cbxCategories.Size = new System.Drawing.Size(192, 94);
            this.cbxCategories.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 593);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // cbUser
            // 
            this.cbUser.FormattingEnabled = true;
            this.cbUser.Location = new System.Drawing.Point(34, 20);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(121, 21);
            this.cbUser.TabIndex = 2;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(34, 92);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblCouner
            // 
            this.lblCouner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCouner.AutoSize = true;
            this.lblCouner.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCouner.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCouner.Location = new System.Drawing.Point(597, 4);
            this.lblCouner.Name = "lblCouner";
            this.lblCouner.Size = new System.Drawing.Size(51, 37);
            this.lblCouner.TabIndex = 4;
            this.lblCouner.Text = "10";
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(3, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 593);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoSize = true;
            this.lblQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestion.Location = new System.Drawing.Point(29, 189);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(98, 25);
            this.lblQuestion.TabIndex = 6;
            this.lblQuestion.Text = "Question";
            // 
            // lblAnswer1
            // 
            this.lblAnswer1.AutoSize = true;
            this.lblAnswer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnswer1.Location = new System.Drawing.Point(115, 265);
            this.lblAnswer1.Name = "lblAnswer1";
            this.lblAnswer1.Size = new System.Drawing.Size(67, 24);
            this.lblAnswer1.TabIndex = 7;
            this.lblAnswer1.Text = "Answ1";
            // 
            // lblAnswer2
            // 
            this.lblAnswer2.AutoSize = true;
            this.lblAnswer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnswer2.Location = new System.Drawing.Point(115, 322);
            this.lblAnswer2.Name = "lblAnswer2";
            this.lblAnswer2.Size = new System.Drawing.Size(67, 24);
            this.lblAnswer2.TabIndex = 8;
            this.lblAnswer2.Text = "Answ2";
            // 
            // lblAnswer3
            // 
            this.lblAnswer3.AutoSize = true;
            this.lblAnswer3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnswer3.Location = new System.Drawing.Point(115, 381);
            this.lblAnswer3.Name = "lblAnswer3";
            this.lblAnswer3.Size = new System.Drawing.Size(67, 24);
            this.lblAnswer3.TabIndex = 9;
            this.lblAnswer3.Text = "Answ3";
            // 
            // btnAnswer1
            // 
            this.btnAnswer1.Location = new System.Drawing.Point(34, 265);
            this.btnAnswer1.Name = "btnAnswer1";
            this.btnAnswer1.Size = new System.Drawing.Size(75, 23);
            this.btnAnswer1.TabIndex = 10;
            this.btnAnswer1.Text = "Válasz";
            this.btnAnswer1.UseVisualStyleBackColor = true;
            // 
            // btnAnswer2
            // 
            this.btnAnswer2.Location = new System.Drawing.Point(34, 325);
            this.btnAnswer2.Name = "btnAnswer2";
            this.btnAnswer2.Size = new System.Drawing.Size(75, 23);
            this.btnAnswer2.TabIndex = 11;
            this.btnAnswer2.Text = "Válasz";
            this.btnAnswer2.UseVisualStyleBackColor = true;
            // 
            // btnAnswer3
            // 
            this.btnAnswer3.Location = new System.Drawing.Point(34, 384);
            this.btnAnswer3.Name = "btnAnswer3";
            this.btnAnswer3.Size = new System.Drawing.Size(75, 23);
            this.btnAnswer3.TabIndex = 12;
            this.btnAnswer3.Text = "Válasz";
            this.btnAnswer3.UseVisualStyleBackColor = true;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(29, 464);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(78, 25);
            this.lblResult.TabIndex = 13;
            this.lblResult.Text = "Helyes";
            // 
            // Quizes
            // 
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnAnswer3);
            this.Controls.Add(this.btnAnswer2);
            this.Controls.Add(this.btnAnswer1);
            this.Controls.Add(this.lblAnswer3);
            this.Controls.Add(this.lblAnswer2);
            this.Controls.Add(this.lblAnswer1);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.lblCouner);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cbUser);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.cbxCategories);
            this.Name = "Quizes";
            this.Size = new System.Drawing.Size(651, 593);
            this.Load += new System.EventHandler(this.Quizes_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
