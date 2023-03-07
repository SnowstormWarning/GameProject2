using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameProject2.Tools;
using System.Windows.Forms;
using GameProject2.Sprites;
using SharpDX.Direct2D1;
using System.Net;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Media;

namespace GameProject2.StateManagement
{
    public class StandardGame: State
    {
        private MainMenu _mainMenu;
        private StateCommands _command = StateCommands.None;
        private bool[,] _grid = new bool[24,10];
        private Block _currentBlock;
        private BlockTypes _nextBlock;
        private Random _random;
        private StaticSprite[,] _blockSprites = new StaticSprite[24,10];
        private StaticSprite[] _miniSprites = new StaticSprite[7];
        private Vector2 _gameBoardStartPos;
        private float _blockSpacing;
        private Vector2 _miniBlockPos;
        private Vector2 _scorePos;
        private float _miniBlockFactor = 6;
        private float _blockScale = 0.019f;
        private int _timer;
        private int _timeStep => 2000/_speed;
        private int _speed = 1;
        private float _inputBufferDown;
        private float _inputBufferLeft;
        private float _inputBufferRight;
        private float _inputBufferRotateCW;
        private float _inputBufferRotateCCW;
        private float _inputBufferSpace;
        private float _inputBufferLength = 100f;
        private SoundEffect _blockDrop;
        private SoundEffect _lineClear;
        private Song _downfallMusic;
        public int Score;

        public StandardGame(MainMenu mainMenu)
        {
            this._mainMenu = mainMenu;
        }

