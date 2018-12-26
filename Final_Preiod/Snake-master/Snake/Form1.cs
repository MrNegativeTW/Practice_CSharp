﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Snake
{
    public partial class SnakeForm : Form,IMessageFilter
    {
        SnakePlayer Player1;
        FoodManager FoodMngr;
        Random r = new Random();
        private int score = 0;
        public SnakeForm()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            this.FormClosed += (s, e) => Application.RemoveMessageFilter(this);
            Player1 = new SnakePlayer(this);
            FoodMngr = new FoodManager(GameCanvas.Width, GameCanvas.Height);
            FoodMngr.AddRandomFood(10);
            scoreText.Text = score.ToString();
        }

        

        public void ResetGame()
        {
            Player1 = new SnakePlayer(this);
            FoodMngr = new FoodManager(GameCanvas.Width, GameCanvas.Height);
            FoodMngr.AddRandomFood(10);
            score = 0;
        }

        public bool PreFilterMessage(ref Message msg)
        {
            if(msg.Msg == 0x0101) //KeyUp
                Input.SetKey((Keys)msg.WParam, false);
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(msg.Msg == 0x100) //KeyDown
                Input.SetKey((Keys)msg.WParam, true);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Player1.Draw(canvas);
            FoodMngr.Draw(canvas);
        }

        // 檢查是否吃到
        private void CheckForCollisions() {
            if (Player1.IsIntersectingRect(new Rectangle(-100, 0, 100, GameCanvas.Height)))
                Player1.OnHitWall(Direction.left);

            if (Player1.IsIntersectingRect(new Rectangle(0, -100, GameCanvas.Width, 100)))
                Player1.OnHitWall(Direction.up);

            if (Player1.IsIntersectingRect(new Rectangle(GameCanvas.Width, 0, 100, GameCanvas.Height)))
                Player1.OnHitWall(Direction.right);

            if (Player1.IsIntersectingRect(new Rectangle(0, GameCanvas.Height, GameCanvas.Width, 100)))
                Player1.OnHitWall(Direction.down);

            //吃到食物時
            List<Rectangle> SnakeRects = Player1.GetRects();
            foreach(Rectangle rect in SnakeRects)
            {
                if(FoodMngr.IsIntersectingRect(rect,true)) {
                    FoodMngr.AddRandomFood();
                    Player1.AddBodySegments(1);
                    score += 5;
                    scoreText.Text = score.ToString();
                }
            }
        }

        // 轉方向
        private void SetPlayerMovement() {
            if (Input.IsKeyDown(Keys.Left))
            {
                Player1.SetDirection(Direction.left);
            }
            else if (Input.IsKeyDown(Keys.Right))
            {
                Player1.SetDirection(Direction.right);
            }
            else if (Input.IsKeyDown(Keys.Up))
            {
                Player1.SetDirection(Direction.up);
            }
            else if (Input.IsKeyDown(Keys.Down))
            {
                Player1.SetDirection(Direction.down);
            }
            Player1.MovePlayer();
        }

        // 重複循環檢查動作
        private void GameTimer_Tick(object sender, EventArgs e) {
            SetPlayerMovement();
            CheckForCollisions();
            GameCanvas.Invalidate();
        }

        // 啟動暫停系統 - 按鈕
        private void Start_Btn_Click(object sender, EventArgs e) {
            ToggleTimer();
        }

        // 啟動暫停系統主機
        public void ToggleTimer()
        {
            GameTimer.Enabled = !GameTimer.Enabled;
        }

        private void SnakeForm_Load(object sender, EventArgs e) {

        }

        private void exit_btn_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}