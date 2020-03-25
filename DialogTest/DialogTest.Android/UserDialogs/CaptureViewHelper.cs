using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Renderscripts;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;

namespace UserDialogs
{
    public class CaptureViewHelper
    {
        public static Bitmap CaptureWindow(float scale)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            var decorView = activity.Window.DecorView;
            decorView.DrawingCacheEnabled = true;
            decorView.BuildDrawingCache();
            Bitmap tempCapture = decorView.DrawingCache;
            Rect frame = new Rect();
            activity.Window.DecorView.GetWindowVisibleDisplayFrame(frame);
            int statusBarHeight = frame.Top;

            DisplayMetrics displaymetrics = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(displaymetrics);

            int width = displaymetrics.WidthPixels;
            int height = displaymetrics.HeightPixels;
            Matrix matrix = new Matrix();
            matrix.SetScale(scale, scale);
            Bitmap capture = Bitmap.CreateBitmap(tempCapture, 0, statusBarHeight, width, height
                , matrix, true);
            decorView.DrawingCacheEnabled = false;
            decorView.DestroyDrawingCache();
            return capture;
        }

        /**
     * 截取当前窗体的截图，根据[isShowStatusBar]判断是否包含当前窗体的状态栏
     * 原理是获取当前窗体decorView的缓存生成图片
     */
        public static Bitmap CaptureWindow(bool isShowStatusBar = true, float scale = 1)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            // 获取当前窗体的View对象
            var view = activity.Window.DecorView;
            view.DrawingCacheEnabled = true;
            // 生成缓存
            view.BuildDrawingCache();

            Bitmap capture = null;
            Matrix matrix = new Matrix();
            matrix.SetScale(scale, scale);
            if (isShowStatusBar)
            {
                DisplayMetrics metric = new DisplayMetrics();
                var display = activity.WindowManager.DefaultDisplay;
                display.GetRealMetrics(metric);
                int width = metric.WidthPixels; // 宽度（PX）
                int height = metric.HeightPixels; // 高度（PX）

                // 绘制整个窗体，包括状态栏
                capture = Bitmap.CreateBitmap(view.DrawingCache, 0, 0, width,
                    height, matrix, true);
            }
            else
            {
                // 获取状态栏高度
                var rect = new Rect();
                view.GetWindowVisibleDisplayFrame(rect);
                var display = activity.WindowManager.DefaultDisplay;
                Point point = new Point();
                display.GetSize(point);
                // 减去状态栏高度
                capture = Bitmap.CreateBitmap(view.DrawingCache, 0,
                rect.Top, point.X, point.Y - rect.Top, matrix, true);
            }

            view.DrawingCacheEnabled = false;
            view.DestroyDrawingCache();

            return capture;
        }

        public static Bitmap MartixBitmap(Bitmap sentBitmap)
        {
            Matrix matrix = new Matrix();
            matrix.SetScale(0.2f, 0.2f);
            var newBit = Bitmap.CreateBitmap(sentBitmap, 0, 0, sentBitmap.Width, sentBitmap.Height, matrix, true);
            return newBit;
        }


        public static Bitmap RsBlur(Context context, Bitmap source, int radius)
        {

            Bitmap inputBmp = source;
            //(1)
            RenderScript renderScript = RenderScript.Create(context);

            // Allocate memory for Renderscript to work with
            //(2)
            Allocation input = Allocation.CreateFromBitmap(renderScript, inputBmp);
            Allocation output = Allocation.CreateTyped(renderScript, input.Type);
            //(3)
            // Load up an instance of the specific script that we want to use.
            ScriptIntrinsicBlur scriptIntrinsicBlur = ScriptIntrinsicBlur
                .Create(renderScript, Element.U8_4(renderScript));
            //(4)
            scriptIntrinsicBlur.SetInput(input);
            //(5)
            // Set the blur radius
            scriptIntrinsicBlur.SetRadius(radius);
            //(6)
            // Start the ScriptIntrinisicBlur
            scriptIntrinsicBlur.ForEach(output);
            //(7)
            // Copy the output to the blurred bitmap
            output.CopyTo(inputBmp);
            //(8)
            renderScript.Destroy();

            return inputBmp;
        }

        public static Bitmap FastBlur(Bitmap sentBitmap, int radius)
        {
            Bitmap bitmap = sentBitmap.Copy(sentBitmap.GetConfig(), true);

            if (radius < 1)
            {
                return (null);
            }

            int w = bitmap.Width;
            int h = bitmap.Height;

            int[] pix = new int[w * h];

            bitmap.GetPixels(pix, 0, w, 0, 0, w, h);

            int wm = w - 1;
            int hm = h - 1;
            int wh = w * h;
            int div = radius + radius + 1;

            int[] r = new int[wh];
            int[] g = new int[wh];
            int[] b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p, yp, yi, yw;
            int[] vmin = new int[Math.Max(w, h)];

            int divsum = (div + 1) >> 1;
            divsum *= divsum;
            int[] dv = new int[256 * divsum];
            for (i = 0; i < 256 * divsum; i++)
            {
                dv[i] = (i / divsum);
            }

            yw = yi = 0;

            int[][] stack = new int[div][];

            for (int item = 0; item < div; item++)
            {
                stack[item] = new int[3];
            }

            int stackpointer;
            int stackstart;
            int[] sir;
            int rbs;
            int r1 = radius + 1;
            int routsum, goutsum, boutsum;
            int rinsum, ginsum, binsum;

            for (y = 0; y < h; y++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    p = pix[yi + Math.Min(wm, Math.Max(i, 0))];
                    sir = stack[i + radius];
                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = (p & 0x0000ff);
                    rbs = r1 - Math.Abs(i);
                    rsum += sir[0] * rbs;
                    gsum += sir[1] * rbs;
                    bsum += sir[2] * rbs;
                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }
                }
                stackpointer = radius;

                for (x = 0; x < w; x++)
                {

                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];

                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;

                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];

                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];

                    if (y == 0)
                    {
                        vmin[x] = Math.Min(x + radius + 1, wm);
                    }
                    p = pix[yw + vmin[x]];

                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = (p & 0x0000ff);

                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];

                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;

                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[(stackpointer) % div];

                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];

                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];

                    yi++;
                }
                yw += w;
            }
            for (x = 0; x < w; x++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = Math.Max(0, yp) + x;

                    sir = stack[i + radius];

                    sir[0] = r[yi];
                    sir[1] = g[yi];
                    sir[2] = b[yi];

                    rbs = r1 - Math.Abs(i);

                    rsum += r[yi] * rbs;
                    gsum += g[yi] * rbs;
                    bsum += b[yi] * rbs;

                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }

                    if (i < hm)
                    {
                        yp += w;
                    }
                }
                yi = x;
                stackpointer = radius;
                for (y = 0; y < h; y++)
                {
                    pix[yi] = (int)((0xff000000 & pix[yi]) | (dv[rsum] << 16) | (dv[gsum] << 8) | dv[bsum]);

                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;

                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];

                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];

                    if (x == 0)
                    {
                        vmin[y] = Math.Min(y + r1, hm) * w;
                    }
                    p = x + vmin[y];

                    sir[0] = r[p];
                    sir[1] = g[p];
                    sir[2] = b[p];

                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];

                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;

                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[stackpointer];

                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];

                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];

                    yi += w;
                }
            }

            bitmap.SetPixels(pix, 0, w, 0, 0, w, h);

            return (bitmap);
        }
    }
}