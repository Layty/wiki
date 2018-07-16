using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Windows.Forms;  

using System.Text.RegularExpressions;

using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        private static FileStream fs;
        private static StreamWriter sw;


        static void Main(string[] args)
        {

            int leval = 0;



            //string strInput="";// = Console.ReadLine();
            //Process p = new Process();
            ////设置要启动的应用程序
            //p.StartInfo.FileName = "cmd.exe";
            ////是否使用操作系统shell启动
            //p.StartInfo.UseShellExecute = false;
            //// 接受来自调用程序的输入信息
            //p.StartInfo.RedirectStandardInput = false;
            ////输出信息
            //p.StartInfo.RedirectStandardOutput = false;
            //// 输出错误
            //p.StartInfo.RedirectStandardError = false;
            ////不显示程序窗口
            //p.StartInfo.CreateNoWindow = false;
            ////启动程序
            //p.Start();



            Console.WriteLine("开始处理amwiki文件,如果不清楚流程,请先备份,exe放和library同级的目录");


            //App路径在    E:\amwikiTool\App\amwikiTool\amwikiTool\bin\Debug
            //源md文件在   E:\amwikiTool\library  
            //备份的md     E:\amwikiTool\libbak


            //Path.GetDirectoryName(destinationFile);


            //string AppPath = @"E:\amwikiTool\App\amwikiTool\amwikiTool\bin\Debug";

            string MdLibrary = @"library";//@"E:\amwikiTool\library";
            string navigationMd = MdLibrary + @"\$navigation.md";

            //复制操作
            string MdSrc = @"LibSrc";
            string MdSrcto = MdLibrary;

            if (!System.IO.Directory.Exists(MdSrc))
            {
                string msg = @"第一次执行,程序会将 library 拷贝为 LibSrc ,后续文章在 LibSrc中发布后再运行本程序不会有此提示,确定会生成,否则退出";

                if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    //这里是点击 "是"的执行代码
                    System.IO.Directory.CreateDirectory(MdSrc);
                    try
                    {
                        CopyFolder(MdSrcto, MdSrc);
                        //反向处理amwiki的md链接为正常的链接
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("文件夹操作权限出错,请检查\r\n" + ex.ToString());
                        return;
                    }
                }
                else
                {
                    //这里是点击"否" 的执行代码
                    return;

                }

            }


            //0.删除原来的library文件
            if (System.IO.Directory.Exists(MdLibrary)) //如果文件夹存在
            {
                try
                {
                    System.IO.Directory.Delete(MdLibrary, true);

                    //strInput = @"rd/s/q ";

                    //strInput = strInput + System.IO.Directory.GetCurrentDirectory() + "\\" + MdLibrary+"&exit";
                    ////向cmd窗口发送输入信息
                    //p.StandardInput.WriteLine(strInput);
                    //p.StandardInput.AutoFlush = true;
                    ////获取输出信息
                    //string strOuput = p.StandardOutput.ReadToEnd();
                    //p.WaitForExit();
                    //p.Close();
                }
                catch
                {
                    MessageBox.Show("library文件夹被占用,无法删除");
                }
                finally
                {
                    //p.Close();
                }
            }

            //1. 强制覆盖新的目录
            //CopyDirectory(MdSrc, MdLibrary+"../../");
            try
            {
                CopyFolder(MdSrc, MdSrcto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件夹操作权限出错,请检查\r\n" + ex.ToString());
                goto _Lable_over;
            }


            //2.生成library的导航文件,遍历的同时处理md文件
            string sourceFile = @"result.txt";
            fs = new FileStream(sourceFile, FileMode.Create);
            sw = new StreamWriter(fs);
            //开始写入导航文件
            //固定格式,先空一行
            sw.WriteLine("");
            sw.WriteLine("#### [首页](?file=home-首页)");
            sw.WriteLine("- [home-首页](?file=home-首页 \"home-首页\")");

            MakeNavigation(MdLibrary, -1);
            sw.Flush();//清空缓冲区
            sw.Close(); //关闭流
            fs.Close();
            //复制导航文件
            string destinationFile = navigationMd;
            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            System.IO.File.Copy(sourceFile, destinationFile, isrewrite);
            System.IO.File.Delete(sourceFile);


        _Lable_over:

            //4.处理文件流
            try
            {
                sw.Flush();//清空缓冲区
                sw.Close(); //关闭流
                fs.Close();
            }
            catch
            {

            }



            

            Console.WriteLine("请按任意键继续……");
            //string input = Console.ReadLine();//读取一串字符，直到用户按下回车。
        }

        //生成导航文件
        public static void MakeNavigation(string path, int leval)
        {
            DirectoryInfo theFolder = new DirectoryInfo(@path);
            leval++;
            foreach (System.IO.FileSystemInfo NextF in theFolder.GetFileSystemInfos())
            {

                if ((NextF.Name[0] == '[') || NextF.Name.Contains(".assets") || (NextF.Name[0] == '.'))
                {
                    continue;
                }
                if (NextF is DirectoryInfo)
                {
                    if (leval == 0)
                    {
                        sw.WriteLine("");
                        sw.Write("##### ");
                        sw.WriteLine(Cutpath(NextF.Name));
                    }
                    else //>=1
                    {
                        for (int i = 0; i < leval ; i++) sw.Write("  ");
                        sw.WriteLine("- **" + Cutpath(NextF.Name) + "**");
                    }
                    MakeNavigation(NextF.FullName, leval);
                }
                else
                {
                    //只列出md文件
                    if (NextF.Name.ToUpper().Contains(".MD"))
                    {
                        if (NextF.Name.ToUpper().Contains("HOME-首页"))
                        {

                        }
                        else 
                        {
                            for (int i = 0; i < leval; i++) sw.Write("  ");
                            sw.Write("- [");
                            sw.Write(Cutpath(NextF.Name));
                            sw.Write("](?file=");
                            string fpath = Cutref(NextF.FullName).Replace(" ","%20");
                            // MessageBox.Show(NextF.FullName); ,路径是 用\ 表示的win系统
                            sw.Write(fpath + " ");
                            sw.Write("\"" + Cutpath(NextF.Name) + "\"");
                            sw.WriteLine(")");
                            assertpathget(NextF.FullName);
                        }
                    }
                }
            }
            

        }

        //去除md后缀后数字序号
        public static string Cutpath(string fullpath)
        {
            string s = "";
            int cnt = 0;
            //去除开头的数字序号
            foreach (char i in fullpath)
            {

                if ((i >= '0' && i <= '9') || (i == '_') || (i == '-'))
                {
                    cnt++;
                }
                else
                {
                    break;
                }
            }
            //去除后面的.md 后缀
            if (fullpath.Length > 1 + cnt)
            {

                s = fullpath.Substring(fullpath.Length - 3, 3);
                //MessageBox.Show(s.ToUpper());
                if (s.ToUpper().Contains(".MD"))
                {
                    //MessageBox.Show(cnt.ToString());
                    //MessageBox.Show(fullpath.Length.ToString());
                    s = fullpath.Substring(cnt, fullpath.Length - cnt - 3);
                    //MessageBox.Show("s=" + s);
                }
                else
                {
                    s = fullpath.Substring(cnt, fullpath.Length - cnt);
                    //MessageBox.Show("没有后缀" + s);
                }
            }
            else
            {
                s = fullpath;
            }
            return s;
        }


        /// <summary>
        /// 正则处理目录导航的路径
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string Cutref(string fullpath)
        {
            string input = fullpath;//@"E:\amwikiTool\library\001-学习amWiki\01-amWiki轻文库简介.md";
            string regexStr = @"(.*?)(library\\)(.*)([.][mM][Dd]\b)";
            Regex reg = new Regex(regexStr);
            Match match = reg.Match(input);

            if (match.Success)
            {
                string value = match.Groups[3].Value;
                Console.WriteLine(value);
                return value;
            }
            else
            {
                Console.WriteLine("处理到一个错误路径:" + fullpath);
                return fullpath;
            }
        }


        //正则处理md文件中的图片链接 和 其他链接  这里不判断有没有!符号   ...[...](/)...
        public static string assertpathget(string mdpath)
        {

            //去除到library层级的路径
            //文件路径为      E:\amwikiTool\library\002-测试2\LCD之程序框架.assets
            //图片路径为      E:\amwikiTool\library\002-测试2\LCD之程序框架.assets
            //实际引用        LCD之程序框架.assets/LCD程序.png
            //wiki引用        library/002-测试2/LCD之程序框架.assets/LCD程序.png

            //总结,在原来路径上加上 library\002-测试2\...文件自身的路径 
            //也就是去除 文件自身路径前的 library部分,


            //路径统一使用 \
            //Str=Str.Replace("/","\\")

            Console.WriteLine("当前处理的md文件为:" + mdpath);

            string mdfullpath = mdpath;//@"E:\amwikiTool\library\002-测试2\08-LCD之程序框架.md";
            //string pngpath = @"![LCD程序](LCD之程序框架.assets/LCD程序.png)";


            string oldstr = @"/";
            string newstr = @"\";  //统一使用这个符号

            mdfullpath = mdfullpath.Replace(oldstr, newstr);

            Console.WriteLine("正则处理的路径:" + mdpath);

            //获取相对路径
            string mdRelativePath = "";
            string regexStr = @"(.*\\library\\)(.*)(\\.+.md)";
            Regex reg = new Regex(regexStr);
            Match match = reg.Match(mdfullpath);
            if (match.Success)
            {
                mdRelativePath = match.Groups[2].Value+"\\";
            }
            else
            {
                //return "";
            }
            Console.WriteLine("相对路径=" + mdRelativePath);

            //创建处理修改图片文件
            StreamWriter swmd;
            FileStream fsmd;

            string sourceFile;
            string destinationFile;
            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之

            fsmd = new FileStream("md.txt", FileMode.Create);
            swmd = new StreamWriter(fsmd);

            StreamReader sr = new StreamReader(mdfullpath);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                //Console.WriteLine("xml template:" + line);
                //regexStr = @"(\S*)(\!\[)(.+)(\]\()(.+)(\))";
                regexStr = @"(.*\[.*\]\()(.+)(\))";
                reg = new Regex(regexStr);
                match = reg.Match(line);
                if (match.Success)// 判断路径是否包含 一些字符
                {
                    if (line.Contains("assets") ||
                        line.Contains("assert") ||
                        (match.Groups[5].Value.Contains("[") && match.Groups[5].Value.Contains("]"))
                        )
                    {
                        int i = 0;
                        Console.WriteLine(i.ToString() + "=  " + match.Groups[i].Value);
                        i++;
                        Console.WriteLine(i.ToString() + "=  " + match.Groups[i].Value);
                        i++;
                        Console.WriteLine(i.ToString() + "=  " + match.Groups[i].Value);
                        i++;
                        Console.WriteLine(i.ToString() + "=  " + match.Groups[i].Value);
                        i++;
                        Console.WriteLine(i.ToString() + "=  " + match.Groups[i].Value);
                        i++;
                        Console.WriteLine(i.ToString() + "=  " + match.Groups[i].Value);
                        i++;
                        Console.WriteLine(i.ToString() + "=  " + match.Groups[i].Value);


                        string val = match.Groups[1].Value + "library\\" + mdRelativePath + match.Groups[2].Value + match.Groups[3].Value;
                        val = val.Replace(newstr, oldstr); 
                        Console.WriteLine("原来的链接为:"+line);
                        Console.WriteLine("修改后的链接:" + val);
                        line = val;
                    }
                }
                swmd.WriteLine(line);
            }
            if (sr != null) sr.Close();

            //清空缓冲区
            swmd.Flush();
            //关闭流
            swmd.Close();
            fsmd.Close();

            //重命名文件
            mdfullpath = mdfullpath.Replace("/", "\\");
            sourceFile = "md.txt";
            destinationFile = mdfullpath;
            isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            System.IO.File.Copy(sourceFile, destinationFile, isrewrite);
            System.IO.File.Delete(sourceFile);
            return "";
        }


     


        //// 复制文件夹,保留源文件夹的名字
        //public static void CopyDirectory(string srcdir, string desdir)
        //{
        //    string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

        //    string desfolderdir = desdir + "\\" + folderName;

        //    if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
        //    {
        //        desfolderdir = desdir + folderName;
        //    }
        //    string[] filenames = Directory.GetFileSystemEntries(srcdir);

        //    foreach (string file in filenames)// 遍历所有的文件和目录
        //    {
        //        if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
        //        {

        //            string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
        //            if (!Directory.Exists(currentdir))
        //            {
        //                Directory.CreateDirectory(currentdir);
        //            }

        //            CopyDirectory(file, desfolderdir);
        //        }

        //        else // 否则直接copy文件
        //        {
        //            string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

        //            srcfileName = desfolderdir + "\\" + srcfileName;


        //            if (!Directory.Exists(desfolderdir))
        //            {
        //                Directory.CreateDirectory(desfolderdir);
        //            }

        //            try
        //            {
        //                File.Copy(file, srcfileName, true);
        //            }
        //            catch
        //            { 
        //            }


        //        }
        //    }//foreach 
        //}//functio


        //复制文件夹的内容到目的文件夹,不保留源文件夹的名字
        public static void CopyFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("创建目标目录失败：" + ex.Message);
                    }
                }
                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    try
                    {
                        File.Copy(c, destFile, true);//覆盖模式
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("复制文件失败：" + ex.Message);
                    }

                });
                //获得源文件下所有目录文件
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //采用递归的方法实现
                    CopyFolder(c, destDir);
                });
            }
            else
            {
                throw new DirectoryNotFoundException("源目录不存在！");
            }
        }























    }
}
