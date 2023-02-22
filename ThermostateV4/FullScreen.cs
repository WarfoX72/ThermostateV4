using System.Windows.Forms;
using System.Drawing;

namespace ThermostateV4

{
    public class fullScreen
    {
        private FormWindowState winState;
        private FormBorderStyle brdStyle;
        private bool topMost;
        private Rectangle bounds;

        private bool IsMaximized = false;

        public void Maximize(Form targetForm)
        {
            if (!IsMaximized)
            {
                IsMaximized = true;
                Save(targetForm);
                targetForm.WindowState = FormWindowState.Maximized;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.TopMost = true;
                WinApi.SetWinFullScreen(targetForm.Handle);
            }
        }

        private void Save(Form targetForm)
        {
            winState = targetForm.WindowState;
            brdStyle = targetForm.FormBorderStyle;
            topMost = targetForm.TopMost;
            bounds = targetForm.Bounds;
        }

        public void Restore(Form targetForm)
        {
            targetForm.WindowState = winState;
            targetForm.FormBorderStyle = brdStyle;
            targetForm.TopMost = topMost;
            targetForm.Bounds = bounds;
            IsMaximized = false;
        }
    }

}
