using FFMpegCore;
using FFMpegCore.Extensions.Downloader;
using FFMpegCore.Extensions.System.Drawing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;




public class RenderVideo
{
    private string _outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "record.mp4");
    private string testBmpPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "test.bmp");

    private List<string> imageFiles = new List<string>
    {
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder" , "Temp", "img0001.Png"),
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "Temp", "img0002.Png"),
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "Temp", "img0003.Png"),
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "Temp", "img0004.Png"),
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "Temp", "img0005.Png"),
    };
    IEnumerable<BitmapVideoFrameWrapper> CreateFramesSD(int count, int width, int height)
    {
        for (int i = 0; i < count; i++)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics gfx = Graphics.FromImage(bmp);

            gfx.Clear(Color.Navy);

            Point pt = new Point(i, i);
            Size sz = new Size(i, i);
            var rect = new Rectangle(pt, sz);
            gfx.FillRectangle(Brushes.Green, rect);

            Font fnt = new Font("consolas", 24);
            gfx.DrawString($"Frame: {i + 1:N0}", fnt, Brushes.Yellow, 2, 2);

            var wrappedBitmap = new BitmapVideoFrameWrapper(bmp);
            yield return wrappedBitmap;
        }

    }

    public IEnumerable<BitmapVideoFrameWrapper> CreateSingleFrame(int width, int height)
    {
        // 1. Create a bitmap
        //var bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
        var bmp = new Bitmap(testBmpPath);


        // 2. Draw on the bitmap
        using (Graphics gfx = Graphics.FromImage(bmp))
        {
            gfx.Clear(Color.Yellow);
            gfx.FillEllipse(Brushes.Orange, 10, 10, width - 20, height - 20);
            gfx.DrawString("Sample Frame", new Font("Arial", 24), Brushes.Yellow, 20, 20);
        }

        // 3. Wrap the bitmap for FFmpegCore
        var wrappedBitmap = new BitmapVideoFrameWrapper(bmp);

        // 4. Return the wrapped frame
        yield return wrappedBitmap;
    }









    public async Task FinalizeAsync()
    {
        var ffDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg");
        GlobalFFOptions.Configure(x => x.BinaryFolder = ffDir);

        Directory.CreateDirectory(ffDir);
        if (!System.IO.File.Exists(System.IO.Path.Combine(ffDir, "ffmpeg.exe")) ||
            !System.IO.File.Exists(System.IO.Path.Combine(ffDir, "ffprobe.exe")))
        {
            // Ensure TLS 1.2 for downloads on .NET Framework
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

            // Offload to background to avoid freezing UI
            await System.Threading.Tasks.Task.Run(() =>
                FFMpegDownloader.DownloadBinaries(FFMpegCore.Extensions.Downloader.Enums.FFMpegVersions.LatestAvailable));

        }

        // var frames = CreateFramesSD(count: 100, width: 400, height: 300);
        //var frames = CreateSingleFrame(width: 1900, height: 1200);

        foreach (var file in imageFiles)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("Image file not found: " + file);
            // Optionally, check image size:
            using (var img = Image.FromFile(file))
            {
                Console.WriteLine($"{file}: {img.Width}x{img.Height}");
            }
        }

        FFMpegArguments
            .FromFileInput(imageFiles, verifyExists: false)
            .OutputToFile(_outputPath, overwrite: true, options => options
                .WithVideoCodec(FFMpegCore.Enums.VideoCodec.LibX264)
                .WithFramerate(2)
                .WithCustomArgument("-pix_fmt yuv420p")) // Ensures compatibility
            .ProcessSynchronously();
    }

}

