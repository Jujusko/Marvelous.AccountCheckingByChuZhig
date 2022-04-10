﻿using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig.HostProject
{
    public class ConfigAlyona : IConfigAlyona
    {
        //private readonly ILogger<AuthRequestClient> _logger;
        private readonly IConfiguration _urls;

        public ConfigAlyona(IConfiguration config)
        {
            _urls = config;
        }
        public async Task<RestResponse<T>> SendRequest<T>(string url, string path, Microservice service, string jwtToken = "null")
        {
            var request = new RestRequest(path);
            //var client = new RestClient(_urls[service.ToString()]);
            var client = new RestClient(url);
            client.Authenticator = new JwtAuthenticator(jwtToken);
            client.AddDefaultHeader(nameof(Microservice), Microservice.MarvelousAccountChecking.ToString());
            var response = await client.ExecuteAsync<T>(request);
            CheckTransactionError(response, service);
            Console.WriteLine("QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ\n\n\n\n\n\n\n\n" +
                $"{response.Data}\n\n\n\n\n\n\n\n\n\n\n\n\n");

            return response;
        }

        private static void CheckTransactionError<T>(RestResponse<T> response, Microservice service)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
            //else if(response.StatusCode == System.Net.HttpStatusCode.)
            //{

            //}
            //if (response.Data is null)
            //    throw new BadGatewayException($"Content equal's null {response.ErrorException!.Message}");//another ex
        }
    }
}
