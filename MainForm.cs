using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MakingDecisionSolver
{
    public partial class MakingDecisionSolver : Form
    {
        private List<Alternative> alternatives = new List<Alternative>();
        private TableLayoutPanel mainTable = null!;
        private TableLayoutPanel resultTable = null!;
        private TableLayoutPanel headerTable = null!;
        private NumericUpDown num = null!;
        public event EventHandler NumValueChanged;        
        public MakingDecisionSolver()
        {
            InitializeComponent();
        }

        private void MakingDecisionSolver_Load(object sender, EventArgs e)
        {
            resize_main_table(2, is_new: true);
        }
        private void add_table_header()
        {
            headerTable = new TableLayoutPanel();
            headerTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            headerTable.Dock = DockStyle.Top;
            headerTable.ColumnCount = 4;
            headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            headerTable.RowCount = 1;
            headerTable.RowStyles.Add(new RowStyle());
            headerTable.Height = 40;
            this.Controls.Add(headerTable);

            num = new NumericUpDown();
            num.Dock = DockStyle.Fill;
            num.Anchor = AnchorStyles.None;
            num.Minimum = 2;
            num.Value = alternatives.Count;
            num.ValueChanged += (s, e) => ResizeMainTable();
            headerTable.Controls.Add(num, 0, 0);

            Label lbl = new Label();
            lbl.Anchor = AnchorStyles.None;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Text = "Альтернативы";
            headerTable.Controls.Add(lbl, 1, 0);

            lbl = new Label();
            lbl.Anchor = AnchorStyles.None;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Text = "Цены";
            headerTable.Controls.Add(lbl, 2, 0);

            lbl = new Label();
            lbl.Anchor = AnchorStyles.None;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Text = "Доход";
            headerTable.Controls.Add(lbl, 3, 0);
        }

        public void add_resultTable(bool isReadOnly = true)
        {
            resultTable = new TableLayoutPanel();
            resultTable.AutoSize = true;
            resultTable.Anchor = AnchorStyles.None;
            resultTable.ColumnCount = 1;
            resultTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            resultTable.RowCount = 2;
            resultTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            resultTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            resultTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            mainTable.Controls.Add(resultTable, 0, Convert.ToInt32(mainTable.RowCount / 2));

            TextBox txtBox = new TextBox();
            txtBox.Anchor = AnchorStyles.None;
            txtBox.TextAlign = HorizontalAlignment.Center;
            txtBox.Text = "Результат";
            txtBox.ReadOnly = isReadOnly;
            resultTable.Controls.Add(txtBox, 0, 0);

            txtBox = new TextBox();
            txtBox.Anchor = AnchorStyles.None;
            txtBox.TextAlign = HorizontalAlignment.Center;
            txtBox.Text = "";
            txtBox.ReadOnly = isReadOnly;
            resultTable.Controls.Add(txtBox, 0, 1);
        }

        private void ResizeMainTable()
        {
            int delta = Math.Abs(alternatives.Count - (int)num.Value);
            if (num.Value < alternatives.Count)
            {
                for (int i = 0; i < delta; ++i)
                {
                    alternatives.RemoveAt(alternatives.Count - 1);
                }
            }
            else
            {
                for (int i = 0; i < delta; ++i)
                {
                    alternatives.Add(new Alternative());
                }
            }
            resize_main_table(alternatives.Count);
        }
        private void AddAlternativeControl(Alternative alt, int pos)
        {
            AlternativeControl altControl = new AlternativeControl(alt, mainTable, pos + 1);
            altControl.ValueSettingsChanged += (s, e) => FindProfit();
        }

        private void FindProfit()
        {
            for (int i = 0; i < alternatives.Count; ++i)
            {
                mainTable.Controls[i * 7 + 1].Controls[1].Text = alternatives[i].FindAlternativeProfit().ToString();
            }
            resultTable.Controls[1].Text = alternatives.Max(x => x.FindAlternativeProfit()).ToString();
        }

        private void resize_main_table(int alt_count, bool is_new = false)
        {
            if (mainTable != null)
            {
                mainTable.RowCount = 0;
                mainTable.ColumnCount = 0;
                this.Controls.Remove(mainTable);
                this.Controls.Remove(headerTable);                
                this.Refresh();
            }
            mainTable = new TableLayoutPanel();
            mainTable.Dock = DockStyle.Fill;
            mainTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            mainTable.AutoScroll = true;
            mainTable.ColumnCount = 4;
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            mainTable.RowCount = alt_count * 3;
            int rowCount = mainTable.RowCount;
            for (int i = 0; i < rowCount; ++i)
            {
                mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / rowCount));
            }
            this.Controls.Add(mainTable);

            //Результат
            add_resultTable();
            //Альтернативы
            if (is_new)
            {
                for (int i = 0; i < alt_count; ++i)
                {
                    alternatives.Add(new Alternative());
                    AddAlternativeControl(alternatives[i], i);
                }
            }
            else
            {
                for (int i = 0; i < alt_count; ++i)
                {
                    AddAlternativeControl(alternatives[i], i);
                }
            }
            //Шапка
            add_table_header();
            FindProfit();
        }
    }
}


