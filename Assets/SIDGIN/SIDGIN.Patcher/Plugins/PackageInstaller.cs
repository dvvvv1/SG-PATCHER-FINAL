using SIDGIN.Patcher.Client;
using SIDGIN.Patcher.Common;
using SIDGIN.Patcher.SceneManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace SIDGIN.Patcher.Unity
{
    public class PackageInstallProcess
    {
        public string packageName;
        public bool isDone;
        public PatcherProgress patcherProgress;
        public string errorStatus;
        public System.Exception error;
        public Action onComplete;
    }
    public class PackageInstaller : MonoBehaviour
    {
        List<PackageInstallProcess> packageProcesses = new List<PackageInstallProcess>();
        static PackageInstaller _instance;
        static PackageInstaller instance
        {
            get
            {
                if(_instance == null)
                {
                    var go = new GameObject("SIDGIN Package Installer");
                    _instance = go.AddComponent<PackageInstaller>();
                }
                return _instance;
            }
        }
        static PatcherClient PrepareClient()
        {
            var targetPath = Application.persistentDataPath;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            var patcherClinetSettings = ClientSettings.Get();

            InternalErrorHandler.onErrorHandled += InternalErrorHandler_onErrorHandled;
            var client = new PatcherClient();
        
            return client;
        }

        static void InternalErrorHandler_onErrorHandled(Exception ex)
        {
            Debug.LogError($"SG Patcher Internal Error: {ex + "\n" + ex.InnerException?.Message + "\n" + ex.StackTrace}");
        }

        public static async void InstallPackage(string packageName)
        {
            if (instance.packageProcesses.All(x => x.packageName != packageName))
            {
                var packageProcess = new PackageInstallProcess() { packageName = packageName };

                instance.packageProcesses.Add(packageProcess);
                try
                {
                    var patcherClient = PrepareClient();
                    patcherClient.onProgressChanged += (PatcherProgress patcherProgress) =>
                    {
                        packageProcess.patcherProgress = patcherProgress;
                    };
                    await patcherClient.InstallPackage(packageName);
                    SGSceneManager.Initialize();
                    SGResources.Initialize();
                    packageProcess.isDone = true;
                    if (packageProcess.onComplete != null)
                        packageProcess.onComplete.Invoke();
                }
                catch (System.Exception ex)
                {
                    packageProcess.errorStatus = ex + "\n" + ex.InnerException?.Message + "\n" + ex.StackTrace;
                    packageProcess.error = ex;
                }
                finally
                {
                    packageProcess.isDone = true;
                    if (instance.packageProcesses.Contains(packageProcess))
                    {
                        instance.packageProcesses.Remove(packageProcess);
                    }
                }
            }

        }
        public static PackageInstallProcess GetInstallProccess(string packageName)
        {
            var installProcess = instance.packageProcesses.Find(x => x.packageName == packageName);
            return installProcess;
        }
        public static bool CheckInstalledPackage(string packageName)
        {
            var patcherClient = new PatcherClient();
            var applicationData = patcherClient.GetApplicationData();
            if(applicationData != null)
            {
                return applicationData.packages.Any(x => x.name == packageName);
            }
            else
            {
                return false;
            }
        }
    }
}