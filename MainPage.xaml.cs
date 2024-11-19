using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace Scholar_System
{
    public partial class MainPage : ContentPage
    {
        public readonly HttpClient _httpClient = new HttpClient();

        private ObservableCollection<PersonModel> _personas;

        public ObservableCollection<PersonModel> Personas
        {
            get => _personas;
            set
            {
                _personas = value;
                OnPropertyChanged();
            }
        }

        public async void GetData()
        {
            //Defino la ruta a donde estamos consultando la información (La API)
            var response = await _httpClient.GetStringAsync("https://fi.jcaguilar.dev/v1/escuela/persona");

            var personas = JsonSerializer.Deserialize<ObservableCollection<PersonModel>>(response);

            Personas = personas;
        }

        public MainPage()
        {
            InitializeComponent();
            GetData();
            BindingContext = this;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage());
        }

        private async void Delete_Person(object sender, EventArgs e)
        {

            var button = sender as Button;
            var idPersona = button?.CommandParameter.ToString();

            bool answer = await DisplayAlert("Caution👀", "¿Estás seguro de eliminar esta persona?", "Sí", "No");

            if (answer)
            {
        
                var data = new { id_persona = idPersona };
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Debug.WriteLine(json);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    Content = content,
                    RequestUri = new Uri("https://fi.jcaguilar.dev/v1/escuela/persona")
                };

                Debug.WriteLine(request);

                var response = await _httpClient.SendAsync(request);
                var errorContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(response);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Actualización", "Se eliminó la persona", "Salir");
                }
                else
                {
                    await DisplayAlert("Error", $"Hubo un error del tipo {response.StatusCode}", "Salir");
                    Debug.WriteLine(response.RequestMessage);
                    Debug.WriteLine(errorContent);
                }
            }
            else
            {
                Debug.WriteLine("No se decidió borrar");
            }
        }







        private async void Edit_Person(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Editar nombre", "");
            string lstName = await DisplayPromptAsync("Editar apellido", "");

            var button = sender as Button;  // Asegúrate de que el sender sea un botón
            var idPersona = button?.CommandParameter?.ToString();

            var data = new { 
                         id_persona = idPersona,   
                         nombre = name,
                         apellido = lstName};
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                Content = content,
                RequestUri = new Uri("https://fi.jcaguilar.dev/v1/escuela/persona")

            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Succes", "Se actualizó correctamente la persona", "Cerrar");
            }
            else
            {
                await DisplayAlert("Error", "Hubo un error al actualizar a la persona", "Cerrar");
            }
        }
    }
}
