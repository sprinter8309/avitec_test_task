using TestAvitecTask.Services;
using TestAvitecTask.Data.Dto;

namespace TestAvitecTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationParamsDto UtilityParams = new ApplicationParamsDto();

            InputService.GetUtilityParams(UtilityParams);

            InputService.GetUtilityOptionsDirectInput(UtilityParams);

            CopyService.CopyFiles(UtilityParams);

            FinalActionService.FinalActions(UtilityParams);       
        }
    }
}
