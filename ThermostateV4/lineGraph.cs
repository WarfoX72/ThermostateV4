using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ThermostateV4
{
    class lineGraph
    {
        public class colorObject
        {
            private int id;
            private Color colorItem;
            public colorObject(int id, Color color)
            {
                this.id = id;
                this.colorItem = color;
            }
            public Color ColorItem
            {
                get { return colorItem; }
                set { colorItem = value; }
            }
            public int ID
            {
                get { return id; }
                set { id = value; }
            }
        }

        public class lineItems
        {
            private int id;
            private List<double> items = new List<double>();

            public lineItems(int id)
            {
                this.id = id;
            }
            public int ID
            {
                get { return id; }
                set { id = value; }
            }
            public List<double> ITEMS{
                get { return items; }
                set { items = value;}
            }
        }
        private System.Windows.Forms.PictureBox currentPicture;
        private Bitmap tempImageBuffer;
        private Graphics tempGraphics;
        private Color backgroundColor = Color.Azure;
        private Color axisColor = Color.FromArgb(0x50, 0x50, 0x50);
        private int minutes = -1;
        private int hour = -1;

        public int Width;
        public int Height;
        public int margin = 4;
        //
        public double min = 0;
        public double max = 5;
        

        public List<colorObject> colorList = new List<colorObject>();
        public List<lineItems> linesItems = new List<lineItems>();

        /**
         * 
         */
        public lineGraph(System.Windows.Forms.PictureBox picture)
        {
            currentPicture = picture;
            tempImageBuffer = new Bitmap(picture.Width, picture.Height, PixelFormat.Format24bppRgb);
            tempGraphics = Graphics.FromImage(tempImageBuffer);
            Width = picture.Width-(margin*2);
            Height = picture.Height-(margin*2);
        }

        public void copyImage(object sende, PaintEventArgs e)
        {
            Graphics xGraph = e.Graphics;
            xGraph.DrawImageUnscaled(tempImageBuffer, 0, 0, tempImageBuffer.Width, tempImageBuffer.Height);
        }

        private void updateStartTime()
        {
            minutes -= 1;
            if (minutes < 0)
            {
                minutes = 59;
                hour +=1;
                if (hour > 23)
                {
                    hour = 0;
                }
            }
        }
        /**
         * Force every List that has an overflow to reduce size
         * 
         */
        private void advance()
        {
            bool advanced = false;
            for(int count=0; count< linesItems.Count; count++)
            {
                int lineSize = linesItems[count].ITEMS.Count;
                if (lineSize >= Width)
                {
                    linesItems[count].ITEMS.RemoveAt(0);
                    advanced = true;
                }
            }

            if (advanced) {
                updateStartTime();
            }
        }

        public void pushTemp(int lines, double temp)
        {
            if (minutes == -1)
            {
                DateTime currDate = DateTime.Now;

                minutes = currDate.Minute;
                hour = currDate.Hour;
            }
            // Create lines
            if (linesItems.Count < lines+1)
            {
                for(int x=linesItems.Count; x<lines+1; x++)
                {
                    linesItems.Add(new lineItems(x));
                }
            }
            if (linesItems[lines].ITEMS.Count >= Width)
            {
                advance();
            }
            linesItems[lines].ITEMS.Add(temp);
        }

        /**
         * Set colors
         */
        public void setColors(Color color)
        {
            int currentIndex = colorList.Count;
            colorList.Add(new colorObject(currentIndex+1, color));
        }
        public void resetColors()
        {
            colorList.Clear();
        }

        private double calculateSteps(double min, double max,double minSteps = 5)
        {
            double steps = 0;
            if (min>=0 && max >= 0)
            {
                steps = max - min;
            } 
            else if (min <0 && max > 0)
            {
                steps = max + Math.Abs(min);
            } 
            else if (min <0 && max < 0)
            {
                steps = Math.Abs(max) - Math.Abs(min);
            }
            steps = steps < minSteps ? minSteps : steps;
            steps = Height / steps;
            return steps;
        }

        private void drawXAxis()
        {
            Font drawFont = new Font("Arial", 8, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            Pen axisPen = new Pen(axisColor);
            int startX = margin + minutes;
            int startHour = hour;
            axisPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            for (int x = startX; x < Width; x += 60)
            {
                tempGraphics.DrawLine(axisPen, x, margin, x, Height);
                String label = startHour.ToString()+":00";
                int cc = x - 14;
                if (cc < margin)
                {
                    cc = margin;
                }
                tempGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                tempGraphics.DrawString(label, drawFont, drawBrush, new Point(cc, Height-12));
                tempGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                startHour += 1;
                if (startHour == 24)
                {
                    startHour = 0;                         
                }
            }
        }

        private void drawYAxis(double min, double max)
        {
            double degreeStep = calculateSteps(min,max);

            Pen axisPen = new Pen(axisColor);

            Font drawFont = new Font("Arial", 8,FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            axisPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            int degree_count = 0;
            int deg_count =(int) min;
            for (float y = Height; y >= margin ; y -= (float) degreeStep)
            {
                tempGraphics.DrawLine(axisPen, margin, (float) y, Width, (float) y);
                degree_count += 1;
                deg_count++;
                if ((degree_count % 5) == 0)
                {
                    string label = deg_count + "°C";
                    tempGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    tempGraphics.DrawString(label, drawFont, drawBrush, new Point(margin,(int) y-5));
                    tempGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    axisPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                } else
                {
                    axisPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }
            }
        }

        private void drawAxis()
        {
            Pen axisPen = new Pen(axisColor);
            tempGraphics.DrawLine(axisPen, margin, margin, margin, Height);
            tempGraphics.DrawLine(axisPen, margin, Height, Width, Height);
        }

        private void cleanUpBuffer()
        {
            tempGraphics.Clear(backgroundColor);
        }

        private void copyImage()
        {
            currentPicture.Invalidate();
        }

        public void drawGraphAxis()
        {
            cleanUpBuffer();
            double[] maxMin = calcMaxMin();
            min = maxMin[0];
            max = maxMin[1];
            drawAxis();
            drawXAxis();
            drawYAxis(min,max);
        }

        private double[] calcMaxMin()
        {
            double[] maxMin = new double[2];
            int lines = linesItems.Count;
            double min = 90;
            double max = -50;
            for (int x=0; x<lines; x++)
            {
                List<double> tempItems = linesItems[x].ITEMS;
                for (int y =0; y< tempItems.Count; y++)
                {
                    if (tempItems[y] > max)
                    {
                        max = tempItems[y];
                    }
                    if (tempItems[y]< min)
                    {
                        min = tempItems[y];
                    }
                }
            }
            maxMin[0] = min-1;
            maxMin[1] = max+1;
            return maxMin;
        }
        public void drawGraphData()
        {
            int lines = linesItems.Count;
            double degreeStep = calculateSteps(min, max,1);
            double transpose = min * -1;
            tempGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            for (int x=0; x<lines; x++)
            {
                List<Double> tempItems = linesItems[x].ITEMS;
                Color penColor = Color.FromArgb(0x50, 0x50, 0x50);
                if (colorList.Count >= 1)
                { 
                    penColor = colorList[x].ColorItem;
                }
                Pen currentPen = new Pen(penColor);
                for (int y=1; y<tempItems.Count; y++)
                {
                    int startX = (int) y - 1+margin;
                    int endX = (int) y+margin;
                    int startY = (int) (Height-((tempItems[y - 1] + transpose) * degreeStep+margin));
                    int endY = (int) (Height-((tempItems[y] + transpose) * degreeStep+margin));
                    tempGraphics.DrawLine(currentPen, new Point(startX, startY), new Point(endX, endY));
                }
           }
            tempGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

        }
        public void blitGraph()
        {
            copyImage();
        }
    }
}
