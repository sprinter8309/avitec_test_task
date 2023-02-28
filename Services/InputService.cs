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
        public static void GetUtilityOptions(ApplicationParamsDto UtilityParams, string[] args)
        {
            if (Array.Exists(args, element => element=="--remove-after-copy")) {
                UtilityParams.IsDeleteSourceFiles = true;
            }

            if (Array.Exists(args, element => element == "--show-copied-files")) {
                UtilityParams.IsShowCopiedFiles = true;
            }

            if (Array.Exists(args, element => element == "--calculate-total-size")) {
                UtilityParams.IsTotalSizeMustBeCalculated = true;
            }

            Regex ThreadCountParam = new Regex(@"--threads-quantity=(\d+)");

            foreach (var param in args) {
                if (ThreadCountParam.IsMatch(param)) {

                    MatchCollection Matches = ThreadCountParam.Matches(param);
                    Group GrBuf = Matches[0].Groups[1];
                    string StrBuf = GrBuf.Captures[0].ToString();

                    UtilityParams.CopyThreadsQuantity = Convert.ToInt32(StrBuf);
                }
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
