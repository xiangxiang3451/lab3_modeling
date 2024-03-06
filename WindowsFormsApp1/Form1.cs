using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private bool[,] grid;
        private bool[,] nextGrid;
        private bool isRunning;

        public Form1()
        {
            InitializeComponent();
            InitializeGrid();

            timer = new Timer();
            timer.Interval = 500;  // Set the refresh interval to 0.5 seconds
            timer.Tick += Timer1_Tick;

            grid = new bool[20, 20]; // Initialize the grid
            nextGrid = new bool[20, 20]; // Initialize the grid of the next state
        }

        private void InitializeGrid()
        {

            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.BackColor = Color.Black;

            //Add rows and columns
            for (int i = 0; i < 20; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            }

            // Add grid cells and register click events
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    Panel panel = new Panel();
                    panel.Dock = DockStyle.Fill;
                    panel.Margin = new Padding(0);
                    panel.BackColor = Color.White;
                    panel.Click += Panel_Click; // Add click event

                    tableLayoutPanel1.Controls.Add(panel, col, row);
                }
            }
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                Panel panel = sender as Panel;
                if (panel != null)
                {
                    int row = tableLayoutPanel1.GetPositionFromControl(panel).Row;
                    int col = tableLayoutPanel1.GetPositionFromControl(panel).Column;
                    grid[row, col] = !grid[row, col];
                    panel.BackColor = grid[row, col] ? Color.Black : Color.White;
                }
            }
        }

        private void ApplyRule110()
        {
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    int aliveNeighbors = CountAliveNeighbors(row, col);

                    // Apply Rule 110
                    if ((grid[row, col] && aliveNeighbors == 2) || aliveNeighbors == 1)
                    {
                        nextGrid[row, col] = true;
                    }
                    else
                    {
                        nextGrid[row, col] = false;
                    }
                }
            }
        }

        private int CountAliveNeighbors(int row, int col)
        {
            int count = 0;

            // Check the left neighbor
            if (col > 0 && grid[row, col - 1])
            {
                count++;
            }

            // Check the right neighbor
            if (col < 19 && grid[row, col + 1])
            {
                count++;
            }

            return count;
        }

        private void UpdateGrid()
        {
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    grid[row, col] = nextGrid[row, col];
                    UpdatePanelColor(row, col);
                }
            }
        }

        private void UpdatePanelColor(int row, int col)
        {
            Panel panel = tableLayoutPanel1.GetControlFromPosition(col, row) as Panel;
            if (panel != null)
            {
                panel.BackColor = grid[row, col] ? Color.Black : Color.White;
            }
        }



        private void StartButton_Click(object sender, EventArgs e)
        {
            isRunning = true;
            timer.Start();
        }



        private void Timer1_Tick(object sender, EventArgs e)
        {
            ApplyRule110(); // reply rule 110
            UpdateGrid();
        }

        private void PauseButton_Click_1(object sender, EventArgs e)
        {
            isRunning = false;
            timer.Stop();
        }
    }
}