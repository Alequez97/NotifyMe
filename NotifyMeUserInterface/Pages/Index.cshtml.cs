using DomainEntites;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;

namespace NotifyMeUserInterface.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRestClient _restClient;

        public List<Group> Groups { get; private set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            var baseUri = new Uri("https://localhost:5001/"); // TODO Replace with configuration reader
            _restClient = new RestClient(baseUri);
#if DEBUG
            _restClient.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        }

        public void OnGet()
        {
            var request = new RestRequest("api/groups", Method.GET);

            var response = _restClient.Execute<List<Group>>(request);

            if (response.IsSuccessful)
            {
                Groups = response.Data;
            }
            else
            {
                Groups = new List<Group>();
                Console.WriteLine(response.ErrorMessage);
            }
        }
    }
}
