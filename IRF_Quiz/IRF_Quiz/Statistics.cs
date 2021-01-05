using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Windows.Forms.DataVisualization.Charting;
using IRF_Quiz.Entities;
using System.Drawing;

namespace IRF_Quiz
{
    public partial class Statistics : UserControl
    {
        private CheckedListBox cboxPlayers;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private CheckedListBox cboxCategories;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        QuizEntities context = new QuizEntities();
        List<Quiz> QuizList = new List<Quiz>();
        List<ChartValues> chartValues = new List<ChartValues>();
        List<Player> sPlayersList = new List<Player>();
        List<Category> sCategoriesList = new List<Category>();
        List<PlayerColor> sPlayerColors = new List<PlayerColor>();
        Color seriesColor;
        public Statistics()
        {
            InitializeComponent();
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            context.Quizs.Load();
            context.Players.Load();
            context.Categories.Load();
            context.Questions.Load();

            cboxCategories.DataSource = context.Categories.Local;
            cboxCategories.DisplayMember = "CategoryName";
            cboxPlayers.DataSource = context.Players.Local;
            cboxPlayers.DisplayMember = "PlayerName";

            var dates = from x in context.Quizs
                          select x.Date;
            List<DateTime> datesList = dates.ToList<DateTime>();
            DateTime minDate = datesList.Min();

            dtpFrom.Value = minDate;
            dtpTo.Value = DateTime.Today;

            for (int i = 0; i < cboxCategories.Items.Count; i++)
            {
                cboxCategories.SetItemChecked(i, true);
            }

            for (int i = 0; i < cboxPlayers.Items.Count; i++)
            {
                cboxPlayers.SetItemChecked(i, true);
                PlayerColor pc = new PlayerColor();
                var selected = (Player)cboxPlayers.Items[i];
                pc.pcolorID = selected.PlayerID;
                pc.Color = GetColor();
                sPlayerColors.Add(pc);
                //sPlayersList.Add(selected);
            }
        }

        private void DrawCharts()
        {

            for (int i = 0; i < sPlayersList.Count; i++)
            {
                var col = from x in sPlayerColors
                          where x.pcolorID.Equals(sPlayersList[i].PlayerID)
                          select new { color = x.Color };

                foreach (var item in col)
                {
                    seriesColor = item.color;
                }

                chartValues.Clear();
                GetValues(true);

                chart2.DataSource = chartValues;

                var series1 = chart1.Series[0];
                series1.ChartType = SeriesChartType.Line;
                series1.XValueMember = "Xvalues";
                series1.YValueMembers = "Yvalues";
                series1.BorderWidth = 2;
                series1.Color = Color.FromArgb(255, 255, 255);

                var legend1 = chart1.Legends[0];
                legend1.Enabled = false;

                var chartArea1 = chart2.ChartAreas[0];
                chartArea1.AxisX.MajorGrid.Enabled = false;
                chartArea1.AxisY.MajorGrid.Enabled = false;
                chartArea1.AxisY.IsStartedFromZero = false;

                chartValues.Clear();
                GetValues(false);

                chart2.DataSource = chartValues;

                var series2 = chart1.Series[0];
                series2.ChartType = SeriesChartType.Line;
                series2.XValueMember = "Xvalues";
                series2.YValueMembers = "Yvalues";
                series2.BorderWidth = 2;
                series2.Color = seriesColor;

                var legend2 = chart1.Legends[0];
                legend2.Enabled = false;

                var chartArea2 = chart1.ChartAreas[0];
                chartArea2.AxisX.MajorGrid.Enabled = false;
                chartArea2.AxisY.MajorGrid.Enabled = false;
                chartArea2.AxisY.IsStartedFromZero = false;
            }
        }

        private void GetValues(bool v)
        {
            for (int i = 0; i < sPlayersList.Count; i++)
            {
                var answersCount = from x in QuizList
                                   where x.Result.Equals(v) && x.PlayerFK.Equals(sPlayersList[i].PlayerID)
                                   group x by x.Date into g
                                   select new { date = g.Key, ans = g.Count(x => x.Result) };

                foreach (var item in answersCount)
                {
                    ChartValues cv = new ChartValues();
                    cv.Xvalues = item.date;
                    cv.Yvalues = item.ans;
                    chartValues.Add(cv);
                }
            }
        }

