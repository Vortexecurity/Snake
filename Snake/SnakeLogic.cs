using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake {
    public enum Direction {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public class CollisionEventArgs : EventArgs {
        public int X { get; set; }
        public int Y { get; set; }

        public CollisionEventArgs(int x, int y) {
            X = x;
            Y = y;
        }
    }

    public class FruitEatEventArgs : EventArgs {
        public int X { get; set; }
        public int Y { get; set; }

        public FruitEatEventArgs(int x, int y) {
            X = x;
            Y = y;
        }
    }

    public class SnakeLogic {
        private char[,] _grid = new char[14,14];
        public char[,] Grid {
            get => _grid;
            private set => _grid = value;
        }

        private Point _pos;
        public Point Position {
            get => _pos;
            private set => _pos = value;
        }
        private readonly Random _rand;

        public Direction Direction { get; private set; }

        public Size FieldSizePixel { get; private set; }

        private event EventHandler<CollisionEventArgs> OnCollide;
        private event EventHandler<FruitEatEventArgs> OnFruitEat;
        public event EventHandler<PaintEventArgs> OnDesirePaint;

        private Graphics _graphics;
        public Graphics Graphics {
            get => _graphics;
            private set => _graphics = value;
        }

        /*private Rectangle _drawRect;
        public Rectangle DrawingRectangle {
            get => _drawRect;
            private set => _drawRect = value;
        }*/

        public bool Running { get; private set; }

        private Timer _timer;
        public Timer Timer {
            get => _timer;
            private set => _timer = value;
        }

        private int _appendixLength;
        public int AppendixLength {
            get => _appendixLength;
            private set => _appendixLength = value;
        }

        private SnakePanel _panel;
        public SnakePanel SnakePanel {
            get => _panel;
            private set => _panel = value;
        }

        public SnakeLogic(Size size) {
            FieldSizePixel = size;
            _rand = new Random();
            Running = false;
            _timer = new Timer {
                Interval = 50,
                Enabled = false
            };

            for (int x = 0; x < _grid.GetLength(0); x++) {
                for (int y = 0; y < _grid.GetLength(1); y++) {
                    _grid[x, y] = ' ';
                }
            }

            using (var imgBuf = new Bitmap(size.Width, size.Height)) {
                using (var graphics = Graphics.FromImage(imgBuf)) {
                    _graphics = graphics;
                }
             }

            /*using (Pen p = new Pen(Color.Black)) {
                _graphics.DrawRectangle(p, 0, 0, FieldSizePixel.Width, FieldSizePixel.Height);
            }*/
            
            // Random Startpunkt
            Position = new Point(_rand.Next(3, 7), _rand.Next(3, 7));

            // Random Richtung
            Direction = (Direction)Enum.GetValues(typeof(Direction)).GetValue(_rand.Next(3));

            this.OnCollide += (s, e) => {

            };

            this.OnFruitEat += (s, e) => {

            };

        }

        protected virtual void OnCollision(CollisionEventArgs e) {
            EventHandler<CollisionEventArgs> handler = OnCollide;
            if (handler != null) {
                handler(this, e);
            }
        }

        protected virtual void OnFruitEaten(FruitEatEventArgs e) {
            EventHandler<FruitEatEventArgs> handler = OnFruitEat;
            if (handler != null) {
                handler(this, e);
            }
        }

        protected virtual void OnMustPaint(PaintEventArgs e) {
            EventHandler<PaintEventArgs> handler = OnDesirePaint;
            if (handler != null) {
                handler(this, e);
            }
        }

        private void Respawn() {
            Running = false;
            _timer.Enabled = false;
            Position = new Point(_rand.Next(3, 7), _rand.Next(3, 7));
            Direction = (Direction)Enum.GetValues(typeof(Direction)).GetValue(_rand.Next(3));
        }

        public void Start() {
            Running = true;
            _timer.Enabled = true;
        }

        public void Stop() {
            Running = false;
            _timer.Enabled = false;
        }

        private void timer_Tick(object sender, EventArgs e) {
            OnMustPaint(new PaintEventArgs(_graphics, Rectangle.Empty));
        }

        public void Left() {
            Direction = Direction.LEFT;
            if (_pos.X == 0) {
                OnCollision(new CollisionEventArgs(_pos.X, _pos.Y));
                Died();
            } else {
                _pos.X--;
                _grid[_pos.X, _pos.Y] = 'S';
            }
        }

        public void Right() {
            Direction = Direction.RIGHT;
            if (_pos.X == 13) {
                OnCollision(new CollisionEventArgs(_pos.X, _pos.Y));
                Died();
            } else {
                _pos.X++;
                _grid[_pos.X, _pos.Y] = 'S';
            }
        }

        public void Up() {
            Direction = Direction.UP;
            if (_pos.Y == 0) {
                OnCollision(new CollisionEventArgs(_pos.X, _pos.Y));
                Died();
            } else {
                _pos.Y--;
                _grid[_pos.X, _pos.Y] = 'S';
            }
        }

        public void Down() {
            Direction = Direction.DOWN;
            if (_pos.Y == 13) {
                OnCollision(new CollisionEventArgs(_pos.X, _pos.Y));
                Died();
            } else {
                _pos.Y++;
                _grid[_pos.X, _pos.Y] = 'S';
            }
        }

        private void Died() {
            /*Running = false;
            _timer.Enabled = false;
            _pos = new Point(_rand.Next(3, 7), _rand.Next(3, 7));
            Direction = (Direction)Enum.GetValues(typeof(Direction)).GetValue(_rand.Next(3));*/
            Respawn();
        }
    }
}
