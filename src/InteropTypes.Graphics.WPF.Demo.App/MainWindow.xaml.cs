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

namespace WPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new AppMVVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var gltfScene = new InteropTypes.Graphics.Backends.GltfSceneBuilder();

            using(var dc = gltfScene.Create3DContext())
            {
                // mySceneRoot.DrawTo(dc);                
            }

            // var cam = mySceneRoot.Camera;

            // gltfScene.SetCamera(cam);

            gltfScene.Save("result.glb", new InteropTypes.Graphics.Backends.GLTFWriteSettings { CameraSize=2 });            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var record = new InteropTypes.Graphics.Drawing.Record3D();
            // mySceneRoot.DrawTo(record);            

            var camView = InteropTypes.Graphics.Drawing.CameraView3D.CreateDefaultFrom(record.BoundingMatrix);

            // mySceneRoot.Camera = camView;
        }
    }
}
