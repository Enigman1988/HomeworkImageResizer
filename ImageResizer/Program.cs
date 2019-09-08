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
        static long originSpendingTime = 0;
        static long newSpendingTime = 0;
        static double resizeRatio;

        static void Main(string[] args)
        {
            sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            destinationPath = Path.Combine(Environment.CurrentDirectory, "output");
            resizeRatio = 2.0;

            ImageProcess imageProcess = new ImageProcess();

            imageProcess.Clean(destinationPath);
            originSpendingTime = ExecuteResizeAndCalculateSpendingTime(new Action<string, string, double>(imageProcess.ResizeImages));

            imageProcess.Clean(destinationPath);
            newSpendingTime = ExecuteResizeAndCalculateSpendingTime(new Action<string, string, double>(imageProcess.ResizeImagesAsync));

            ShowEfficenty(originSpendingTime, newSpendingTime);

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
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

        /// <summary>
        /// 執行縮放並計算執行時間
        /// </summary>
        /// <param name="action">縮放方法</param>
        /// <returns></returns>
        static long ExecuteResizeAndCalculateSpendingTime(Action<string, string, double> action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            action(sourcePath, destinationPath, resizeRatio);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
