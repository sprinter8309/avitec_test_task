using TestAvitecTask.Services;
using TestAvitecTask.Data.Dto;

namespace TestAvitecTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationParamsDto UtilityParams = new ApplicationParamsDto();

            InputService.GetUtilityOptions(UtilityParams, args);

            InputService.GetUtilityParams(UtilityParams);

            CopyService.CopyFiles(UtilityParams);

            FinalActionService.FinalActions(UtilityParams);       
        }
    }
}
