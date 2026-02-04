
using SharpDX.Direct3D9;

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;


using WpfProj.MVVM;


namespace WpfProj.ViewModel
{
    public class ScreenRecord3D9VM : ViewModelBase
    {
        readonly string videoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "video.mp4");
        readonly string tempFolderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "Temp");

        //private readonly Queue<BitmapVideoFrameWrapper> sources = new Queue<BitmapVideoFrameWrapper>();
        
        

        private Direct3D _direct3D;
        private Device _d3dDevice;
        private Surface _offscreenSurface;                
        private System.Windows.Threading.DispatcherTimer _captureTimer;

        private VideoEncoding _encoder;
        private RenderVideo _render;
        private int indexer = 1;

        public void CreateD3DDevice(IntPtr handle)
        {
            DisposeDevice(); // ensure cleanup if re-created

            _direct3D = new Direct3D();

            var presentParams = new PresentParameters
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard,
                BackBufferFormat = Format.Unknown,
                PresentationInterval = PresentInterval.Default,               
            };


            _d3dDevice = new Device(
               _direct3D,
               0,
               DeviceType.Hardware,
               handle,
               CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded,
               presentParams);



            var adapterCount = _direct3D.AdapterCount;

            // Get the adapter display mode and size the capture surface to the FULL SCREEN
            var displayMode = _direct3D.GetAdapterDisplayMode(0);
            int _captureWidth = displayMode.Width;
            int _captureHeight = displayMode.Height;
                    

            // Destination must be system memory for GetFrontBufferData
            _offscreenSurface = Surface.CreateOffscreenPlain(
                _d3dDevice,                
                _captureWidth,
                _captureHeight,
                Format.A8R8G8B8,
                Pool.SystemMemory);

        }


        private void StartCaptureLoop()
        {
            if (_captureTimer != null) return;
            indexer = 1;

            _captureTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(33) // ~30 FPS
            };
            _captureTimer.Tick += (s, e) =>
            {
                try
                {
                    CaptureFrame();
                }
                catch (Exception ex)
                {
                    // Handle/log as appropriate
                    System.Diagnostics.Debug.WriteLine($"CaptureFrame error: {ex}");
                }
            };
            _captureTimer.Start();
        }

        private void StopCaptureLoop()
        {
            if (_captureTimer != null)
            {
                _captureTimer.Stop();
                _captureTimer = null;
            }
        }

        private void CaptureFrame()
        {
            if (_d3dDevice == null || _offscreenSurface == null) return;           
            

            // GetFrontBufferData requires a plain surface in system memory as destination.
            // So create a temp render-target compatible surface and then copy to system memory.
            // For desktop capture, use the adapter's front buffer:
            _d3dDevice.GetFrontBufferData(0, _offscreenSurface);

            // create file
            var filePath = Path.Combine(tempFolderName, $"img{indexer++.ToString("D4")}.Png");
            Surface.ToFile(_offscreenSurface, filePath, ImageFileFormat.Png);
            _encoder?.AddFrame(filePath);
            
            
            //With BitmapVideoFrameWrapper
            //Bitmap bitmap = new Bitmap(filePath);
            //_encoder?.AddFrame(bitmap);
            
            //BitmapVideoFrameWrapper wrappedBitmap = new BitmapVideoFrameWrapper(bitmap);
            //sources.Enqueue(wrappedBitmap);

        }

        public void StartRecording(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            // Create D3D device sized to capture area
            CreateD3DDevice(hwnd);

            // Initialize encoder
            _encoder = new VideoEncoding();
            _encoder.VideoEncoder(videoPath, frameRate: 10);

           
            Directory.CreateDirectory(tempFolderName);
            // Start frame capture loop
            StartCaptureLoop();

        }

        public void StopRecording()
        {
            // Logic to stop screen recording
            StopCaptureLoop();
            try
            {
                // Finish encoding and cleanup               
                //_encoder?.FinalizeAsync();
                _encoder?.Finalize(true);
                MessageBox.Show("Screen recording saved to: " + videoPath);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FinalizeAsync error: {ex}");
            }
            finally
            {
                Directory.Delete(tempFolderName, true);
                _encoder?.Dispose();
                _encoder = null;
                DisposeDevice();
            }


        }

        public void VideoRender()
        {
            _render = new RenderVideo();
            // Finish encoding and cleanup
            _render.FinalizeAsync();
            
        }


        

        private void DisposeDevice()
        {
            try
            {
                _offscreenSurface?.Dispose();
                _offscreenSurface = null;
                _d3dDevice?.Dispose();
                _d3dDevice = null;
                _direct3D?.Dispose();
                _direct3D = null;
            }
            catch { /* swallow dispose errors safely */ }
        }


    }
}
