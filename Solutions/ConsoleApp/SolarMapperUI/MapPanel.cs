using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
    internal abstract class MapPanel : Panel
    {
        protected MapPanel()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;
            
            this.InitializeHandlers();
        }

        protected virtual void InitializeHandlers()
        {
            this.MouseClick += BodyClick;
            this.Paint += PrintObjects;
        }

        protected abstract void PrintObjects(object sender, PaintEventArgs e);
        protected abstract void BodyClick(object sender, MouseEventArgs e);

    }
}
