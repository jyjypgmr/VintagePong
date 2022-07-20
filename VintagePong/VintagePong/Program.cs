/*******************************************
* 1970's vintage Pong game
* By Jamie Youngjae Yoo
* Description : 
* 1970's vintage Pong game
* ******************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GDIDrawer;

namespace VintagePong
{
    class Program
    {
        static void Main(string[] args)
        {
            // graphics interface
            CDrawer cdCanvas;   
            cdCanvas = new CDrawer();
            // set scale of graphic interface
            // default size is 800 x 600
            // after scaled by 5 : 160 x 120
            cdCanvas.Scale = 5;
            // turn off the automatic update based on a timer
            cdCanvas.ContinuousUpdate = false;

            // point for current mouse point, location of again button and quit button
            Point ptMousePoint, ptAgainBtn, ptQuitBtn;
            // boolean to start the game
            bool bStart = false;

            do
            {
                // ball's x and y location
                int iBall_X = 2, iBall_Y;
                // ball's velocity
                int iBall_Xvel, iBall_Yvel;
                // game score and game speed
                int iScore = 0, iSpeed;
                // walls thickness
                int iWallThickness = 2;
                // paddle's length and thickness
                int iPaddleLength = 20, iPaddleThickness = 10;

                DrawWalls(cdCanvas, iWallThickness);

                // set ball's y location as random when the game starts
                Random rdRand = new Random();
                iBall_Y = rdRand.Next(iWallThickness + 1, cdCanvas.ScaledHeight - iWallThickness);

                // keep playing as long as the ball does not go out to the left side of the drawer
                while (iBall_X >= 0)
                {
                    if (cdCanvas.GetLastMouseLeftClick(out ptMousePoint))
                    {
                        cdCanvas.AddRectangle(iBall_X, iBall_Y, 2, 2);
                    }

                    // erase the old graphics(ball, paddle)
                    cdCanvas.Clear();

                    // get current mouse position
                    cdCanvas.GetLastMousePositionScaled(out ptMousePoint);

                    DrawPaddle(ptMousePoint, Color.Red, iPaddleLength, iWallThickness, iPaddleThickness, cdCanvas);

                    // draw score board
                    cdCanvas.AddText($"{iScore}", 30);

                    cdCanvas.Render();
                }

            } while (true);
        }

        private static void DrawPaddle(Point ptMousePoint, Color col, int iPaddleLength, int iWallThickness, int iPaddleThickness, CDrawer cdCanvas)
        {
            // set paddle's y boundary
            ptMousePoint.Y = (ptMousePoint.Y < iPaddleLength/2 + iWallThickness + 1) ? iPaddleLength/2 + iWallThickness + 1: ptMousePoint.Y;
            ptMousePoint.Y = (ptMousePoint.Y > cdCanvas.ScaledHeight - 1 - iPaddleLength/2 - iWallThickness) ? cdCanvas.ScaledHeight - 1 - iPaddleLength/2 - iWallThickness : ptMousePoint.Y;

            // set paddle's starting position based on current mouse positoin
            int iPaddle_X = 1, iPaddle_Y = ptMousePoint.Y - iPaddleLength/2;

            // draw paddle
            cdCanvas.AddLine(iPaddle_X, iPaddle_Y, iPaddle_X, iPaddle_Y + iPaddleLength, col, iPaddleThickness);

            cdCanvas.Render();
        }

        #region DrawWalls
        /// <summary>
        /// draw top, right and bottom side walls
        /// </summary>
        /// <param name="canvas"></param>
        private static void DrawWalls(CDrawer canvas, int iWallThickness)
        {
            // draw top wall
            for (int x = 0; x < canvas.ScaledWidth; x++)
            {
                for (int y = 0; y < iWallThickness; y++)
                {
                    canvas.SetBBScaledPixel(x, y, Color.Gray);
                }
            }

            // draw right wall
            for (int y = 0; y < canvas.ScaledHeight; y++)
            {
                for (int x = canvas.ScaledWidth-1; x > canvas.ScaledWidth - 1 - iWallThickness; x--)
                {
                    canvas.SetBBScaledPixel(x, y, Color.Gray);
                }
            }

            // draw bottom wall
            for (int x = 0; x < canvas.ScaledWidth; x++)
            {
                for (int y = canvas.ScaledHeight - 1; y > canvas.ScaledHeight - 1 - iWallThickness; y--)
                {
                    canvas.SetBBScaledPixel(x, y, Color.Gray);
                }
            }

            canvas.Render();
        }
        #endregion
    }
}
