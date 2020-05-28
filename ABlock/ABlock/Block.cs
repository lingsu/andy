using System;
using System.Collections.Generic;
using System.Text;

namespace ABlock
{
    public class Block
    {
        /// <summary>
        /// 时间戳，也就是这个区块的创建时间
        /// </summary>
        public DateTimeOffset time_stamp { get; set; }
        /// <summary>
        /// 数据，可以是任意类型，是我们要用区块链来保存的数据
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 前一个区块的hash值
        /// </summary>
        public string pre_hash { get; set; }
        /// <summary>
        /// 当前区块的hash值
        /// </summary>
        public string hash { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        public string nonce { get; set; }
    }
}
