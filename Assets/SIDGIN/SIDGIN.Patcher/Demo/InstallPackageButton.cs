
using SIDGIN.Patcher.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstallPackageButton : MonoBehaviour
{
    [SerializeField]
    string packageName;
    [SerializeField]
    GameObject playButton;
    [SerializeField]
    GameObject installButton;
    [SerializeField]
    GameObject statusObject;
    [SerializeField]
    Image progressBar;
    [SerializeField]
    Text statusText;

    PackageInstallProcess installProcess;

    private void Start()
    {
        if (PackageInstaller.CheckInstalledPackage(packageName))
        {
            playButton.gameObject.SetActive(true);
            statusObject.gameObject.SetActive(false);
            installButton.gameObject.SetActive(false);
        }
        else
        {
            installProcess = PackageInstaller.GetInstallProccess(packageName);
            if(installProcess != null && !installProcess.isDone)
            {
                installProcess.onComplete = OnComplete;

                playButton.gameObject.SetActive(false);
                statusObject.gameObject.SetActive(true);
                installButton.gameObject.SetActive(false);
            }
            else
            {
                playButton.gameObject.SetActive(false);
                statusObject.gameObject.SetActive(false);
                installButton.gameObject.SetActive(true);
            }
        }
    }
    public void StartInstall()
    {
        PackageInstaller.InstallPackage(packageName);
        installProcess = PackageInstaller.GetInstallProccess(packageName);
        installProcess.onComplete = OnComplete;
        playButton.gameObject.SetActive(false);
        statusObject.gameObject.SetActive(true);
        installButton.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(installProcess != null && !installProcess.isDone && installProcess.patcherProgress != null)
        {
            progressBar.fillAmount = installProcess.patcherProgress.progress;
            statusText.text = installProcess.patcherProgress.status;
        }
    }
    void OnComplete()
    {
        if(installProcess.isDone && installProcess.error == null)
        {
            playButton.gameObject.SetActive(true);
            statusObject.gameObject.SetActive(false);
            installButton.gameObject.SetActive(false);
            installProcess = null;
        }
    }
}
