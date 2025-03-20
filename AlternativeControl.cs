using System;
using System.Drawing;
using System.Windows.Forms;


namespace MakingDecisionSolver
{
    class AlternativeControl : UserControl
    {
        private Alternative alt;        
        private int pos = 0;

        //Контейнеры
        private TableLayoutPanel mainTable;        
        private TableLayoutPanel probTable;
        public event EventHandler ValueSettingsChanged;

        public AlternativeControl(Alternative _alt, TableLayoutPanel main, int pos)
        {
            this.mainTable = main;
            this.alt = _alt;
            this.pos = pos;
            InitializeControl(pos);
        }

        private void InitializeControl(int i)
        {
            add_cell_alternative(i);
        }
        private void add_cell_alternative(int i)
        {
            int pos = 3 * i - 2;
            //альтернатива
            add_cell(1, pos, "Альтернатива_" + i, isReadOnly: false);
            //повысилась
            add_cell(2, pos - 1, "Повысилась", "inc");
            //не изм
            add_cell(2, pos, "Не изм", "nchange");
            //понизизилась
            add_cell(2, pos + 1, "Понизилась", "dec");
        }
        public void add_cell(int column, int row, string tbox_text, string name = "", bool isReadOnly = true)
        {
            probTable = new TableLayoutPanel();
            probTable.AutoSize = true;
            probTable.Anchor = AnchorStyles.None;
            probTable.ColumnCount = 1;
            probTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            probTable.RowCount = 2;
            probTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            probTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            probTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            mainTable.Controls.Add(probTable, column, row);

            TextBox txtBox = new TextBox();
            txtBox.Anchor = AnchorStyles.None;
            txtBox.TextAlign = HorizontalAlignment.Center;
            txtBox.Text = isReadOnly ? tbox_text : alt.Name == "" ? tbox_text : alt.Name;
            if(!isReadOnly)
                txtBox.TextChanged += (s, e) => alt.Name = txtBox.Text;
            txtBox.ReadOnly = isReadOnly;
            probTable.Controls.Add(txtBox, 0, 0);

            if (isReadOnly)
            {
                NumericUpDown probValue = new NumericUpDown();
                probValue.Dock = DockStyle.Fill;
                probValue.Anchor = AnchorStyles.None;
                probValue.TextAlign = HorizontalAlignment.Center;
                probValue.DecimalPlaces = 2;
                probValue.Maximum = 1M;
                probValue.Value = alt.GetField<decimal>(name + "Probability");
                probValue.ValueChanged += (s, e) => { alt.SetField(name + "Probability", probValue.Value); OnValueSettingsChanged(); };
                probTable.Controls.Add(probValue, 0, 1);

                NumericUpDown profValue = new NumericUpDown();
                profValue.Dock = DockStyle.Fill;
                profValue.Anchor = AnchorStyles.None;
                profValue.TextAlign = HorizontalAlignment.Center;
                profValue.Maximum = decimal.MaxValue;
                profValue.Minimum = decimal.MinValue;
                profValue.Value = alt.GetField<decimal>(name + "Profit");
                profValue.BackColor = Color.LightGoldenrodYellow;
                profValue.ValueChanged += (s, e) => { alt.SetField(name + "Profit", profValue.Value); OnValueSettingsChanged(); OnValueChangedColor(s, name + "Profit");};
                mainTable.Controls.Add(profValue, column + 1, row);
                OnValueChangedColor(profValue, field_value: profValue.Value);
            }
            else
            {
                TextBox txtBoxAltValue = new TextBox();
                txtBoxAltValue.Anchor = AnchorStyles.None;
                txtBoxAltValue.TextAlign = HorizontalAlignment.Center;
                txtBoxAltValue.ReadOnly = true;
                probTable.Controls.Add(txtBoxAltValue, 0, 1);
            }
        }
        private void OnValueChangedColor(object sender = null, string name = "", decimal field_value = 0)
        {            
            if (name != "")
                field_value = alt.GetField<decimal>(name);
            if (field_value > 0)
            {
                ((NumericUpDown)sender).BackColor = Color.LimeGreen;
            }
            else if (field_value < 0)
            {
                ((NumericUpDown)sender).BackColor = Color.Red;
            }
            else
            {
                ((NumericUpDown)sender).BackColor = Color.LightYellow;
            }

        }
        protected virtual void OnValueSettingsChanged()
        {
            ValueSettingsChanged?.Invoke(this, EventArgs.Empty);
        }       
    }
}
