﻿using Microsoft.AspNetCore.Mvc;
using Pokedex.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace Pokedex.Controllers
{
    public class PokemonController : Controller
    {
        public async Task<IActionResult> Index(int? pagina = 0)
        {
            const string apiUrl = "http://pokeapi.co/api/v2/pokemon";
            int limit = 20;
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{apiUrl}?limit={limit}&offset={limit * pagina}");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PokemonApiResponse>(content);

            int? i = limit * pagina;

            var pokemons = new List<Pokemon>();
            foreach(var p in result.Results)
            {
                i++; 

                Pokemon pokemon = new();
                pokemon.Name = p.Name;
                pokemon.Url = $"https://assets.pokemon.com/assets/cms2/img/pokedex/detail/{((i < 1000) ? i?.ToString("#000") : i)}.png";

                pokemons.Add(pokemon);
            }

            return View(pokemons);
        }

        public async Task<IActionResult> Details(int id)
        {
            const string apiUrl = "http://pokeapi.co/api/v2/pokemon";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{apiUrl}/{id}");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PokemonDetails>(content);

            return View(result);

        }
    }
}
