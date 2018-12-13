﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Snake {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        //Snap: Change Border
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            /* Change Form Border
            if (comboBox1.Text == "Normal") {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            }
            else if (comboBox1.Text == "Borderless") {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }
            */
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)48 || e.KeyChar == (Char)49 ||
               e.KeyChar == (Char)50 || e.KeyChar == (Char)51 ||
               e.KeyChar == (Char)52 || e.KeyChar == (Char)53 ||
               e.KeyChar == (Char)54 || e.KeyChar == (Char)55 ||
               e.KeyChar == (Char)56 || e.KeyChar == (Char)57 ||
               e.KeyChar == (Char)13 || e.KeyChar == (Char)8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        //Donate Button
        private void button1_Click(object sender, EventArgs e) {
            var result = MessageBox.Show(
                "https://www.organicsoupkitchen.org/donate" + "\n台科大南部分校提醒您，不明連結可能包含惡意程式\n\n按「是」繼續前往", 
                "即將開啟連結", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes) {
                System.Diagnostics.Process.Start("https://www.organicsoupkitchen.org/donate");
            }
        }
    

        //Game Start Button
        private void button2_Click(object sender, EventArgs e) {

            /*
             * Source : https://dotblogs.com.tw/yc421206/archive/2010/08/10/17108.aspx
             */

            //Init XML
            XmlDocument doc = new XmlDocument();
            XmlElement Settings = doc.CreateElement("Settings");
            doc.AppendChild(Settings);

            /*
             * Create Child Node - Window Mode
             */
            XmlElement Window = doc.CreateElement("Window");
            //設定屬性
            if (comboBox1.Text == "Normal") {
                Window.SetAttribute("value", "normal");
            } else if (comboBox1.Text == "Borderless") {
                Window.SetAttribute("value", "borderLess");
            }
            //加入至 Window 節點底下
            Settings.AppendChild(Window);


            /*
             * Create Child Node - Backgroung Color Change or Not
             */
            XmlElement BackColor = doc.CreateElement("BackColor");
            //設定屬性
            if (comboBox2.Text == "Enable") {
                BackColor.SetAttribute("value", "enable");
            } else if (comboBox2.Text == "Disable") {
                BackColor.SetAttribute("value", "disable");
            }
            //加入至 BackColor 節點底下
            Settings.AppendChild(BackColor);


            /*
             * Create Child Node - Money
             */
            XmlElement Money = doc.CreateElement("Money");
            //設定屬性

            //Check Number
            int money = 0;

            try {
                if (string.IsNullOrEmpty(textBox1.Text)) {
                    Money.SetAttribute("value", "0");
                } else {
                    money = Convert.ToInt32(textBox1.Text);
                    Money.SetAttribute("value", textBox1.Text);
                }
            } catch {
                MessageBox.Show("請輸入正確的數字，謝謝惠顧。");
            }
            

            //加入至 BackColor 節點底下
            Settings.AppendChild(Money);

            //存檔
            doc.Save("settings.xml");

        }


        // Quit Button
        private void button3_Click(object sender, EventArgs e) { this.Dispose(); }



        //Cover Picture
        private void timer1_Tick(object sender, EventArgs e) {
            //Release Memory
            pictureBox1.Image.Dispose();
            pictureBox1.Image = Properties.Resources.Radeon_VR_Ready;
            this.Text = "越吃越肥 | Radeon VR Ready";
        }

        //Cover Picture
        private void timer2_Tick(object sender, EventArgs e) {
            //Release Memory
            pictureBox1.Image.Dispose();
            pictureBox1.Image = Properties.Resources.GeForce_RTX_01;
            this.Text = "越吃越肥 | Powered by Geforce RTX";
        }

        private void Form1_Load(object sender, EventArgs e) {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        
    }
}
