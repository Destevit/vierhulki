using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphX;
using VierhulkiCore;
using GraphX.PCL.Logic.Models;
using GraphX.PCL.Common.Interfaces;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data;

namespace VierhulkiGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable matrix; BackgroundWorker bw;
        BinaryTreeChecker<GuiVertex, GuiEdge> btc;
        List<GuiEdge> edges = new List<GuiEdge>();
        List<GuiVertex> vertices = new List<GuiVertex>();
        GuiGraph graph;
        public MainWindow()
        {
            InitializeComponent();
            graph = new GuiGraph();
            SetupArea();
        }

        private void SetupArea()
        {
            var logicCore = new GuiGXLogicCore { Graph = graph };
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;
            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;
            logicCore.AsyncAlgorithmCompute = false;
            Area.LogicCore = logicCore;
        }

        private void generateGraph()
        {
            graph.Clear();
            graph.AddVertexRange(vertices);
            graph.AddEdgeRange(edges);
            Area.GenerateGraph(true, true);
            Zoom.ZoomToFill();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (bw == null)
            {
                btc = new BinaryTreeChecker<GuiVertex, GuiEdge>();
                bw = new BackgroundWorker();
                btc.EdgeChecking += Btc_EdgeChecking;
                btc.CheckingFailed += Btc_CheckingFailed;
                bw.DoWork += Bw_DoWork;
                bw.WorkerReportsProgress = true;
                bw.ProgressChanged += Bw_ProgressChanged;
                bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            }
            if (!bw.IsBusy)
                bw.RunWorkerAsync(directional.IsChecked.GetValueOrDefault());
        }

        private void Btc_CheckingFailed(object sender, CheckingFailedEventArgs e)
        {
            bw.ReportProgress(2, e);
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if((bool)e.Result == true)
            {
                info.Content = "Graf jest drzewem binarnym";
            }
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                BringToFront(Area.EdgesList[(GuiEdge)e.UserState]);
                Area.EdgesList[(GuiEdge)e.UserState].Foreground = Brushes.Blue;
                Area.VertexList[(GuiVertex)(((GuiEdge)e.UserState).From)].Foreground = Brushes.Blue;
            }
            else if(e.ProgressPercentage == 1)
            {
                Area.EdgesList[(GuiEdge)e.UserState].Foreground = Brushes.Black;
            }
            else
            {
                CheckingFailedEventArgs cfea = (CheckingFailedEventArgs)e.UserState;
                string where = "";
                if (cfea.FailedVertex != null)
                {
                    Area.VertexList[(GuiVertex)cfea.FailedVertex].Foreground = Brushes.Red;
                    where = "w. " + cfea.FailedVertex.ID;
                }
                if (cfea.FailedEdge != null)
                {
                    Area.EdgesList[(GuiEdge)cfea.FailedEdge].Foreground = Brushes.Red;
                    where = "k. " + cfea.FailedEdge.From.ID + "->" + cfea.FailedEdge.To.ID;
                }
                info.Content = cfea.Message + (where == "" ? "" : (" (" + where + ")"));
            }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            bool result = btc.Check(edges[0].From, edges, (bool)e.Argument);
            e.Result = result;
        }

        private void Btc_EdgeChecking(object sender, EdgeCheckingEventArgs e)
        {
            bw.ReportProgress(0, (GuiEdge)e.EdgeChecked);
            Thread.Sleep(500);
            bw.ReportProgress(1, (GuiEdge)e.EdgeChecked);
        }
        private void BringToFront(FrameworkElement element)
        {
            var maxZ = Area.Children.OfType<UIElement>()
           .Where(x => x != element)
           .Select(x => Panel.GetZIndex(x))
           .Max();
            Panel.SetZIndex(element, maxZ + 1);
        }

        private void matrixSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            int size;
            if (int.TryParse(matrixSize.Text, out size))
            {
                for (int i = 0; i < size; i++)
                {
                    dt.Columns.Add((i+1).ToString());
                }
                for (int i = 0; i < size; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                Grid.ItemsSource = dt.DefaultView;
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grid.CommitEdit();
            Grid.CommitEdit();
            Grid.Items.Refresh();
            if (AlgorithmTab != null && AlgorithmTab.IsSelected)
            {
                vertices.Clear();
                edges.Clear();
                matrix = ((DataView)Grid.ItemsSource)?.ToTable();
                if (matrix == null) return;
                int size = matrix.Columns.Count;
                GuiVertex[] _vertices = new GuiVertex[size];

                for (int i = 0; i < size; i++)
                {
                    _vertices[i] = new GuiVertex();
                }
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (matrix.Rows[i][j].ToString() == "1")
                            edges.Add(new GuiEdge(_vertices[i], _vertices[j]));
                    }
                }
                vertices.AddRange(_vertices);
                generateGraph();
            }
        }

        private void Grid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^01]").IsMatch(e.Text);
        }
    }
}
