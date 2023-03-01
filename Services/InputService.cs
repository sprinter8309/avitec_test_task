using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestAvitecTask.Data.Dto;

namespace TestAvitecTask.Services
{
    internal class InputService
    {
        public static void GetUtilityOptionsDirectInput(ApplicationParamsDto UtilityParams)
        {
            Console.WriteLine("Введите количество потоков копирования:");
            UtilityParams.CopyThreadsQuantity = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Требуется ли удалять файлы после копирования (yes/no, по-умолчанию no):");
            string ResponseRemoveBuf = Console.ReadLine();
            if (ResponseRemoveBuf.Trim().ToLower() == "yes") {
                UtilityParams.IsDeleteSourceFiles = true;
            }

            Console.WriteLine("Требуется ли выводить список скопированных файлов (yes/no, по-умолчанию no):");
            string ResponseShowBuf = Console.ReadLine();
            if (ResponseShowBuf.Trim().ToLower() == "yes")
            {
                UtilityParams.IsShowCopiedFiles = true;
            }

            Console.WriteLine("Требуется ли посчитать объем скопированных файлов (yes/no, по-умолчанию no):");
            string ResponseCalculateBuf = Console.ReadLine();
            if (ResponseCalculateBuf.Trim().ToLower() == "yes")
            {
                UtilityParams.IsTotalSizeMustBeCalculated = true;
            }
        }

        public static void GetUtilityParams(ApplicationParamsDto UtilityParams)
        {
            Console.WriteLine("Введите путь к исходной папку (source):");
            UtilityParams.SourcePath = Console.ReadLine();

            Console.WriteLine("Введите путь к папке назначения (target):");
            UtilityParams.TargetPath = Console.ReadLine();

            Console.WriteLine("Введите параметр задержки перед копированиями (period, в миллисекундах):");
            UtilityParams.Period = Convert.ToInt64(Console.ReadLine());
        }
    }
}
