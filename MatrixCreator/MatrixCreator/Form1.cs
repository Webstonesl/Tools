using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MatrixCreator
{
    public partial class Form1 : Form
    {
        public static string[] args;
        public string FileName;
        public bool saved = true;
        public Form1()
        {
            
            InitializeComponent();
            if (args.Length > 0)
            {
                open(args[0]);
            }
            
            matrix.ColumnCount = (int)numCols.Value;
            matrix.RowCount = (int)numRows.Value;
            matrix.ColumnHeadersVisible = false;
            matrix.RowHeadersVisible = false;
            colwidthSize();
            rowHeightSize();
            size();
            numCols.Left = ClientSize.Width - numCols.Width;
        }

        private void size()
        {
            matrix.Width = 50 * ((int)numCols.Value) + 3;
            numCols.Top = menuStrip1.Bottom;
            numRows.Top = menuStrip1.Bottom;
            
            matrix.Top = numCols.Bottom+1;

            this.ClientSize = new Size (Math.Max(matrix.Width,153),matrix.Bottom);
            matrix.Left= 0;
            //matrix.Width;
        }
        private void colwidthSize()
        {
            matrix.Width = 50 * ((int)numCols.Value)+3;
            
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                matrix.Columns[i].Width = 50;
            }
            size();
        }
        private void rowHeightSize()
        {

            matrix.Height = 25 * matrix.RowCount+3;
            
            for (int i = 0; i < matrix.RowCount; i++)
            {
                matrix.Rows[i].Height = 25;
            }
            size();
        }

        private void numCols_ValueChanged(object sender, EventArgs e)
        {
            matrix.ColumnCount = (int)numCols.Value;

            saved = false;

            colwidthSize();
        }

        private void numRows_ValueChanged(object sender, EventArgs e)
        {
            matrix.RowCount = (int)numRows.Value;
            saved = false;
            rowHeightSize();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public void open(string fileName)
        {
            
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            StreamReader stream = new StreamReader(fileStream);
            String s = stream.ReadLine();
            string[] a = new string[1];
            a[0] = " x ";
            a = s.Split(a, StringSplitOptions.None);
            matrix.RowCount = int.Parse(a[0]);
            numRows.Value = matrix.RowCount;
            matrix.ColumnCount = int.Parse(a[1]);
            numCols.Value = matrix.ColumnCount;
            for(int i = 0; i < matrix.RowCount; i++)
            {
                s = stream.ReadLine();
                a = new string[1];
                a[0] = ", ";
                a = s.Split(a,StringSplitOptions.RemoveEmptyEntries);
                for(int j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix.Rows[i].Cells[j].Value = a[j];
                }
            }
            
            stream.Close();
            FileName = fileName;
            saved = true;
            colwidthSize();
            rowHeightSize();
            size();
        }
        public void save(string fileName)
        {

            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            streamWriter.Write(numRows.Value);
            streamWriter.Write(" x ");
            streamWriter.Write(numCols.Value);
            streamWriter.WriteLine();
           
            for (int i = 0; i < matrix.RowCount; i++)
            {

                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    streamWriter.Write(matrix.Rows[i].Cells[j].Value);
                    if (j < matrix.ColumnCount - 1)
                        streamWriter.Write(", ");
                }
                streamWriter.WriteLine();
            }
            streamWriter.Flush();
            FileName = fileName;
            saved = true;
            streamWriter.Close();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.DefaultExt = "*.matrix";
            
            if (s.ShowDialog() == DialogResult.OK)
            {
                save(s.FileName);
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.DefaultExt = "*.matrix";
            if (o.ShowDialog() == DialogResult.OK)
            {
                open(o.FileName);
            }
        }
        object s = null;
        private void matrix_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           
                s = matrix.SelectedCells[0].Value;
        }

        private void matrix_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (s!=(matrix.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                saved = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saved)
                return;
            DialogResult r = MessageBox.Show("Do you want to save?", "File Not Saved", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
            if (r == DialogResult.Yes)
            {
                if (FileName != "")
                {
                    save(FileName);
                }
                else
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            else
            if (r == DialogResult.Cancel)
            {
                e.Cancel = true;
            }

        }

    }
}
