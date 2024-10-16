using System.Net.Http;
using System.Threading.Tasks;
using IntroAsPnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntroAsPnet.Servicios;
public class AllSportsApiService
{
    private readonly HttpClient? _httpClient;
    private readonly string? _apiKey;
    public AllSportsApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = "5ff679d1fc34c0a0422d67b16fc1aabf1773768090903043e545c0acdd6950f3";
    }



    public async Task<List<PlayerModel>> SearchPlayersByFullNameAsync(string firstName, string lastName)
    {
        var playerName = $"{firstName} {lastName}";
        var url = $"https://allsportsapi.com/api/football/?&met=Players&playerName={playerName}&APIkey={_apiKey}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonData = JObject.Parse(jsonResponse);

            // Extraer la lista de jugadores desde la respuesta
            var players = jsonData["result"]?.Select(player => new PlayerModel
            {
                Nombre = (string)player["player_name"],
                Posicion = (string)player["player_type"],
                Edad = (int)player["player_age"],
                Equipo = (string)player["team_name"],
                Pais = (string)player["player_country"],
                Goles = (int)player["player_goals"],
                Asistencias = (int)player["player_assists"],
                Amarillas = (int)player["player_yellow_cards"],
                Rojas = (int)player["player_red_card"]
            }).ToList();

            return players ?? new List<PlayerModel>();
        }
        else
        {
            throw new HttpRequestException($"Error al buscar jugadores: {response.ReasonPhrase}");
        }
    }



}