        public override void Initialize()
        {
            _random = new Random();
            _nextBlock = (BlockTypes)_random.Next(0, 7);
            _gameBoardStartPos = PosTool.RelativeVector(0.3f, 0.15f);
            _miniBlockPos = PosTool.RelativeVector(0.6f, 0.2f);
            _scorePos = PosTool.RelativeVector(0.6f, 0.1f);
            _blockSpacing = 2560 * _blockScale * Game1.GlobalScalingFactor.X;
            for(int i = 0; i < 240; i++)
            {
                _blockSprites[i/10,i%10] = new StaticSprite("Block", _gameBoardStartPos + new Vector2(((float)(i%10)) * _blockSpacing, ((float)(i / 10)) * _blockSpacing), _blockScale);
                Console.WriteLine("" + i / 10 + " , " + i % 10);
            }
            _miniSprites[(int)BlockTypes.L] = new StaticSprite("MiniL", _miniBlockPos, _miniBlockFactor);
            _miniSprites[(int)BlockTypes.L_Inverse] = new StaticSprite("MiniLInv", _miniBlockPos, _miniBlockFactor);
            _miniSprites[(int)BlockTypes.T] = new StaticSprite("MiniT", _miniBlockPos, _miniBlockFactor);
            _miniSprites[(int)BlockTypes.Cube] = new StaticSprite("MiniCube", _miniBlockPos, _miniBlockFactor);
            _miniSprites[(int)BlockTypes.Straight] = new StaticSprite("MiniStraight", _miniBlockPos, _miniBlockFactor);
            _miniSprites[(int)BlockTypes.Z] = new StaticSprite("MiniZ", _miniBlockPos, _miniBlockFactor);
            _miniSprites[(int)BlockTypes.Z_Inverse] = new StaticSprite("MiniZInv", _miniBlockPos, _miniBlockFactor);
            GetNextBlock();
        }
        public void GetNextBlock()
        {
            _currentBlock = new Block(_grid, _nextBlock);
            _nextBlock = (BlockTypes)_random.Next(0, 7);
        }
        public override void LoadContent(ContentManager content)
        {
            foreach (StaticSprite sprite in _blockSprites)
            {
                sprite.LoadContent(content);
            }
            foreach(StaticSprite sprite in _miniSprites)
            {
                sprite.LoadContent(content);
            }
            _blockDrop = content.Load<SoundEffect>("BlockDrop");
            _lineClear = content.Load<SoundEffect>("LineClear");
            _downfallMusic = content.Load<Song>("DownfallMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_downfallMusic);
        }
        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keys)
        {
            Tuple<int, int>[] startCords = _currentBlock.Cords;
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            _inputBufferDown = UpdateBuffer(_inputBufferDown, gameTime);
            _inputBufferLeft = UpdateBuffer(_inputBufferLeft, gameTime);
            _inputBufferRight = UpdateBuffer(_inputBufferRight, gameTime);
            _inputBufferRotateCW = UpdateBuffer(_inputBufferRotateCW, gameTime);
            _inputBufferRotateCCW = UpdateBuffer(_inputBufferRotateCCW, gameTime);
            _inputBufferSpace = UpdateBuffer(_inputBufferSpace, gameTime);
            if (_timer >= _timeStep)
            {
                _timer -= _timeStep;
                if (!_currentBlock.MoveBlockDown(_grid))
                {
                    SettleBlock();
                }
            }
            if((keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) || keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down)) && _inputBufferDown <= 0)
            {
                _inputBufferDown = _inputBufferLength;
                _currentBlock.MoveBlockDown(_grid);
            }
            if ((keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) || keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left)) && _inputBufferLeft <= 0)
            {
                _inputBufferLeft = _inputBufferLength;
                _currentBlock.MoveBlockLeft(_grid);
            }
            if ((keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) || keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right)) && _inputBufferRight <= 0)
            {
                _inputBufferRight = _inputBufferLength;
                _currentBlock.MoveBlockRight(_grid);
            }
            if ((keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q) || keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W)) && _inputBufferRotateCCW <= 0)
            {
                _inputBufferRotateCCW = _inputBufferLength*2;
                _currentBlock.RotateBlock(_grid,false);
            }
            if (keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E) && _inputBufferRotateCW <= 0)
            {
                _inputBufferRotateCW = _inputBufferLength*2;
                _currentBlock.RotateBlock(_grid,true);
            }
            if(keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && _inputBufferSpace <= 0)
            {
                Console.WriteLine("" + _inputBufferSpace);
                _inputBufferSpace = 2*_inputBufferLength;
                while (_currentBlock.MoveBlockDown(_grid));
                SettleBlock();
            }
            if (CheckIfFailState())
            {
                _command = StateCommands.RemoveSource;
                MediaPlayer.Stop();
            }
        }

        public int CheckLines()
        {
            List<int> rowsToClear = new List<int>();
            List<int> rowsTested = new List<int>();
            foreach(Tuple<int,int> cord in _currentBlock.Cords)
            {
                if(rowsTested.IndexOf(cord.Item1) < 0)
                {
                    bool full = true;
                    rowsTested.Add(cord.Item1);
                    for (int i = 0; i < 10; i++)
                    {//
                        if (!_grid[cord.Item1, i]) full = false;
                    }
                    if(full) rowsToClear.Add(cord.Item1);
                }
            }
            if (rowsToClear.Count > 0)
            {
                foreach (int i in rowsToClear)
                {
                    for(int j = i-1; j > 2; j--)
                    { 
                        for(int k = 0; k<10; k++)
                        {
                            _grid[j + 1, k] = _grid[j, k];
                        }
                    }
                }
                Score+=rowsToClear.Count;
            }
            return rowsToClear.Count;
        }

        public bool CheckIfFailState()
        {
            for(int i = 0; i < 10; i++)
            {
                if (_grid[0, i] || _grid[1, i] || _grid[2, i] || _grid[3, i]) return true;
            }
            return false;
        }

        public void SettleBlock()
        {
            _blockDrop.Play();
            foreach (Tuple<int, int> cord in _currentBlock.Cords)
            {
                _grid[cord.Item1, cord.Item2] = true;
            }
            if (CheckLines() > 0)
            {
                //Noise Will Be Player Here
                _lineClear.Play();
            }
            GetNextBlock();
        }

        public float UpdateBuffer(float buffer, GameTime gameTime)
        {
            if (buffer > 0)
            {
                buffer -= gameTime.ElapsedGameTime.Milliseconds;
                if (buffer < 0) buffer = 0;
            }
            return buffer;
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, SpriteFont font)
        {
            for(int i = 0; i < 240; i++)
            {
                if (_grid[i / 10, i % 10])
                {
                    _blockSprites[i / 10, i % 10].Draw(gameTime, spriteBatch);
                } 
                else if(i/10 > 3)
                {
                    _blockSprites[i / 10, i % 10].Draw(gameTime, spriteBatch, Color.DarkSlateGray);
                }
            }
            Block temp = new Block(_currentBlock);
            while (temp.MoveBlockDown(_grid));
            foreach (Tuple<int, int> cord in temp.Cords)
            {
                _blockSprites[(int)cord.Item1, (int)cord.Item2].Draw(gameTime, spriteBatch, Color.SlateGray);
            }
            foreach (Tuple<int, int> cord in _currentBlock.Cords)
            {
                _blockSprites[(int)cord.Item1, (int)cord.Item2].Draw(gameTime, spriteBatch, Color.Blue);
            }
            _miniSprites[(int)_nextBlock].Draw(gameTime, spriteBatch, Color.White);
            spriteBatch.DrawString(font, ""+Score, _scorePos, Color.White);
        }

        public override StateCommands GetStateCommand()
        {
            return _command;
        }

        public override State GetTargetState()
        {
            return _mainMenu;
        }
    }
}
