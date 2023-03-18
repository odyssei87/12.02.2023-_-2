using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/*app.Run(async (context) =>
{
    if (context.Request.Path == "/old")
    {
        //await context.Response.WriteAsync("Old Page");
        context.Response.Redirect("https://github.com/session");
    }
    else if(context.Request.Path=="/new")
    {
        await context.Response.WriteAsync("New Page");
    }
    else
    {
        await context.Response.WriteAsync("Main Page");
    }
});*/
/*app.Run(async (context) =>
{
    Person ted = new("Ted", 22);
    await context.Response.WriteAsJsonAsync(ted);
});*/

//app.Run(async (context) =>
//{
//    var response = context.Response;
//    var request = context.Request;
//    if (request.Path == "/api/user")
//    {
//        var message = "Не корректные данные";
//        try
//        {
//            var person = await request.ReadFromJsonAsync<Person>();
//            if (person != null)
//                message = $"Name: {person.Name} Age:{person.Age}";
//        }
//        catch { }
//        await response.WriteAsJsonAsync(new { text = message });
//    }
//    else
//    {
//        response.ContentType = "text/html; charset=utf-8";
//        await response.SendFileAsync("html/index.html");
//    }
//});
//app.Run();

//public record Person(string Name, int Age);

app.Run(async (context1) =>
{
var response1 = context1.Response;//If(request.HasJsonContentType())
var request1 = context1.Request;
if (request1.Path == "/api/user")
{
var message1 = "Некорректные данные";
if (request1.HasJsonContentType())
{
//определяем параметры сериализации/десериализации
var jsonoptions1 = new JsonSerializerOptions();
//добавляем конвертер кода json в объект типа Person
jsonoptions1.Converters.Add(new PersonConverter());
//десериализируем данные с помошью конверта PersonConvert
var person1 = await request1.ReadFromJsonAsync<Person>(jsonoptions1);
if (person1 != null)
message1 = $"Name: {person1.Name} Age: {person1.Age} Languech: {person1.Languech}";
}
await response1.WriteAsJsonAsync(new { text = message1 });
}
else
{
response1.ContentType = "text/html; charset=utf-8";
await response1.SendFileAsync("html/index.html");
}
});
app.Run(); public record Person(string Name, int Age,string Languech);
public class PersonConverter : JsonConverter<Person>
{
    public override Person? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var personName1 = "Undefined";
        var personAge1 = 0;
        var personlanguech1 = " ";
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName?.ToLower())
                {
                    //если свойство age и оно содержит число
                    case "age" when reader.TokenType == JsonTokenType.Number:
                        personAge1 = reader.GetInt32();
                        break;
                    //если свойство age и оно содержит строку
                    case "age" when reader.TokenType == JsonTokenType.String:
                        string? stringValue1 = reader.GetString();
                        //пытаемся конвертировать строку в число
                        if (int.TryParse(stringValue1, out int value))
                        {
                            personAge1 = value;
                        }
                        break;
                    case "name"://если свойство Name/name
                        string? name = reader.GetString();
                        if (name != null)
                            personName1 = name;
                        break;
                    case "languaes":
                        string? languech = reader.GetString();
                        if (languech != null)
                            personlanguech1 = languech;
                       
                        break;
                   
                }
            }
        }
        return new Person(personName1, personAge1,personlanguech1);
    }
    public override void Write(Utf8JsonWriter writer, Person value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteString("languech", value.Languech);
        writer.WriteNumber("age", value.Age);
        writer.WriteEndObject();
    }
   
}