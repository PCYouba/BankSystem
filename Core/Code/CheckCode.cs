using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BankSystem.Code
{
    public static class CheckCode
    {

        #region RndCodeImg
        public static byte[] RndCodeImg()
        {
            int intLength = 5;               //长度
            string strIdentify = "rndcode"; //随机字串存储键值，以便存储到Session中
            Random RND = new Random();
            using (Bitmap bm = new Bitmap(200, 60))
            {
                using (Graphics gArea = Graphics.FromImage(bm))
                {
                    using (SolidBrush sBrush = new SolidBrush(Color.FromArgb(255, 0, 128)))
                    {
                        gArea.FillRectangle(sBrush, 0, 0, bm.Width, bm.Height);
                        Font font = new Font(FontFamily.GenericSerif, 48, FontStyle.Bold, GraphicsUnit.Pixel);                        
                        StringBuilder sBuilder = new StringBuilder();
                        //合法随机显示字符列表
                        string strLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                        //将随机生成的字符串绘制到图片上
                        for (int i = 0; i < intLength; i++)
                        {
                            string CurrChar = strLetters.Substring(RND.Next(0, strLetters.Length - 1), 1);
                            gArea.DrawString(CurrChar, font, new SolidBrush(Color.Blue), i * 38, RND.Next(0, 15));
                            sBuilder.Append(CurrChar); //保存输出的字符。
                        }
                        //生成干扰线条                        
                        for (int i = 0; i < 10; i++)
                        {
                            Color theC = Color.FromArgb(RND.Next(0, 255), RND.Next(0, 255), RND.Next(0, 255));
                            using (Pen pen = new Pen(new SolidBrush(theC), 2))
                            {
                                gArea.DrawLine(pen, new Point(RND.Next(0, bm.Width), RND.Next(0, bm.Height)), new Point(RND.Next(0, bm.Width), RND.Next(0, bm.Height)));
                            }
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bm.Save(ms, ImageFormat.Gif);
                            HttpContext.Current.Session[strIdentify] = sBuilder.ToString().ToLower(); //先保存在Session中(小写)，验证与用户输入是否一致。
                            return ms.ToArray();
                        }
                    }
                }
            }
        }
        #endregion

    }
}