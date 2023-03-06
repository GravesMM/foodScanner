using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;

namespace scannerFood.ViewModels
{
    public class SelectionViewModel : INotifyPropertyChanged
    {
        public bool checkSelected;
        public string checkboxName;

        public bool eggSelected;
        public bool fishSelected;
        public bool molluscanSelected;
        public bool peanutSelected;
        public bool cowSelected;
        public bool soySelected;
        public bool treenutSelected;
        public bool wheatSelected;
        public bool crustaceanSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        public SelectionViewModel()
        {

        }

        public bool EggSelector
        {
            set
            {
                if (eggSelected != value)
                {
                    eggSelected = value;
                    OnPropertyChanged("EggSelector");
                    Preferences.Set("EggCheckbox", eggSelected);
                }

            }
            get { return eggSelected; }
        }
        public bool CowSelector
        {
            set
            {
                if (cowSelected != value)
                {
                    cowSelected = value;
                    OnPropertyChanged("CowSelector");
                    Preferences.Set("CowCheckbox", cowSelected);
                }

            }
            get { return cowSelected; }
        }
        public bool SoySelector
        {
            set
            {
                if (soySelected != value)
                {
                    soySelected = value;
                    OnPropertyChanged("SoyCheckbox");
                    Preferences.Set("SoyCheckbox", soySelected);
                }

            }
            get { return soySelected; }
        }
        public bool WheatSelector
        {
            set
            {
                if (wheatSelected != value)
                {
                    wheatSelected = value;
                    OnPropertyChanged("WheatCheckbox");
                    Preferences.Set("WheatCheckbox", wheatSelected);
                }

            }
            get { return wheatSelected; }
        }
        public bool MolluscanSelector
        {
            set
            {
                if (molluscanSelected != value)
                {
                    molluscanSelected = value;
                    OnPropertyChanged("MolluscanCheckbox");
                    Preferences.Set("MolluscanCheckbox", molluscanSelected);
                }

            }
            get { return molluscanSelected; }
        }
        public bool CrustaceanSelector
        {
            set
            {
                if (crustaceanSelected != value)
                {
                    crustaceanSelected = value;
                    OnPropertyChanged("CrustaceanCheckbox");
                    Preferences.Set("CrustaceanCheckbox", crustaceanSelected);
                }

            }
            get { return crustaceanSelected; }
        }
        public bool PeanutSelector
        {
            set
            {
                if (peanutSelected != value)
                {
                    peanutSelected = value;
                    OnPropertyChanged("PeanutCheckbox");
                    Preferences.Set("PeanutCheckbox", peanutSelected);
                }

            }
            get { return peanutSelected; }
        }
        public bool FishSelector
        {
            set
            {
                if (fishSelected != value)
                {
                    fishSelected = value;
                    OnPropertyChanged("FishCheckbox");
                    Preferences.Set("FishCheckbox", fishSelected);
                }

            }
            get { return fishSelected; }
        }
        public bool TreenutSelector
        {
            set
            {
                if (treenutSelected != value)
                {
                    treenutSelected = value;
                    OnPropertyChanged("TreenutCheckbox");
                    Preferences.Set("TreenutCheckbox", treenutSelected);
                }

            }
            get { return treenutSelected; }
        }
        //public bool SelectedCheckbox
        //{
        //    set
        //    {
        //        if (checkSelected != value)
        //        {
        //            checkSelected = value;
        //            OnPropertyChanged(checkboxName);
        //            Preferences.Set(checkboxName, checkSelected);
        //        }
        //    }
        //    get { return checkSelected; }
        //}


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //checkboxName = propertyName;
        }
    }
}
