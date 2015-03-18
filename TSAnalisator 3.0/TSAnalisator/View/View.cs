using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TSAnalisator.Model.PrintStructures;

namespace TSAnalisator.View
{
    public class View : Form, IView
    {
        private BufferedGraphics _bufGraph;
        private BufferedGraphicsContext _bufGraphContext;

        public View()
        {
            InitializeComponent();

            this.SuspendLayout();
            this.Visible = true;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            _bufGraphContext = BufferedGraphicsManager.Current;
            _bufGraphContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            _bufGraph = _bufGraphContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
            this.ResumeLayout();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // View
            // 
            this.ClientSize = new System.Drawing.Size(167, 773);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "View";
            this.ShowIcon = false;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.View_Paint);
            this.Resize += new System.EventHandler(this.View_Resize);
            this.ResumeLayout(false);

        }

        private void View_Resize(object sender, EventArgs e)
        {
            try
            {
                this.SuspendLayout();
                _bufGraphContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                if (_bufGraph != null)
                {
                    _bufGraph.Dispose();
                    _bufGraph = null;
                }

                _bufGraph = _bufGraphContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
                this.ResumeLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void View_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                _bufGraph.Render(e.Graphics);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Draw(List<DrawPrint> prints)
        {
            try
            {
                for (int i = 0; i < prints.Count; i++)
                {
                    if (prints[i].Bitmap == null)
                    {
                        prints[i].Bitmap = new Bitmap(this.Width, prints[i].PrintFont.Height);

                        Graphics tmpBufGraph = Graphics.FromImage(prints[i].Bitmap);
                        tmpBufGraph.Clear(prints[i].GroundColor);
                        tmpBufGraph.DrawString(prints[i].Time.ToString(prints[i].TimePattern) + "  " + prints[i].OutputPrice + "  " + prints[i].Volume.ToString() + "  " + prints[i].Info, prints[i].PrintFont, new SolidBrush(prints[i].FontColor), 0, 0);
                    }

                    Action action = () =>
                    {
                        _bufGraph.Graphics.DrawImage(prints[i].Bitmap, 0, i * prints[i].Bitmap.Height, prints[i].Bitmap.Width, prints[i].Bitmap.Height);
                    };

                    if (InvokeRequired)
                        Invoke(action);
                    else
                        action();
                }

                this.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Draw(long totalVolume)
        {
            try
            {
                Action action = () =>
                {
                    this.Text = totalVolume.ToString();
                };

                if (InvokeRequired)
                    Invoke(action);
                else
                    action();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
