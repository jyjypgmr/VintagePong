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
using System.Threading;

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
            Point ptMousePoint = new Point(-1, -1), ptAgainBtn, ptQuitBtn;
            // boolean to start the game
            bool bStart = false;

            do
            {
                // ball's x and y location
                int iBall_X = 2, iBall_Y = 0;
                // ball's velocities
                int iBall_Xvel = 1, iBall_Yvel = 1, iBall_Size = 2;
                // game score and game speed
                int iScore = 0, iSpeed = 20;
                // walls thickness
                int iWallThickness = 2;
                // paddle's length and thickness
                int iPaddleLength = 20, iPaddleThickness = 10;
                // paddle's x and y location
                int iPaddle_X = 1, iPaddle_Y = 0;

                DrawWalls(cdCanvas, iWallThickness);

                // set ball's y location as random when the game starts
                Random rdRand = new Random();
                iBall_Y = rdRand.Next(iWallThickness + 1, cdCanvas.ScaledHeight - iWallThickness);

                // keep playing as long as the ball does not go out to the left side of the drawer
                while (iBall_X >= 0)
                {
                    // erase the old graphics(ball, paddle)
                    cdCanvas.Clear();

                    //get mouse position
                    cdCanvas.GetLastMousePositionScaled(out ptMousePoint);

                    DrawPaddle(ptMousePoint, Color.Red, ref iPaddle_X, ref iPaddle_Y, iPaddleLength, iWallThickness, iPaddleThickness, cdCanvas);

                    // draw score board
                    cdCanvas.AddText($"{iScore}", 30);

                    // set boolean for starting game to true if mouse left button is clicked
                    if (cdCanvas.GetLastMouseLeftClickScaled(out ptMousePoint))
                        bStart = true;

                    // draw and move ball if condition is true
                    if (bStart)
                    {
                        // draw ball
                        cdCanvas.AddRectangle(iBall_X, iBall_Y, iBall_Size, iBall_Size);

                        MoveBall(ref iBall_X, ref iBall_Y, ref iBall_Xvel, ref iBall_Yvel, ref iSpeed, 
                            cdCanvas, iWallThickness, iPaddle_X, iPaddle_Y, iPaddleLength, ref iScore);

                        Thread.Sleep(iSpeed);
                    }
                }

                cdCanvas.Render();
            } while (false);
        }

        private static void MoveBall
            (ref int iBall_X, ref int iBall_Y, ref int iBall_Xvel, ref int iBall_Yvel, ref int iSpeed, 
            CDrawer cdCanvas, int iWallThickness, int iPaddle_X, int iPaddle_Y, int iPaddleLength, ref int iScore)
        {
            // move ball's position by velocities
            iBall_X += iBall_Xvel;
            iBall_Y += iBall_Yvel;

            // reverse y direction of the ball if ball hit the bottom or top walls
            if (iBall_Y > cdCanvas.ScaledHeight - iWallThickness - 1 || iBall_Y < iWallThickness + 1)
                iBall_Yvel *= -1;
            // reverse x direction of the ball if ball hit the right wall
            if (iBall_X > cdCanvas.ScaledWidth - 1 - iWallThickness)
                iBall_Xvel *= -1;
            // reverse x direction if ball hit the paddle
            if (iBall_X == iPaddle_X && (iBall_Y >= iPaddle_Y && iBall_Y <= iPaddle_Y + iPaddleLength))
            {
                iBall_Xvel *= -1;

                // increase score when the ball hit the paddle
                iScore++;

                // add sound
                Console.Beep();

                // increase the ball's movement by decreasing the sleep duration
                if (iSpeed > 2)
                    iSpeed -= 1;
            }

        }

        private static void DrawPaddle(Point ptMousePoint, Color col,ref int iPaddle_X, ref int iPaddle_Y, int iPaddleLength, int iWallThickness, int iPaddleThickness, CDrawer cdCanvas)
        {
            // set paddle's y boundary
            ptMousePoint.Y = (ptMousePoint.Y < iPaddleLength/2 + iWallThickness + 1) ? iPaddleLength/2 + iWallThickness + 1: ptMousePoint.Y;
            ptMousePoint.Y = (ptMousePoint.Y > cdCanvas.ScaledHeight - 1 - iPaddleLength/2 - iWallThickness) ? cdCanvas.ScaledHeight - 1 - iPaddleLength/2 - iWallThickness : ptMousePoint.Y;

            // set paddle's starting position based on current mouse positoin
            iPaddle_Y = ptMousePoint.Y - iPaddleLength/2;

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
        }
        #endregion
    }
}
