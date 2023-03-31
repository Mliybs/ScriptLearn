using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string directoryPath = @"C:\MyDirectory"; // 要打包的文件所在的目录路径
        string outputPath = @"C:\MyPackedFile.myext"; // 打包后的文件路径
        
        // 获取目录中的所有文件名
        string[] fileNames = Directory.GetFiles(directoryPath);
        
        // 创建二进制写入器，用于写入打包后的文件
        using (BinaryWriter writer = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
        {
            // 首先写入一个整型变量，表示有多少个子文件
            writer.Write(fileNames.Length);
            
            // 逐个写入每个文件名
            foreach (string fileName in fileNames)
            {
                writer.Write(fileName);
            }
            
            // 逐个将每个文件的内容按顺序写入进去
            foreach (string fileName in fileNames)
            {
                byte[] fileContents = File.ReadAllBytes(fileName);
                writer.Write(fileContents);
            }
        }
    }
}
