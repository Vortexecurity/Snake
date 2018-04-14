using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake {
    public class SnakePanel : Panel {
        private SnakeLogic _snakeLogic;
        // Amount of squares
        private int squareSize = 14;

        public SnakePanel(SnakeLogic logic) : base() {
            _snakeLogic = logic;

            (this as Control).KeyDown += OnKeyDown;
        }

        protected override void OnPaint(PaintEventArgs e) {
            using (Pen p = new Pen(Color.Black)) {
                for (int x = 0; x < _snakeLogic.Grid.GetLength(0) * (Width / x); x += this.Width / squareSize) {
                    for (int y = 0; y < _snakeLogic.Grid.GetLength(1) * (Height / y); y += this.Height / squareSize) {
                        char curr = _snakeLogic.Grid[x / squareSize, y / squareSize];
                        if (curr == ' ') {
                            p.Color = Color.Black;
                            e.Graphics.DrawRectangle(p, x, y, this.Width / squareSize, this.Height / squareSize);
                        } else if (curr == 'S') {
                            p.Color = Color.Orange;
                            e.Graphics.DrawRectangle(p, x, y, this.Width / squareSize, this.Height / squareSize);
                        } else if (curr == 'X') {
                            p.Color = Color.Green;
                            e.Graphics.DrawRectangle(p, x, y, this.Width / squareSize, this.Height / squareSize);
                        }
                    }
                }
            }
            //base.OnPaint(e);
            //using (Pen p = new Pen(Color.Black)) {
            //    for (int i = 0; i < 14; i++) {
            //        for (int x = 0, y = 0; x < this.Width && y < this.Height; x += this.Width / 14, y += this.Height / 14) {
            //            e.Graphics.DrawRectangle(p, new Rectangle(x, y, this.Width / 14, this.Height / 14));
            //        }
            //    }
            //}
        }

        protected virtual void OnKeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Up:
                    _snakeLogic.Up();
                    break;
                case Keys.Down:
                    _snakeLogic.Down();
                    break;
                case Keys.Left:
                    _snakeLogic.Left();
                    break;
                case Keys.Right:
                    _snakeLogic.Right();
                    break;
                case Keys.Enter:
                    if (!_snakeLogic.Running) {
                        _snakeLogic.Start();
                    }
                    break;
            }
        }
    }
}
