using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAvitecTask.Data.Dto
{
    internal class ApplicationParamsDto
    {
        public string SourcePath { get; set; }

        public string TargetPath { get; set; }

        public long Period { get; set; }

        public int CopyThreadsQuantity { get; set; } = 1;

        public bool IsDeleteSourceFiles { get; set; } = false;

        public bool IsShowCopiedFiles { get; set; } = false;

        public bool IsTotalSizeMustBeCalculated { get; set; } = false;

        public List<string> CopyFilesList { get; set; } = null;

        public List<string> CopyFilesHeap { get; set; } = null;

        public bool IsCopyingStopped { get; set; } = false;
    }
}
