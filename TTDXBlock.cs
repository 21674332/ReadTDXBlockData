 
namespace ConsoleApp3
{
    using System;
    using System.Text;

    public class TTDXBlockHeader
    {
        public string szVersion = new string(' ', 64); // 使用空格填充默认值
        public int nIndexOffset;
        public int nDataOffset;
        public int nData1;
        public int nData2;
        public int nData3;
    }
     
     

    public class TTDXBlockInfo
    {
        public string Name { get; set; }
        public int StockCount { get; set; }
        public string Category { get; set; }
        public List<string> StockCodes { get; set; }
    }

}
