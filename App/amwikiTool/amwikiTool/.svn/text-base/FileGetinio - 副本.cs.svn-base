using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Windows.Forms;

using System.Text.RegularExpressions;

namespace amwikiTool
{
    class FileGetinio
    {
        private static FileStream fs;
        private static StreamWriter sw;

        

        public  void MainTest()
        {
            string path;
            int leval = 0;

            Console.WriteLine("请输入需要列出内容的文件夹的完整路径和文件名：");


            //App路径在    E:\amwikiTool\App\amwikiTool\amwikiTool\bin\Debug
            //源md文件在   E:\amwikiTool\library  
            //备份的md     E:\amwikiTool\libbak

            string AppPath = @"E:\amwikiTool\App\amwikiTool\amwikiTool\bin\Debug";
            string MdLibrary = @"E:\amwikiTool\library";
            string MdLibBak = @"E:\amwikiTool\libbak";

            //复制操作
            string MdSrc = @"E:\amwikiTool\LibSrc";
            string MdSrcto = @"E:\amwikiTool\library";


            //0.删除原来的library文件
            if (System.IO.Directory.Exists(MdLibrary)) //如果文件夹存在
            {
                try
                {
                    System.IO.Directory.Delete(MdLibrary, true);
                }
                catch
                {
                    MessageBox.Show("library文件夹被占用,无法删除");
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
            }
            

            //2.生成library的导航文件

            
            //3. 处理library的md文件

            //4.


            return;




            path = "E:/amwikiTool/library";//Console.ReadLine();
            path.Replace('\\', '/');

            fs = new FileStream("result.txt", FileMode.Create);
            sw = new StreamWriter(fs);

            //开始写入文件
            //固定格式,先空一行
            sw.WriteLine("");
            sw.WriteLine("#### [首页](?file=home-首页)");


            //Cutpath("aa");
            //Cutref("aa");
            //assertpathget("a");

            MakeNavigation(path, leval);


            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();

            //cp文件
            string sourceFile = @"result.txt";
            string destinationFile = @"E:\amwikiTool\library\$navigation.md";
            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            System.IO.File.Copy(sourceFile, destinationFile, isrewrite);


            Console.WriteLine("请按任意键继续……");
            //Console.ReadKey();
        }

        /// <summary>
        /// 列出path路径对应的文件夹中的子文件夹和文件
        /// 然后再递归列出子文件夹内的文件和文件夹
        /// </summary>
        /// <param name="path">需要列出内容的文件夹的路径</param>
        /// <param name="leval">当前递归层级，用于控制输出前导空格的数量</param>

        public static void listDirectory(string path, int leval)
        {
            DirectoryInfo theFolder = new DirectoryInfo(@path);
            leval++;
            foreach (System.IO.FileSystemInfo NextF in theFolder.GetFileSystemInfos())
            {

                if ((NextF.Name[0] == '[') || NextF.Name.Contains(".assets"))
                {
                    continue;
                }
                if (NextF is DirectoryInfo)
                {
                    for (int i = 0; i < leval; i++) sw.Write('\t');
                    sw.Write("--)");
                    sw.WriteLine(NextF.Name);
                    listDirectory(NextF.FullName, leval);
                }
                else
                {
                    for (int i = 0; i < leval; i++) sw.Write('\t');
                    sw.Write("-->");
                    sw.WriteLine(NextF.Name);
                }
            }

        }

        //生成导航文件
        public static void MakeNavigation(string path, int leval)
        {
            DirectoryInfo theFolder = new DirectoryInfo(@path);
            leval++;
            foreach (System.IO.FileSystemInfo NextF in theFolder.GetFileSystemInfos())
            {

                if ((NextF.Name[0] == '[') || NextF.Name.Contains(".assets"))
                {
                    continue;
                }
                if (NextF is DirectoryInfo)
                {
                    if (leval == 1)
                    {
                        sw.WriteLine("");
                        sw.Write("##### ");
                        sw.WriteLine(Cutpath(NextF.Name));
                    }
                    else //>=2
                    {
                        for (int i = 0; i < leval-2; i++) sw.Write(' ');
                        sw.WriteLine("- **" + Cutpath(NextF.Name) + "**");
                    }
                    MakeNavigation(NextF.FullName, leval);
                }
                else
                {
                    for (int i = 0; i < leval-1; i++) sw.Write(' ');
                    sw.Write("- [");
                    sw.Write(Cutpath(NextF.Name));
                    sw.Write("](?file=");
                    string fpath = Cutref(NextF.FullName);
                    // MessageBox.Show(NextF.FullName); ,路径是 用\ 表示的win系统
                    sw.Write(fpath+" ");
                    sw.Write("\""+Cutpath(NextF.Name)+"\"");
                    sw.WriteLine(")");
                    assertpathget(NextF.FullName);
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
                
                if ( (i >= '0' && i <= '9') || (i=='_') ||(i=='-') )
                {
                    cnt++;    
                }
                else
                {
                    break;
                }
            }
            //去除后面的.md 后缀
            if (fullpath.Length > 3 + cnt)
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
            string regexStr=@"(\S*?)(library\\)(\S*)(.md\b)";
            Regex reg = new Regex(regexStr);
            Match match=reg.Match(input);

            if (match.Success)
            {
                string value = match.Groups[3].Value;
                Console.WriteLine(value);
                return value;
            }
            else
            {
                Console.WriteLine("over");
                return fullpath;
            }
        }



        //正则处理md文件中的图片链接
        public static string assertpathget(string mdpath)
        {
            string s;

            //去除到library层级的路径
            //文件路径为      E:\amwikiTool\library\002-测试2\LCD之程序框架.assets
            //图片路径为      E:\amwikiTool\library\002-测试2\LCD之程序框架.assets
            //实际引用        LCD之程序框架.assets/LCD程序.png
            //wiki引用        library/002-测试2/LCD之程序框架.assets/LCD程序.png

            //总结,在原来路径上加上 library\002-测试2\...文件自身的路径 
            //也就是去除 文件自身路径前的 library部分,


            //路径统一使用 \
            //Str=Str.Replace("/","\\")



            string mdfullpath = mdpath;//@"E:\amwikiTool\library\002-测试2\08-LCD之程序框架.md";
            //string pngpath = @"![LCD程序](LCD之程序框架.assets/LCD程序.png)";


            string oldstr = @"/";
            string newstr = @"\";  //统一使用这个符号

            mdfullpath = mdfullpath.Replace(oldstr, newstr);


            //获取相对路径
            string mdRelativePath = "";
            string regexStr=@"(\S*?)(library\\)(\S*)(\\.+.md\b)";
            Regex reg = new Regex(regexStr);
            Match match = reg.Match(mdfullpath);
            if (match.Success)
            {
                mdRelativePath = match.Groups[2].Value + match.Groups[3].Value + "/";
            }
            else
            {
                return "";
            }
            Console.WriteLine("相对路径="+mdRelativePath);

            //创建处理修改图片文件
            StreamWriter swmd;
            FileStream fsmd;

            //备份文件
            string sourceFile = mdfullpath;
            string destinationFile = mdfullpath.Replace("library", "libbak"); ;
            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            //得到文件的路径,如果备份的文件夹不存在,则去创建目录
            string pathme = Path.GetDirectoryName(destinationFile);
            if (!System.IO.Directory.Exists(pathme))
            {
                System.IO.Directory.CreateDirectory(pathme);//不存在就创建目录   
            }   
            System.IO.File.Copy(sourceFile, destinationFile, isrewrite);


            fsmd = new FileStream("md.txt", FileMode.Create);
            swmd = new StreamWriter(fsmd);

            StreamReader sr = new StreamReader(mdfullpath);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                //Console.WriteLine("xml template:" + line);
                regexStr = @"(\S*)(\!\[)(.+)(\]\()(.+)(\))";
                reg = new Regex(regexStr);
                match = reg.Match(line);
                if (line.Contains("library")) //已经替换过了,不在处理
                { 
                
                }
                else if (match.Success )
                {
                    string val = match.Groups[1].Value + match.Groups[2].Value + match.Groups[3].Value+ match.Groups[4].Value;
                    val = val + mdRelativePath + match.Groups[5].Value + match.Groups[6].Value;
                    val = val.Replace(newstr, oldstr);
                    line = val;
                    Console.WriteLine(val);
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
            return "";      
        }

        // 复制文件夹,保留源文件夹的名字
        public static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }

                    try
                    {
                        File.Copy(file, srcfileName, true);
                    }
                    catch
                    { 
                    }
                    
                   
                }
            }//foreach 
        }//functio


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
