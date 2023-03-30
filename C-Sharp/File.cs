using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace MyFilePackager
{
    public class Packager
    {
        // 打包文件
        public static void Pack(string[] files, string packFile)
        {
            using (FileStream output = new FileStream(packFile, FileMode.Create))
            {
                // 写入标识信息
                byte[] header = System.Text.Encoding.UTF8.GetBytes("MYPACK");
                output.Write(header, 0, header.Length);

                // 写入子文件数量
                BinaryWriter writer = new BinaryWriter(output);
                writer.Write(files.Length);

                // 依次写入每个子文件的名称和大小
                List<long> fileSizes = new List<long>();
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string fileDir = Path.GetDirectoryName(file);
                    writer.Write(fileName);
                    writer.Write(fileDir);

                    long fileSize = new FileInfo(file).Length;
                    fileSizes.Add(fileSize);
                    writer.Write(fileSize);
                }

                // 写入子文件内容
                foreach (string file in files)
                {
                    using (FileStream input = new FileStream(file, FileMode.Open))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                        }
                    }
                }

                // 压缩文件
                output.Position = 0;
                using (FileStream compressed = new FileStream(packFile + ".gz", FileMode.Create))
                {
                    using (GZipStream gzip = new GZipStream(compressed, CompressionMode.Compress))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = output.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            gzip.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }

        // 解包文件
        public static void Unpack(string packFile, string outputDir)
        {
            using (FileStream input = new FileStream(packFile + ".gz", FileMode.Open))
            {
                // 解压文件
                using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress))
                {
                    // 验证标识信息
                    byte[] header = new byte[6];
                    gzip.Read(header, 0, header.Length);
                    if (System.Text.Encoding.UTF8.GetString(header) != "MYPACK")
                    {
                        throw new Exception("Invalid pack file!");
                    }

                    // 读取子文件数量
                    BinaryReader reader = new BinaryReader(gzip);
                    int fileCount = reader.ReadInt32();

                    // 依次读取每个子文件的名称、路径和大小
                    for (int i = 0; i < fileCount; i++)
                    {
                        string fileName = reader.ReadString();
                        string fileDir = reader.ReadString();
                        long fileSize = reader.ReadInt64();

                        // 创建目录
                        string outputPath = Path.Combine(outputDir, fileDir);
                        Directory.CreateDirectory(outputPath);

                        // 读取文件内容
                        using (FileStream output = new FileStream(Path.Combine(outputPath, fileName), FileMode.Create))
                        {
