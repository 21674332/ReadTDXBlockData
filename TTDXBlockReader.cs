using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    using System;
    using System.IO;
    using System.Text;
    using static System.Reflection.Metadata.BlobBuilder;

    public class TTDXBlockReader
    {
        public TTDXBlockHeader ReadBlockHeader(BinaryReader reader)
        {
            TTDXBlockHeader header = new TTDXBlockHeader
            {
                szVersion = Encoding.Default.GetString(reader.ReadBytes(64)).TrimEnd(),

            };

            var bytes = reader.ReadBytes(4);
            header.nIndexOffset = BitConverter.ToInt32(bytes);
            bytes = reader.ReadBytes(4);
            header.nDataOffset = BitConverter.ToInt32(bytes);
            header.nData1 = reader.ReadInt32();
            header.nData2 = reader.ReadInt32();
            header.nData3 = reader.ReadInt32();
            return header;
        }
 
     
        public List<TTDXBlockInfo> ParseBlockFile(string filePath)
        {
            List<TTDXBlockInfo> blocks = new List<TTDXBlockInfo>();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                TTDXBlockHeader header = ReadBlockHeader(reader);
                //var blockIndex  = ReadBlockIndex(reader);
                // 跳过索引部分，直接读取数据部分
                reader.BaseStream.Seek(header.nDataOffset, SeekOrigin.Begin);

                // 读取板块个数
                int blockCount = BitConverter.ToInt16(reader.ReadBytes(2), 0);

                // 这里需要根据实际的nData1, nData2, nData3来解析具体的数据结构
                // 以下代码仅为示例，实际解析需要根据文件结构来编写
                for (int i = 0; i < blockCount; i++)
                {
                    TTDXBlockInfo block = new TTDXBlockInfo();
                    block.Name = Encoding.GetEncoding("GB2312").GetString(reader.ReadBytes(9)).TrimEnd('\0');
                    block.StockCount = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                    block.Category = BitConverter.ToInt16(reader.ReadBytes(2), 0).ToString();

                    // 读取股票代码
                    block.StockCodes = new List<string>();
                    int stockCodeLength = 7; // 每个股票代码的长度
                    for (int j = 0; j < 400; j++) // 最多包含400只个股
                    {
                        //reader.BaseStream.Seek(fs.Position - stockCodeLength, SeekOrigin.Begin); // 回退到上一个股票代码的位置
                        string code = Encoding.Default.GetString(reader.ReadBytes(7)).TrimEnd('\0');
                        if (!string.IsNullOrEmpty(code))
                        {
                            block.StockCodes.Add(code);
                        }
                        
                        //reader.BaseStream.Seek(fs.Position + stockCodeLength, SeekOrigin.Begin); // 移动到下一个股票代码的位置
                    }

                    // 跳到下一个板块的数据起始位置
                    //reader.BaseStream.Seek(reader.BaseStream.Position + (2813 - (9 + 2 + 2 + 7 * 400)), SeekOrigin.Begin);

                    blocks.Add(block);
                }

                //// 读取板块记录
                //while (reader.BaseStream.Position < reader.BaseStream.Length)
                //    {
                //        TTDXBlockRecord record = ReadBlockRecord(reader);
                //        // 处理读取到的记录，例如打印或者存储
                //        Console.WriteLine($"Name: {record.szName}, Code: {record.szCode}");
                //    }
                //}
            }
            return blocks;
        }
    }

}
