using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Media3D;
using WpfProj.MVVM;
using static ZDSoft.SDK;

namespace WpfProj.ViewModel
{
    public class ScreenRecordVM : ViewModelBase
    {
        readonly string videoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ScreenRecorder", "record.mp4");

      
        public void InitializeRecording()
        {
            // Logic to stop screen recording

            ScnLib_InitializeW("");
            

            
        }      
           


       

        public void StartRecording(Window window)
        {

            // Logic to start screen recording         



            ScnLib_SetVideoPathW(videoPath);



            int left = (int)window.Left;
            int top = (int)window.Top;
            int right = left + (int)window.Width;
            int bottom = top + (int)window.Height;

            var frameWnd = ScnLib_GetCaptureRegionFrameWnd();
            ScnLib_SetCaptureWnd(frameWnd, true);

            ScnLib_MonitorVolumeLevel(true);

            ScnLib_ShowCaptureRegionFrame(true);

            //ScnLib_SetCaptureRegion(left,  top,  right,  bottom);



            // Fix for CS1510: create a variable to pass by ref            
            //ScnLib_SelectCaptureRegionW(ref left, ref top, ref right,ref  bottom, ref frameWnd, "my");

            ScnLib_RecordCursor(true);
            ScnLib_EnableVideoVariableFrameRate(true);
            ScnLib_StartRecording();

           

            //StringBuilder videoPath = new StringBuilder(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "record.mp4"));




        }

        public void StopRecording()
        {
            // Logic to stop screen recording            
            ScnLib_StopRecording();
           

        }
        public void UninitializeRecording()
        {
            // Logic to stop screen recording
            ScnLib_Uninitialize();
        }



    }
}
