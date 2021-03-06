﻿using MobileFramework.PluginManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FreshMvvm;
using MobileFramework.MonitoringPlugin.Helpers;
using System.Globalization;
using System.Collections.ObjectModel;
using Syncfusion.SfChart.XForms;
using MobileFramework.Helpers;

namespace MobileFramework.MonitoringPlugin.SubPages
{
    /// <summary>
    /// this is the System overview plugin Page viewModel which is connected to the AlarmPluginPage
    /// </summary>
    public class AddBloodSugarPageModel : FreshBasePageModel, INotifyPropertyChanged
    {
        private string name;
        private string test;
        IPluginCollector pluginCollector;
        private BloodSugarDataPoint existingPoint;
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// constructor of the Overview Plugin PageModel
        /// sets up eventhandler for structure refreshed event
        /// </summary>
        /// <param name="model"></param>
        public AddBloodSugarPageModel(IPluginCollector _pluginCollector)
        {
            Name = "AddBloodSugar";
            pluginCollector = _pluginCollector;
        }
        public AddBloodSugarPageModel(IPluginCollector _pluginCollector, BloodSugarDataPoint point)
        {
            Name = "AddBloodSugar";
            pluginCollector = _pluginCollector;
            Time = point.Date.TimeOfDay;
            Date = point.Date;
            BloodSugarValue = point.BloodSugarLevel;
            existingPoint = point;
        }


        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
           
          
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
          
        }

     
        ///</summary>
        /// returns the Name of the Plugin the PageModel is related to;
        /// </summary>
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public void preSetFields()
        {
            if (Time.Days + Time.Hours + Time.Minutes == 0 && Date == new DateTime(1900, 1, 1))
            {
                Time = DateTime.Now.TimeOfDay;
                Date = DateTime.Now;
            }
        }

        public double BloodSugarValue { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public Command AddDataPoint
        {
            get
            {
                //test notification
                return new Command(() =>
                {
                   MonitoringPluginSettingsModel tmpSettingsModel =  (MonitoringPluginSettingsModel) pluginCollector.SettingsModels.Where(x => x.Key == PluginNames.MonitoringPluginName).Select(x => x.Value).FirstOrDefault();
                    
                    BloodSugarDataPoint tmpPoint = new BloodSugarDataPoint();
                    DateTime tmpDateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds);
                    tmpPoint.BloodSugarLevel = BloodSugarValue;
                    tmpPoint.Date = tmpDateTime;

                    if (existingPoint != null)
                    {
                        tmpSettingsModel.BloodSugarDataPoints.Remove(existingPoint);
                    }
                    tmpSettingsModel.BloodSugarDataPoints.Add(tmpPoint);

                    FreshMasterDetailNavigation nav = App.GetNavigationContainer();
                    nav.PopPage(false, true);
                });
            }
        }
        
    }
}
