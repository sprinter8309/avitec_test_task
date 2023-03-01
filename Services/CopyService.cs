using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using TestAvitecTask.Data.Dto;

namespace TestAvitecTask.Services
{
    internal class CopyService
    {
        public static void CopyFileAction(object InputObject)
        {            
            Stopwatch WatchObject = new Stopwatch();

            if (InputObject is ApplicationParamsDto UtilityParams)
            {
                StringBuilder CopyFileName = new StringBuilder();

                // Взятие файла из общего пула файлов остающихся к копированию 
                lock (typeof(CopyService))
                {                    
                    if (UtilityParams.CopyFilesHeap.Count > 0)
                    {
                        CopyFileName.Append(UtilityParams.CopyFilesHeap.ElementAt(0));
                        UtilityParams.CopyFilesHeap.RemoveAt(0);
                    }
                }

                if (CopyFileName.Length > 0 && !UtilityParams.IsCopyingStopped)
                {
                    WatchObject.Start();

                    try {
                        File.Copy(Path.Combine(UtilityParams.SourcePath, CopyFileName.ToString()), Path.Combine(UtilityParams.TargetPath, CopyFileName.ToString()));
                    } catch (IOException CopyError) {
                        Console.WriteLine(CopyError.Message);  
                    }

                    WatchObject.Stop();

                    if (WatchObject.ElapsedMilliseconds > UtilityParams.Period)
                    {
                        CopyFileAction(UtilityParams);
                    }
                    else
                    {
                        Thread.Sleep((int)(UtilityParams.Period - WatchObject.ElapsedMilliseconds));
                        CopyFileAction(UtilityParams);
                    }
                }
            }            
        }

        public static void ControlCopyProcess(List<Thread> CopyThreadsPool, ApplicationParamsDto UtilityParams, Thread StopControlThread)
        {
            bool IsThreadsWorking = true;

            Console.WriteLine("Начало копирования");
            Console.WriteLine();
            Console.WriteLine("! Вы можете прервать выполнение копирования, нажав любую клавишу:");

            while (IsThreadsWorking)
            {
                IsThreadsWorking = false;

                foreach (Thread ThElement in CopyThreadsPool) {
                    if (ThElement.IsAlive) {
                        IsThreadsWorking = true;
                    }
                }

                if (IsThreadsWorking) {
                    Thread.Sleep((int)UtilityParams.Period);
                }                
            }

            Console.WriteLine();

            if (!UtilityParams.IsCopyingStopped) {
                Console.WriteLine("Копирование завершено");
                StopControlThread.Abort();
            } else {
                DeleteCopiedFilesAfterStopCommand(UtilityParams);
                Console.WriteLine("Копирование было отменено, скопированные файлы удалены");
            }
        }

        public static void WatchStopCopyProcess(object InputObject)
        {
            ConsoleKeyInfo CheckReadKey = Console.ReadKey(true);
            Console.WriteLine("Отменяем копирование");
            if (InputObject is ApplicationParamsDto UtilityParams) {
                UtilityParams.IsCopyingStopped = true;
            }   
        }

        public static void CopyFiles(ApplicationParamsDto UtilityParams)
        {
            PrepareFilesToCopyList(UtilityParams);
           
            if (UtilityParams.CopyFilesHeap.Count > 0)
            {
                List<Thread> CopyThreadsPool = new List<Thread>();

                for (int i = 0; i < UtilityParams.CopyThreadsQuantity; i++)
                {
                    CopyThreadsPool.Add(new Thread(new ParameterizedThreadStart(CopyFileAction)));
                }
                
                foreach (Thread ThElement in CopyThreadsPool)
                {
                    ThElement.Start(UtilityParams);
                }

                Thread StopControlThread = new Thread(new ParameterizedThreadStart(WatchStopCopyProcess));
                StopControlThread.Start(UtilityParams);

                ControlCopyProcess(CopyThreadsPool, UtilityParams, StopControlThread);
            }
            else {
                Console.WriteLine("Нет файлов подходящих для копирования");
            }
        }

        private static void PrepareFilesToCopyList(ApplicationParamsDto UtilityParams)
        {
            string[] SourceFilesList = Directory.GetFiles(UtilityParams.SourcePath);
            string[] TargetFilesList = Directory.GetFiles(UtilityParams.TargetPath);

            List<string> SourceFileNamesList = new List<string>();
            List<string> TargetFileNamesList = new List<string>();

            foreach (string file in SourceFilesList)
            {
                SourceFileNamesList.Add(file.Substring(UtilityParams.SourcePath.Length + 1));
            }

            foreach (string file in TargetFilesList)
            {
                TargetFileNamesList.Add(file.Substring(UtilityParams.TargetPath.Length + 1));
            }

            UtilityParams.CopyFilesList = SourceFileNamesList.Except(TargetFileNamesList).ToList();
            UtilityParams.CopyFilesHeap = UtilityParams.CopyFilesList.ToList();
        }

        private static void DeleteCopiedFilesAfterStopCommand(ApplicationParamsDto UtilityParams)
        {
            foreach (string file in UtilityParams.CopyFilesList) {
                if (File.Exists(Path.Combine(UtilityParams.TargetPath, file))) {
                    File.Delete(Path.Combine(UtilityParams.TargetPath, file));
                }
            }
        }
    }
}
