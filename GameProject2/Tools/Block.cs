using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject2.Tools
{
    public enum BlockTypes
    {
        T,
        Z,
        Z_Inverse,
        L,
        L_Inverse,
        Straight,
        Cube
    }

    public enum Directions
    {
        North,
        East,
        South,
        West
    }
    public class Block
    {
        private BlockTypes _type;

        public Tuple<int, int>[] Cords;

        private Tuple<int, int> source => Cords[0];

        public Block(Block otherBlock)
        {
            _type = otherBlock._type;
            Cords = otherBlock.Cords;
        }


        public static bool IsBlockInBounds(Tuple<int, int>[] block, bool[,] grid)
        {
            foreach (Tuple<int, int> cord in block)
            {
                if (cord.Item1 < 0 || cord.Item1 > 23 || cord.Item2 < 0 || cord.Item2 > 9) return false;
                if (grid[cord.Item1, cord.Item2]) return false;
            }

            return true;
        }

        public static Tuple<int, int>[] CopyCords(Tuple<int, int>[] source, Tuple<int, int> offset)
        {
            Tuple<int, int>[] tempCords = new Tuple<int, int>[4];
            for (int i = 0; i < 4; i++)
            {
                tempCords[i] = new Tuple<int, int>(source[i].Item1 + offset.Item1, source[i].Item2 + offset.Item2);
            }
            return tempCords;
        }

        public static Tuple<int, int>[] CopyCords(Tuple<int, int>[] source)
        {
            return CopyCords(source, new Tuple<int, int>(0, 0));
        }

        public bool MoveBlock(bool[,] grid, Tuple<int, int> direction)
        {
            Tuple<int, int>[] tempCords;
            tempCords = CopyCords(Cords,direction);
            if (!IsBlockInBounds(tempCords,grid)) return false;
            Cords = tempCords;
            return true;
        }

        public bool MoveBlockDown(bool[,] grid)
        {
            return MoveBlock(grid,new Tuple<int,int>(1,0));
        }

        public bool MoveBlockLeft(bool[,] grid)
        {
            return MoveBlock(grid, new Tuple<int, int>(0, -1));
        }

        public bool MoveBlockRight(bool[,] grid)
        {
            return MoveBlock(grid, new Tuple<int, int>(0, 1));
        }

        public bool RotateBlock(bool[,] grid, bool isClockwise)
        {
            if (_type == BlockTypes.Cube) return true;
            Tuple<int, int>[] temp;
            int offsetDirection;
            if (source.Item2 < 5) offsetDirection = 1; else offsetDirection = -1;
            if (isClockwise)
            {
                for(int offset = 0; offset < 3; offset++)
                {
                    temp = CopyCords(Cords);
                    for (int i = 0; i < 4; i++)
                    {
                        int oldX = temp[i].Item1 - source.Item1;
                        int oldY = temp[i].Item2 - source.Item2;
                        temp[i] = new Tuple<int, int>(source.Item1 + oldY, source.Item2 - oldX + offset * offsetDirection);
                    }
                    if (IsBlockInBounds(temp, grid))
                    {
                        Cords = temp;
                        return true;
                    }
                }
            }
            else
            {
                for (int offset = 0; offset < 3; offset++)
                {
                    temp = CopyCords(Cords);
                    for (int i = 0; i < 4; i++)
                    {
                        int oldX = temp[i].Item1 - source.Item1;
                        int oldY = temp[i].Item2 - source.Item2;
                        temp[i] = new Tuple<int, int>(source.Item1 - oldY, source.Item2 + oldX + offset * offsetDirection);
                    }
                    if (IsBlockInBounds(temp, grid))
                    {
                        Cords = temp;
                        return true;
                    }
                }
            }
            return false;
        }

        public Block(bool[,] grid, BlockTypes type)
        {
            Cords = new Tuple<int, int>[4];
            _type = type;
            switch (type)
            {
                case BlockTypes.Cube:
                    if (!(grid[4, 3] || grid[4, 4])) // Top Row clear
                    {
                        if (!(grid[5, 3] || grid[5, 4])) //Bottom row clear
                        {
                            Cords[0] = new Tuple<int, int>(4, 3);
                            Cords[1] = new Tuple<int, int>(4, 4);
                            Cords[2] = new Tuple<int, int>(5, 3);
                            Cords[3] = new Tuple<int, int>(5, 4);
                        }
                        else
                        {
                            Cords[0] = new Tuple<int, int>(3, 3);
                            Cords[1] = new Tuple<int, int>(3, 4);
                            Cords[2] = new Tuple<int, int>(4, 3);
                            Cords[3] = new Tuple<int, int>(4, 4);
                        }
                    }
                    else
                    {
                        Cords[0] = new Tuple<int, int>(2, 3);
                        Cords[1] = new Tuple<int, int>(2, 4);
                        Cords[2] = new Tuple<int, int>(3, 3);
                        Cords[3] = new Tuple<int, int>(3, 4);
                    }
                    break;
                case BlockTypes.Z:
                    if (!(grid[4, 3] || grid[4, 4] || grid[5, 4] || grid[5, 5])) // Full clear
                    {
                        Cords[0] = new Tuple<int, int>(4, 3);
                        Cords[1] = new Tuple<int, int>(4, 4);
                        Cords[2] = new Tuple<int, int>(5, 4);
                        Cords[3] = new Tuple<int, int>(5, 5);
                    }
                    else if (!(grid[4, 4] || grid[4, 5])) // Half Clear
                    {
                        Cords[0] = new Tuple<int, int>(3, 3);
                        Cords[1] = new Tuple<int, int>(3, 4);
                        Cords[2] = new Tuple<int, int>(4, 4);
                        Cords[3] = new Tuple<int, int>(4, 5);
                    }
                    else
                    {
                        Cords[0] = new Tuple<int, int>(2, 3);
                        Cords[1] = new Tuple<int, int>(2, 4);
                        Cords[2] = new Tuple<int, int>(3, 4);
                        Cords[3] = new Tuple<int, int>(3, 5);
                    }
                    break;
                case BlockTypes.Z_Inverse:
                    if (!(grid[4, 4] || grid[4, 5] || grid[5, 3] || grid[5, 4])) // Full clear
                    {
                        Cords[0] = new Tuple<int, int>(4, 4);
                        Cords[1] = new Tuple<int, int>(4, 5);
                        Cords[2] = new Tuple<int, int>(5, 3);
                        Cords[3] = new Tuple<int, int>(5, 4);
                    }
                    else if (!(grid[4, 3] || grid[4, 4])) //Half Clear
                    {
                        Cords[0] = new Tuple<int, int>(3, 4);
                        Cords[1] = new Tuple<int, int>(3, 5);
                        Cords[2] = new Tuple<int, int>(4, 3);
                        Cords[3] = new Tuple<int, int>(4, 4);
                    }
                    else
                    {
                        Cords[0] = new Tuple<int, int>(2, 4);
                        Cords[1] = new Tuple<int, int>(2, 5);
                        Cords[2] = new Tuple<int, int>(3, 3);
                        Cords[3] = new Tuple<int, int>(3, 4);
                    }
                    break;
                case BlockTypes.L:
                    if (!(grid[5, 3] || grid[5, 4] || grid[5, 5] || grid[4, 5])) // Full clear
                    {
                        Cords[0] = new Tuple<int, int>(5, 3);
                        Cords[1] = new Tuple<int, int>(5, 4);
                        Cords[2] = new Tuple<int, int>(5, 5);
                        Cords[3] = new Tuple<int, int>(4, 5);
                    }
                    else if (!(grid[4, 3] || grid[4, 4] || grid[4, 5]))// Half Clear
                    {
                        Cords[0] = new Tuple<int, int>(4, 3);
                        Cords[1] = new Tuple<int, int>(4, 4);
                        Cords[2] = new Tuple<int, int>(4, 5);
                        Cords[3] = new Tuple<int, int>(3, 5);
                    }
                    else //No Clear
                    {
                        Cords[0] = new Tuple<int, int>(3, 3);
                        Cords[1] = new Tuple<int, int>(3, 4);
                        Cords[2] = new Tuple<int, int>(3, 5);
                        Cords[3] = new Tuple<int, int>(2, 5);
                    }
                    break;
                case BlockTypes.L_Inverse:
                    if (!(grid[5, 3] || grid[5, 4] || grid[5, 5] || grid[4, 3])) // Full clear
                    {
                        Cords[0] = new Tuple<int, int>(5, 3);
                        Cords[1] = new Tuple<int, int>(5, 4);
                        Cords[2] = new Tuple<int, int>(5, 5);
                        Cords[3] = new Tuple<int, int>(4, 3);
                    }
                    else if (!(grid[4, 3] || grid[4, 4] || grid[4, 5]))// Half Clear
                    {
                        Cords[0] = new Tuple<int, int>(4, 3);
                        Cords[1] = new Tuple<int, int>(4, 4);
                        Cords[2] = new Tuple<int, int>(4, 5);
                        Cords[3] = new Tuple<int, int>(3, 3);
                    }
                    else //No Clear
                    {
                        Cords[0] = new Tuple<int, int>(3, 3);
                        Cords[1] = new Tuple<int, int>(3, 4);
                        Cords[2] = new Tuple<int, int>(3, 5);
                        Cords[3] = new Tuple<int, int>(2, 3);
                    }
                    break;
                case BlockTypes.Straight:
                    if (!(grid[4, 3] || grid[4, 4] || grid[4, 5] || grid[4, 6]))// Clear
                    {
                        Cords[0] = new Tuple<int, int>(4, 3);
                        Cords[1] = new Tuple<int, int>(4, 4);
                        Cords[2] = new Tuple<int, int>(4, 5);
                        Cords[3] = new Tuple<int, int>(4, 6);
                    }
                    else // No Clear
                    {
                        Cords[0] = new Tuple<int, int>(3, 3);
                        Cords[1] = new Tuple<int, int>(3, 4);
                        Cords[2] = new Tuple<int, int>(3, 5);
                        Cords[3] = new Tuple<int, int>(3, 6);
                    }
                    break;
                case BlockTypes.T:
                    if (!(grid[5, 3] || grid[5, 4] || grid[5, 5] || grid[4, 4]))
                    {
                        Cords[0] = new Tuple<int, int>(5, 4);
                        Cords[1] = new Tuple<int, int>(5, 3);
                        Cords[2] = new Tuple<int, int>(5, 5);
                        Cords[3] = new Tuple<int, int>(4, 4);
                    }
                    else if (!(grid[4, 3] || grid[4, 4] || grid[4, 5]))
                    {
                        Cords[0] = new Tuple<int, int>(4, 4);
                        Cords[1] = new Tuple<int, int>(4, 3);
                        Cords[2] = new Tuple<int, int>(4, 5);
                        Cords[3] = new Tuple<int, int>(3, 4);
                    }
                    else
                    {
                        Cords[0] = new Tuple<int, int>(3, 4);
                        Cords[1] = new Tuple<int, int>(3, 3);
                        Cords[2] = new Tuple<int, int>(3, 5);
                        Cords[3] = new Tuple<int, int>(2, 4);
                    }
                    break;
                default:
                    throw new Exception("Block Not Recognized");
                    break;
            }
        }
    }
}
