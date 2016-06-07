using MjpegProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.IO;

/// <summary>
/// This serves as an exaple of how to grab streaming video from a camera.
/// Currently set up to work with the Android IP Webcam app: https://play.google.com/store/apps/details?id=com.pas.webcam&hl=en
/// 
/// Currently this only works with mpeg streams. See opencv (or emgucv) for using rtsp streams
/// Make sure the stream is already running before hitting the start button
/// </summary>
namespace VideoStreamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MjpegDecoder _mjpeg;

        /// <summary>
        /// Change this address based on the address of the web camera
        /// </summary>
        private static readonly String address = "http://192.168.1.5:8080";

        public MainWindow()
        {
            InitializeComponent();

            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady;

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _mjpeg.ParseStream(new Uri(address + "/video"));
        }

        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            image.Source = e.BitmapImage;
        }

        private async void ZoomSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            ZoomLabel.Content = "Zoom: " + ZoomSlider.Value;
            ZoomSlider.IsEnabled = false;

            //http://128.105.33.94:8080/ptz?zoom=10
            // Create a new webrequest to the mentioned URL.   
            WebRequest myWebRequest = WebRequest.Create(address + "/ptz?zoom=" + ZoomSlider.Value);
            //myWebRequest.BeginGetResponse(null, null);
            await myWebRequest.GetResponseAsync();


            myWebRequest.Abort();
            ZoomSlider.IsEnabled = true;


            // Create a new instance of the RequestState.
            //RequestState myRequestState = new RequestState();
            // The 'WebRequest' object is associated to the 'RequestState' object.
            //myRequestState.request = myWebRequest;
            // Start the Asynchronous call for response.
            //IAsyncResult asyncResult = (IAsyncResult)myWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
        }

        /*
        **** NOT USED ****
        private static void RespCallback(IAsyncResult asynchronousResult)
{
    try
    {
        // Set the State of request to asynchronous.
        RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
        WebRequest myWebRequest1 = myRequestState.request;
        // End the Asynchronous response.
        myRequestState.response = myWebRequest1.EndGetResponse(asynchronousResult);
        // Read the response into a 'Stream' object.
        Stream responseStream = myRequestState.response.GetResponseStream();
        myRequestState.responseStream = responseStream;
        // Begin the reading of the contents of the HTML page and print it to the console.
        IAsyncResult asynchronousResultRead = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);

    }
    catch (WebException e)
    {
        Console.WriteLine("WebException raised!");
        Console.WriteLine("\n{0}", e.Message);
        Console.WriteLine("\n{0}", e.Status);
    }
    catch (Exception e)
    {
        Console.WriteLine("Exception raised!");
        Console.WriteLine("Source : " + e.Source);
        Console.WriteLine("Message : " + e.Message);
    }
}
const int BUFFER_SIZE = 1024;
private static void ReadCallBack(IAsyncResult asyncResult)
{
    try
    {
        // Result state is set to AsyncState.
        RequestState myRequestState = (RequestState)asyncResult.AsyncState;
        Stream responseStream = myRequestState.responseStream;
        int read = responseStream.EndRead(asyncResult);
        // Read the contents of the HTML page and then print to the console.
        if (read > 0)
        {
            myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.bufferRead, 0, read));
            IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
        }
        else
        {
            Console.WriteLine("\nThe HTML page Contents are:  ");
            if (myRequestState.requestData.Length > 1)
            {
                string sringContent;
                sringContent = myRequestState.requestData.ToString();
                Console.WriteLine(sringContent);
            }
            Console.WriteLine("\nPress 'Enter' key to continue........");
            responseStream.Close();
            //allDone.Set();
        }
    }
    catch (WebException e)
    {
        Console.WriteLine("WebException raised!");
        Console.WriteLine("\n{0}", e.Message);
        Console.WriteLine("\n{0}", e.Status);
    }
    catch (Exception e)
    {
        Console.WriteLine("Exception raised!");
        Console.WriteLine("Source : {0}", e.Source);
        Console.WriteLine("Message : {0}", e.Message);
    }

}*/
    }
}

/*public class RequestState
{
    // This class stores the state of the request.
    const int BUFFER_SIZE = 1024;
    public StringBuilder requestData;
    public byte[] bufferRead;
    public WebRequest request;
    public WebResponse response;
    public Stream responseStream;
    public RequestState()
    {
        bufferRead = new byte[BUFFER_SIZE];
        requestData = new StringBuilder("");
        request = null;
        responseStream = null;
    }
}*/