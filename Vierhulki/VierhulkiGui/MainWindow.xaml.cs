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
        BinaryTreeChecker<GuiVertex, GuiEdge> btc = new BinaryTreeChecker<GuiVertex, GuiEdge>();
        List<GuiEdge> edges;
        public MainWindow()
        {
            InitializeComponent();
            SetupArea();
            Loaded += MainWindow_Loaded;
        }

        private void SetupArea()
        {
            var logicCore = new GuiGXLogicCore { Graph = createGraph() };
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Area.GenerateGraph(true, true);
            Zoom.ZoomToFill();
        }

        private GuiGraph createGraph()
        {
            GuiGraph graph = new GuiGraph();

            GuiVertex[] vertices = new GuiVertex[5];
            edges = new List<GuiEdge>();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new GuiVertex();
            }
            edges.Add(new GuiEdge(vertices[0], vertices[1]));
            edges.Add(new GuiEdge(vertices[0], vertices[2]));
            edges.Add(new GuiEdge(vertices[1], vertices[3]));
            edges.Add(new GuiEdge(vertices[1], vertices[4]));
            edges.Add(new GuiEdge(vertices[1], vertices[0]));
            edges.Add(new GuiEdge(vertices[2], vertices[0]));
            edges.Add(new GuiEdge(vertices[3], vertices[1]));
            edges.Add(new GuiEdge(vertices[4], vertices[1]));
            graph.AddVerticesAndEdgeRange(edges);

            return graph;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (bw == null)
            {
                bw = new BackgroundWorker();
                btc.EdgeChecking += Btc_EdgeChecking;
                bw.DoWork += Bw_DoWork;
                bw.WorkerReportsProgress = true;
                bw.ProgressChanged += Bw_ProgressChanged;
            }
            if (!bw.IsBusy)
                bw.RunWorkerAsync();
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                BringToFront(Area.EdgesList[(GuiEdge)e.UserState]);
                Area.EdgesList[(GuiEdge)e.UserState].Foreground = Brushes.Blue;
                Area.VertexList[(GuiVertex)(((GuiEdge)e.UserState).From)].Foreground = Brushes.Blue;
            }
            else
            {
                Area.EdgesList[(GuiEdge)e.UserState].Foreground = Brushes.Black;
            }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            btc.Check(edges[0].From, edges, false);
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
                    dt.Columns.Add(i.ToString());
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
                matrix = ((DataView)Grid.ItemsSource).ToTable();
                MessageBox.Show(matrix.Rows[2][2].ToString());
            }
        }

        private void Grid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^01]").IsMatch(e.Text);
        }
    }
}
