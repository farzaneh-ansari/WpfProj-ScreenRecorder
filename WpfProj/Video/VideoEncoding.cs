using FFMpegCore;
using FFMpegCore.Extensions.Downloader;
using FFMpegCore.Extensions.System.Drawing.Common;
using FFMpegCore.Pipes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

//using Windows.Graphics.DirectX.Direct3D11;


public class VideoEncoding
{

    private string _outputPath;
    private int _frameRate;
    private readonly Queue<IVideoFrame> _frames = new Queue<IVideoFrame>();
    private readonly Queue<string> _bitmapFrames = new Queue<string>();
    private RawVideoPipeSource _videoFramesSource;
    private string inputPattern = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "Temp", "img%04d.png");
    //>ffmpeg -framerate 30 -i D:\ProjTutorial\WpfProj\WpfProj\bin\Debug\Temp\img%4d.png -c:v libx264 -pix_fmt yuv420p output.mp4



    public void VideoEncoder(string outputPath, int frameRate = 30)
    {
        _outputPath = outputPath;
        _frameRate = frameRate;
        _videoFramesSource = new RawVideoPipeSource(_frames)//(CreateFrames(64)
        {
            FrameRate = frameRate,
        };

    }

    public void AddFrame(string bitmapFile)
    {
        if (bitmapFile == null) throw new System.ArgumentNullException(nameof(bitmapFile));

        // Add frame to queue
        _bitmapFrames.Enqueue(bitmapFile);
    }

    public void AddFrame(Bitmap bitmap)
    {
        if (bitmap == null) throw new System.ArgumentNullException(nameof(bitmap));
        if (_videoFramesSource == null) throw new System.InvalidOperationException("Call VideoEncoder(...) before AddFrame().");
        var videoFrame = new BitmapVideoFrameWrapper(bitmap);
        // Add frame to queue
        _frames.Enqueue(videoFrame);
    }


    public void AddFrame(byte[] frameData, int width, int height, int stride)
    {
        if (frameData == null) throw new System.ArgumentNullException(nameof(frameData));
        if (_videoFramesSource == null) throw new System.InvalidOperationException("Call VideoEncoder(...) before AddFrame().");

        //Build a Bitmap from raw 32bpp data honoring the provided stride
        var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        var rect = new Rectangle(0, 0, width, height);
        var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        try
        {
            int destStride = data.Stride;
            int rowBytes = width * 4; // 32bpp

            for (int y = 0; y < height; y++)
            {
                int srcOffset = y * stride;
                var destPtr = System.IntPtr.Add(data.Scan0, y * destStride);
                System.Runtime.InteropServices.Marshal.Copy(frameData, srcOffset, destPtr, rowBytes);
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }


        var videoFrame = new BitmapVideoFrameWrapper(bmp);
        //// Add frame to queue
        //var videoFrame = new BitmapVideoFrameWrapper(new Bitmap(
        //    new MemoryStream(frameData)));
        _frames.Enqueue(videoFrame);

    }

    public async Task FinalizeAsync()////IEnumerable<BitmapVideoFrameWrapper> frames)
    {
        if (_videoFramesSource == null) throw new System.InvalidOperationException("Call VideoEncoder(...) before FinalizeAsync().");

        await DownloadFFMPEG();

        //RawVideoPipeSource source = new RawVideoPipeSource(frames) { FrameRate = 30 };
        FFMpegArguments
           .FromPipeInput(_videoFramesSource)
           .OutputToFile(_outputPath, overwrite: true, options => options.WithVideoCodec("libx264"))
           .ProcessSynchronously();

        //await FFMpegArguments
        //    .FromPipeInput(_videoFramesSource)
        //    .OutputToFile(_outputPath, overwrite: true, o => o
        //        .WithVideoCodec(FFMpegCore.Enums.VideoCodec.LibX264)
        //        .WithConstantRateFactor(21)          // quality: 18–23 typical
        //        .WithFramerate(_frameRate)
        //        .WithCustomArgument("-pix_fmt bgra")  // MP4/H.264 compatibility
        //        .WithCustomArgument("-f mp4 -movflags")) // better playback start
        //    .ProcessAsynchronously();


    }


    public void Finalize(bool fileSource)//IEnumerable<string> sources)
    {
        _ = DownloadFFMPEG();


        FFMpegArguments
            .FromFileInput(inputPattern, verifyExists: false)
            .OutputToFile(_outputPath, overwrite: true, options => options
                .WithVideoCodec("libx264")
                .WithCustomArgument("-pix_fmt yuv420p")
                .WithFramerate(_frameRate))
            .ProcessSynchronously();

    }

    private async Task DownloadFFMPEG()
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
    }

    public void Dispose()
    {
        _frames.Clear();
        _bitmapFrames.Clear();
    }



}

