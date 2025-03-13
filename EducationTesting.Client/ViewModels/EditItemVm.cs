using System;
using System.Windows.Input;
using EducationTesting.Client.Helpers;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public abstract class EditItemVm<T> : BindableBase
    {
        protected readonly Action GoBack;
        public T Item { get; set; }
        public Property<string> ErrorMessageProp { get; } = new Property<string>();
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditItemVm(T item, Action goBack)
        {
            GoBack = goBack;
            Item = item;
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(goBack);
        }

        protected abstract void Save();
    }
}