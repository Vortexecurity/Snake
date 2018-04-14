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

        public event EventHandler<CollisionEventArgs> OnCollide;
        private event EventHandler<FruitEatEventArgs> OnFruitEat;

        private Graphics _graphics;
        public Graphics Graphics {
            get => _graphics;
            private set => _graphics = value;
        }

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

        public SnakeLogic(Size siz) {
            FieldSizePixel = siz;
            _rand = new Random();
            Running = false;
            _timer = new Timer {
                Interval = 50,
                Enabled = false
            };

            for (int x = 0; x < _grid.GetLength(0); x++) {
                for (int y = 0; y < _grid.GetLength(1);) {
                    _grid[x, y] = ' ';
                    if (x == 13) {
                        x = 0;
                        y++;
                    }
                }
            }

            using (Pen p = new Pen(Color.Black)) {
                _graphics.DrawRectangle(p, 0, 0, FieldSizePixel.Width, FieldSizePixel.Height);
            }
            
            Position = new Point(_rand.Next(3, 7), _rand.Next(3, 7));
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
            _pos = new Point(_rand.Next(3, 7), _rand.Next(3, 7));
            Direction = (Direction)Enum.GetValues(typeof(Direction)).GetValue(_rand.Next(3));
        }
    }
}