using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media.Imaging;

using System.Windows.Threading;
using System.Threading;
using System.Windows;



namespace CreatureWorld
{
    class AACreature : Creature
    {
        Image hImage;
        BitmapImage righthhog;
        BitmapImage lefthhog;
        private Thread positionThread = null;
        private bool goR = true;
        double hhogWidth = 356;
        double incSize = 2.0;
        double kingWidth = 0;

        public AACreature(Canvas kingdom, Dispatcher dispatcher, Int32 waitTime = 100) : base(kingdom, dispatcher, waitTime)
        {
            hImage = new Image();
            righthhog = LoadBitmap(@"AACreature\hhogright.png", hhogWidth);
            lefthhog = LoadBitmap(@"AACreature\hhogleft.png", hhogWidth);
        }

        public override void Shutdown()
        {
            if (positionThread != null)
            {
                positionThread.Abort();
            }
        }

        public override void Place(double x, double y)
        {
            hImage.Source = righthhog;
            goR = true;

            this.x = x;
            this.y = y;
            kingdom.Children.Add(hImage);
            hImage.SetValue(Canvas.LeftProperty, this.x);
            hImage.SetValue(Canvas.TopProperty, this.y);

            positionThread = new Thread(currPosition);
            positionThread.Start();
        }

        void currPosition()
        {
            while (true)
            {
                if (goR && !Paused)
                {
                    x += incSize;
                    if (x > kingWidth)
                    {
                        goR = false;
                        switchBit(lefthhog);
                    }else if (!Paused)
                    {
                        x -= incSize;
                        if(x < 0)
                        {
                            goR = true;
                            switchBit(righthhog);
                        }
                    }
                }

                if (kingWidth != kingdom.RenderSize.Width - hhogWidth)
                {
                    kingWidth = kingdom.RenderSize.Width - hhogWidth;
                }
                updatePosition();
                Thread.Sleep(WaitTime);
            }
        }

        void updatePosition()
        {
            Action action = () =>
            {
                hImage.SetValue(Canvas.LeftProperty, x); 
                hImage.SetValue(Canvas.TopProperty, y);
            };
            dispatcher.BeginInvoke(action);
        }

        void switchBit(BitmapImage theBitmap)
        {
            Action action = () =>
            {
                hImage.Source = theBitmap;
            };
            dispatcher.BeginInvoke(action);
        }
    }
}
