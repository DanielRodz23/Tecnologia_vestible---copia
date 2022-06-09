using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Tecnologia_vestible.Model;
using Tecnologia_vestible.View;
using Xamarin.Forms;

namespace Tecnologia_vestible.ViewModel
{
    public class TecnoViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<DispositivoVestible> Lista { get; set; } =
            new ObservableCollection<DispositivoVestible>();

        public ICommand CambiarVistaCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand MostrarDetallesCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        Nuevo nuevo;
        private void CambiarVista(string v)
        {
            if (v == "Agregar")
            {
                DispositivoVestible = new DispositivoVestible(); //Aqui se guardaran los datos capturados al agregar
                nuevo = new Nuevo() { BindingContext = this };
                Application.Current.MainPage.Navigation.PushAsync(nuevo);
            }
            else if (v == "Ver")
            {
                Application.Current.MainPage.Navigation.PopToRootAsync();
            }
        }
        public TecnoViewModel()
        {
            Deserializar();
            CambiarVistaCommand = new Command<string>(CambiarVista);
            AgregarCommand = new Command(Agregar);
            EliminarCommand = new Command<DispositivoVestible>(Eliminar);
            MostrarDetallesCommand = new Command<DispositivoVestible>(MostrarDetalles);
            EditarCommand = new Command<DispositivoVestible>(Editar);
            GuardarCommand = new Command(Guardar);

        }

        void Serializar()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "lista.json";
            File.WriteAllText(file, JsonConvert.SerializeObject(Lista));
        }

        void Deserializar()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "lista.json";
            if (File.Exists(file))
            {
                Lista = JsonConvert.DeserializeObject<ObservableCollection<DispositivoVestible>>(File.ReadAllText(file));
            }
        }

        private void Guardar(object obj)
        {
            //validar
            Lista[indice] = DispositivoVestible; //Reemplaza el original por el clon
            Serializar();
            App.Current.MainPage.Navigation.PopToRootAsync();
        }

        EditarView editarView;
        int indice;
        private void Editar(DispositivoVestible obj)
        {
            indice = Lista.IndexOf(obj);

            DispositivoVestible = new DispositivoVestible
            {
                Nombre = obj.Nombre,
                Precio = obj.Precio,
                Imagen = obj.Imagen,
                Marca=obj.Marca
            };

            editarView = new EditarView()
            {
                BindingContext = this
            };

            App.Current.MainPage.Navigation.PushAsync(editarView);
        }

        DetallesDispositivo detalles;
        private void MostrarDetalles(DispositivoVestible obj)
        {
            detalles = new DetallesDispositivo()
            {
                BindingContext = obj
            };
            App.Current.MainPage.Navigation.PushAsync(detalles);
        }

        private void Eliminar(DispositivoVestible obj)
        {
            if (obj != null)
            {
                Lista.Remove(obj);
                Serializar();
            }
        }

        private void Agregar()
        {
            if (DispositivoVestible != null)
            {
                Error = "";

                if (string.IsNullOrWhiteSpace(DispositivoVestible.Nombre))
                {
                    Error = "Escriba el nombre";
                }

                if (string.IsNullOrWhiteSpace(DispositivoVestible.Imagen))
                {
                    Error = "Inserte URL de imagen";
                }
                if (string.IsNullOrWhiteSpace(DispositivoVestible.Marca))
                {
                    Error = "Escriba marca de dispositivo";
                }
                if (DispositivoVestible.Precio<=0)
                {
                    Error = "Inserte cantidad mayor a cero";
                }

                if (string.IsNullOrWhiteSpace(Error)) //agregar
                {
                    Lista.Add(DispositivoVestible);


                    CambiarVista("Ver");
                    Serializar();

                }

                Change();
            }
        }
        
        private void Change()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public DispositivoVestible DispositivoVestible { get; set; }
        public string Error { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
