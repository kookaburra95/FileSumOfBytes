using System;

namespace FileSumOfBytes.Logic.Model
{
    /// <summary>
    /// File information.
    /// </summary>
    public class FileHashSum
    {
        /// <summary>
        /// File name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// File Hash sum.
        /// </summary>
        public string HashSum { get; }

        /// <summary>
        /// Create new File information.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="hashSum">File Hash sum.</param>
        public FileHashSum(string name, string hashSum)
        {
            Name = name;
            HashSum = hashSum;
        }
    }
}
