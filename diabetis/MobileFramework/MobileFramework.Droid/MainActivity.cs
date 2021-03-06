﻿using System;
using System.Reflection;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Xamarin.Forms;
using MobileFramework.PluginManager;
using Android.Content;
using MobileFramework.Droid.Services;
using MobileFramework.Helpers.Messages;
using MobileFramework.Model;
using Syncfusion.SfChart.XForms.Droid;
using MobileFramework.Database;
using Acr.UserDialogs;

namespace MobileFramework.Droid
{
    [Activity(Label = "BloodCare", Icon = "@drawable/blood_drop", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {   
        /// <summary>
        /// Entry point to application on android
        /// this function is called on appliction start on andorifd plattforms
        /// her the xamarin forms application will be started
        /// Intent is send by start over notification
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            UserDialogs.Init(this);
            Forms.Init(this, bundle);
            new SfChartRenderer();
            WireUpLongRunningTask();
            var x = this.Intent.GetStringExtra("PluginName");
            BloodCareDatabase.Root= System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            LoadApplication(new App(x));
        }


        LongRunningTask longRunningTaskExample;
        FunctionPassingToNativeBackground FuncInfo;
        void WireUpLongRunningTask()
        {
            MessagingCenter.Subscribe<StartLongRunningTaskMessage, FunctionPassingToNativeBackground>(this, "StartLongRunningTaskMessage", (message, obj) =>
            {
                var intent = new Intent(this, typeof(LongRunningTask));
                FuncInfo = obj;
                
                   FunctionPasserToNativeBackground.FunctionName = FuncInfo.FunctionName;
                   FunctionPasserToNativeBackground.Params = FuncInfo.Params;
                   FunctionPasserToNativeBackground.ClassObject = FuncInfo.ClassObject;
                StartService(intent);
            });
        }
    }
}

