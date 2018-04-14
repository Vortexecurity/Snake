using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake {
    public partial class Form1 : Form {
        private SnakePanel snakePanel;
        private SnakeLogic logic;

        public Form1() {
            InitializeComponent();

            // Größe auf die Client-Größe setzen
            logic = new SnakeLogic(this.ClientSize);
            logic.OnDesirePaint += (s, e) => {
                snakePanel.Invalidate();
            };

            snakePanel = new SnakePanel(logic) {
                // Panel ist so groß wie die Client-Größe (dockt an)
                Dock = DockStyle.Fill
            };

            Controls.Add(snakePanel);
        }
    }
}
