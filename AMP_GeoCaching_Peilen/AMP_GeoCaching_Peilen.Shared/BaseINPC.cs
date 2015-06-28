﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AMP_GeoCaching_Peilen
{
    public abstract class BaseINPC : INotifyPropertyChanged  
    {
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged; 

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
