using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Models.Classes;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public class ClassesVm : BindableBase
    {
        private readonly IClassesService _service;
        private readonly IMainLayoutNavStore _mainLayoutNavStore;
        private readonly Lazy<ClassesVm> _classesVmLazy;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                RaisePropertyChanged(nameof(Classes));
            }
        }

        public IEnumerable<Class> Classes =>
            _service.GetList().Where(c =>
                string.IsNullOrWhiteSpace(SearchText) || c.Name.ToLower().Contains(SearchText.ToLower()));

        public ICommand AddCommand { get; }
        public ICommand GoToCommand { get; }
        public ICommand DeleteCommand { get; }

        public ClassesVm(IClassesService service, IMainLayoutNavStore mainLayoutNavStore, Lazy<ClassesVm> classesVmLazy)
        {
            _service = service;
            _mainLayoutNavStore = mainLayoutNavStore;
            _classesVmLazy = classesVmLazy;
            AddCommand = new DelegateCommand(Add);
            GoToCommand = new DelegateCommand<string>(GoTo);
            DeleteCommand = new DelegateCommand<Class>(Delete);
        }

        private void Add() =>
    _mainLayoutNavStore.CurrentVmProp.Value = new EditClassVm(
        new Class(),
        _service,
        () =>
        {
            _mainLayoutNavStore.CurrentVmProp.Value = _classesVmLazy.Value;
            RaisePropertyChanged(nameof(Classes));
        });

        private void GoTo(string id)
        {
            var item = _service.Get(id);
            _mainLayoutNavStore.CurrentVmProp.Value = new EditClassVm(
                item,
                _service,
                () =>
                {
                    _mainLayoutNavStore.CurrentVmProp.Value = _classesVmLazy.Value;
                    RaisePropertyChanged(nameof(Classes));
                });
        }

        private void Delete(Class item)
        {
            _mainLayoutNavStore.CurrentVmProp.Value = new ConfirmationVm(
                $"Удалить класс '{item.Name}'?",
                () =>
                {
                    try
                    {
                        _service.Delete(item.Id);

                        // Обновляем список классов
                        RaisePropertyChanged(nameof(Classes));

                        // Показываем сообщение об успехе
                        MessageBox.Show(
                            "Класс успешно удален!",
                            "Успех",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        // Показываем сообщение об ошибке
                        MessageBox.Show(
                            $"Ошибка при удалении: {ex.Message}",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                    finally
                    {
                        // Возвращаемся к исходному ViewModel
                        _mainLayoutNavStore.CurrentVmProp.Value = _classesVmLazy.Value;
                    }
                },
                () => _mainLayoutNavStore.CurrentVmProp.Value = _classesVmLazy.Value
            );
        }
    }
}