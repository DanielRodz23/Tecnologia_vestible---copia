using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnologia_vestible.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tecnologia_vestible.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaPrincipal : ContentPage
    {
        public VistaPrincipal()
        {
            InitializeComponent();
        }
        private async void SwipeItem_Clicked(object sender, EventArgs e)
        {
            var sw = (SwipeItem)sender; //unboxing
            if (await DisplayAlert("Por favor confirme", $"¿Está seguro de eliminar el {((DispositivoVestible)sw.CommandParameter).Nombre}?", "Si", "No") == true)
            {
                vm.EliminarCommand.Execute(sw.CommandParameter);
            }
        }
    }
}