        private void cboxPlayers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                Player toAdd = (Player)cboxPlayers.Items[e.Index];
                sPlayersList.Add(toAdd);

                FillUpQuiz();
                DrawCharts();

                //for (int i = 0; i < cboxPlayers.Items.Count; i++)
                //{
                //    if (cboxPlayers.GetItemChecked(i))
                //    {
                //        var selected = (Player)cboxPlayers.Items[i];
                //        if (!sPlayersList.Contains(selected))
                //        {
                //            sPlayersList.Add(selected);
                //        }
                //    }
                //}
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                Player toRemove = (Player)cboxPlayers.Items[e.Index];
                sPlayersList.RemoveAll(r => r.PlayerID == toRemove.PlayerID);

                FillUpQuiz();
                DrawCharts();


                //for (int i = 0; i < cboxPlayers.Items.Count; i++)
                //{
                //    if (cboxPlayers.GetItemChecked(i))
                //    {
                //        var selected = (Player)cboxPlayers.Items[i];
                //        if (sPlayersList.Contains(selected))
                //        {
                //            //sPlayersList.RemoveAll(r => r.Equals(selected));

                //            //List<Player> toRemove = sPlayersList.Where(x => x.PlayerID == selected.PlayerID).ToList<Player>;
                //            var remove = from x in sPlayersList
                //                          where x.PlayerID == selected.PlayerID
                //                          select x;

                //            foreach (Player toRemove in remove)
                //            {
                //                sPlayersList.Remove(toRemove);
                //            }


                //            //var id = selected.PlayerID;
                //            //foreach (var item in sPlayersList)
                //            //{
                //            //    if (item.PlayerID == id)
                //            //    {
                //            //        sPlayersList.Remove(item);
                //            //    }
                //            //}
                //        }
                //    }
                //}

            }
        }

        private void cboxCategories_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                Category toAdd = (Category)cboxCategories.Items[e.Index];
                sCategoriesList.Add(toAdd);
                FillUpQuiz();
                DrawCharts();
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                Category toRemove = (Category)cboxCategories.Items[e.Index];
                sCategoriesList.RemoveAll(r => r.CategoryID == toRemove.CategoryID);
                FillUpQuiz();
                DrawCharts();
            }
        }

        private void FillUpQuiz()
        {
            QuizList.Clear();
            foreach (var player in sPlayersList)
            {
                foreach (var category in sCategoriesList)
                {
                    var playerid = player.PlayerID;
                    var categoryid = category.CategoryID;

                    var quizes = from x in context.Quizs
                                 where x.PlayerFK == playerid && x.Question.CategoryFK == categoryid
                                 select x;
                    foreach (var item in quizes)
                    {
                        QuizList.Add(item);
                    }
                }
            }
        }

        private Color GetColor()
        {
            Random r = new Random();
            Color randomColor = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
            return randomColor;
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.cboxPlayers = new System.Windows.Forms.CheckedListBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.cboxCategories = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // cboxPlayers
            // 
            this.cboxPlayers.FormattingEnabled = true;
            this.cboxPlayers.Location = new System.Drawing.Point(12, 56);
            this.cboxPlayers.Name = "cboxPlayers";
            this.cboxPlayers.Size = new System.Drawing.Size(120, 94);
            this.cboxPlayers.TabIndex = 0;
            this.cboxPlayers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cboxPlayers_ItemCheck);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 382);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(613, 220);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(12, 156);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(613, 220);
            this.chart2.TabIndex = 2;
            this.chart2.Text = "chart2";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(265, 88);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpFrom.TabIndex = 3;
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(265, 130);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 20);
            this.dtpTo.TabIndex = 4;
            // 
            // cboxCategories
            // 
            this.cboxCategories.FormattingEnabled = true;
            this.cboxCategories.Location = new System.Drawing.Point(139, 56);
            this.cboxCategories.Name = "cboxCategories";
            this.cboxCategories.Size = new System.Drawing.Size(120, 94);
            this.cboxCategories.TabIndex = 5;
            this.cboxCategories.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cboxCategories_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Játékosok";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(265, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Időszak";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(135, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Kategóriák";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(265, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Időszak vége";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(266, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "Időszak kezdete";
            // 
            // Statistics
            // 
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxCategories);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.cboxPlayers);
            this.Name = "Statistics";
            this.Size = new System.Drawing.Size(646, 619);
            this.Load += new System.EventHandler(this.Statistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }




    }
}
