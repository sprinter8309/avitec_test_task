using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAvitecTask.Data.Dto;

namespace TestAvitecTask.Services
{
    internal class FinalActionService
    {
        public static void FinalActions(ApplicationParamsDto UtilityParams)
        {
            if (!UtilityParams.IsCopyingStopped)
            {
                ShowCopiedFiles(UtilityParams);

                CalculateTotalSize(UtilityParams);

                DeleteSourceFiles(UtilityParams);
            }
        }

        private static void ShowCopiedFiles(ApplicationParamsDto UtilityParams)
        {
            if (UtilityParams.IsShowCopiedFiles && UtilityParams.CopyFilesList.Count > 0) 
            {               
                Console.WriteLine("Список скопированных файлов:");

                foreach (string file in UtilityParams.CopyFilesList)
                {
                    Console.WriteLine(file);
                }               
            }
        }

        private static void CalculateTotalSize(ApplicationParamsDto UtilityParams)
        {
            if (UtilityParams.IsTotalSizeMustBeCalculated && UtilityParams.CopyFilesList.Count > 0)
            {
                DirectoryInfo SrcDirInfo = new DirectoryInfo(UtilityParams.SourcePath);
                FileInfo[] SourceFilesInfo = SrcDirInfo.GetFiles();

                long TotalSizeCopiedFiles = 0;

                foreach (FileInfo FInfo in SourceFilesInfo)
                {
                    if (Array.Exists(UtilityParams.CopyFilesList.ToArray(), element => element == FInfo.Name))
                    {
                        TotalSizeCopiedFiles += FInfo.Length;
                    }
                }

                Console.WriteLine("Общий объем скопированных файлов: " + TotalSizeCopiedFiles + " байт");
            }
        }

        private static void DeleteSourceFiles(ApplicationParamsDto UtilityParams)
        {
            if (UtilityParams.IsDeleteSourceFiles && UtilityParams.CopyFilesList.Count > 0)
            {
                foreach (string file in UtilityParams.CopyFilesList)
                {
                    File.Delete(Path.Combine(UtilityParams.SourcePath, file));
                }
            }
        }
    }
}
