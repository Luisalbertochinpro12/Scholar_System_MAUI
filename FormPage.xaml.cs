using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Scholar_System;

public partial class FormPage : ContentPage
{
    public readonly HttpClient _httpClient = new HttpClient();
    public FormPage()
    {
        InitializeComponent();
    }

    async private void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }


    async private void AddPerson(object sender, EventArgs e)
    {
        Debug.WriteLine("El botón fue presionado.");
        String sex = "h";

        if (male.IsChecked == true)
        {
            sex = "h";
        }
        if (female.IsChecked == true)
        {
            sex = "m";
        }

        var person = new PersonModel
        {
            nombre = userName.Text,
            apellido = lastName.Text,
            sexo = sex,
            fh_nac = dateBth.Date.ToString("yyyy-mm-dd"),
            id_rol = int.TryParse(idRol.Text, out int idRolValue) ? idRolValue : 0
        };
        var json = JsonSerializer.Serialize(person);

        Console.WriteLine(json);

        //Conversión de Json a HTTPS porque si no es imposible usarla
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://fi.jcaguilar.dev/v1/escuela/persona", content);
        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Success", "Se agregó correctamente la persona", "Continuar");
        } else
        {
            Debug.WriteLine(response.StatusCode);
        }
    }
}