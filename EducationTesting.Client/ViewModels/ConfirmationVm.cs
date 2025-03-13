using System;
using System.Windows.Input;
using Prism.Commands;

namespace EducationTesting.Client.ViewModels
{
    public class ConfirmationVm
    {
        private readonly Action _confirm;
        private readonly Action _goBack;
        public string Text { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand GoBackCommand { get; }

        public ConfirmationVm(string text, Action confirm, Action goBack)
        {
            _confirm = confirm;
            _goBack = goBack;
            Text = text;
            ConfirmCommand = new DelegateCommand(Confirm);
            GoBackCommand = new DelegateCommand(goBack);
        }

        private void Confirm()
        {
            _confirm();
            _goBack();
        }
    }
}