using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static string sourcePath;
        static string destinationPath;
        static string destinationPathAsync;
        static double resizeRatio;
        static public ImageProcess imageProcess;

        static async Task Main(string[] args)
        {
            sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            destinationPath = Path.Combine(Environment.CurrentDirectory, "output");
            destinationPathAsync = Path.Combine(Environment.CurrentDirectory, "outputAsync");
            resizeRatio = 2.0;
            imageProcess = new ImageProcess();
            Console.WriteLine("執行中 請稍候");

            long originSpendingTime = CalculateSpendingTimeResize();
            long newSpendingTime = await CalculateSpendingTimeResizeAsync();

            ShowEfficenty(originSpendingTime, newSpendingTime);

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// 執行同步縮放並計算執行時間
        /// </summary>
        /// <returns>耗時(毫秒)</returns>
        static long CalculateSpendingTimeResize()
        {
            imageProcess.Clean(destinationPath);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            imageProcess.ResizeImages(sourcePath, destinationPath, resizeRatio);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// 執行非同步縮放並計算執行時間
        /// </summary>
        /// <returns>耗時(毫秒)</returns>
        static async Task<long> CalculateSpendingTimeResizeAsync()
        {
            imageProcess.Clean(destinationPathAsync);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            await imageProcess.ResizeImagesAsync(sourcePath, destinationPathAsync, resizeRatio);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// 計算效率
        /// </summary>
        /// <param name="oriTime">原本耗時</param>
        /// <param name="newTime">改良後耗時</param>
        static void ShowEfficenty(long oriTime, long newTime)
        {
            Console.WriteLine($"同步縮放花費時間: {oriTime} ms");
            Console.WriteLine($"非同步縮放花費時間: {newTime} ms");

            if (newTime > 0)
            {
                var eff = ((double)oriTime / (double)newTime - 1) * 100;
                Console.WriteLine($"非同步縮放效率提升了 {eff.ToString("f2")}%");
            }
        }
    }
}
