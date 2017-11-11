// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResourceOwnerClient
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(":::::::::::: ALICE ::::::::::::");
            Console.ForegroundColor = previousColor;

            await FireRequests(disco, "alice@alice.com", "password", "api.read_only");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(":::::::::::: BOB ::::::::::::");
            Console.ForegroundColor = previousColor;

            await FireRequests(disco, "bob@bob.com", "password", "api.full_access");

            Console.ReadKey();
        }

        private static async Task FireRequests(DiscoveryResponse disco, string username, string password, string scope)
        {
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, scope);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            Console.WriteLine("GET");
            Console.WriteLine("\n\n");

            var response = await client.GetAsync("http://localhost:5002/values");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }

            Console.WriteLine("POST");
            Console.WriteLine("\n\n");

            var body = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("value", "OIDC")
            });

            response = await client.PostAsync("http://localhost:5002/values", body);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}