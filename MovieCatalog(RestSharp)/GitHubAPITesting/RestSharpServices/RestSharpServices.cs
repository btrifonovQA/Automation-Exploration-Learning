using RestSharp;
using RestSharp.Authenticators;
using RestSharpServices.Models;
using System.Text.Json;


namespace RestSharpServices
{
    public class GitHubApiClient
    {
        private readonly RestClient client;

        public GitHubApiClient(string baseUrl, string username, string token)
        {
            RestClientOptions options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(username, token)
            };

            this.client = new RestClient(options);
        }

        public List<Issue>? GetAllIssues(string repo)
        {
            RestRequest request = new RestRequest($"{repo}/issues", Method.Get);
            RestResponse response = client.Execute(request);

            return response.Content != null ? JsonSerializer.Deserialize<List<Issue>>(response.Content) : null;
        }

        public Issue? GetIssueByNumber(string repo, long issueNumber)
        {
            RestRequest request = new RestRequest($"{repo}/issues/{issueNumber}", Method.Get);
            RestResponse response = client.Execute(request);

            return response.Content != null ? JsonSerializer.Deserialize<Issue>(response.Content) : null;
        }

        public List<Label>? GetAllLabelsForIssue(string repo, long issueNumber)
        {
            RestRequest request = new RestRequest($"/{repo}/issues/{issueNumber}/labels", Method.Get);
            RestResponse response = client.Execute(request, Method.Get);

            return response.Content != null ? JsonSerializer.Deserialize<List<Label>>(response.Content) : null;
        }

        public Comment? GetCommentById(string repo, long commentId)
        {
            RestRequest request = new RestRequest($"/{repo}/issues/comments/{commentId}", Method.Get);
            RestResponse response = client.Execute(request, Method.Get);

            return response.Content != null ? JsonSerializer.Deserialize<Comment>(response.Content) : null;
        }

        public List<Comment>? GetAllCommentsForIssue(string repo, int issueNumber)
        {
            RestRequest request = new RestRequest($"/{repo}/issues/{issueNumber}/comments", Method.Get);
            RestResponse response = client.Execute(request, Method.Get);

            return response.Content != null ? JsonSerializer.Deserialize<List<Comment>>(response.Content) : null;
        }

        public Issue? CreateIssue(string repo, string title, string body)
        {
            RestRequest request = new RestRequest($"/{repo}/issues", Method.Post);
            request.AddJsonBody(new { title, body });

            RestResponse response = client.Execute(request, Method.Post);

            return response.Content != null ? JsonSerializer.Deserialize<Issue>(response.Content) : null;
        }

        public Comment? CreateCommentOnGitHubIssue(string repo, long issueNumber, string body)
        {
            RestRequest request = new RestRequest($"/{repo}/issues/{issueNumber}/comments", Method.Get);
            request.AddJsonBody(new { body });

            RestResponse response = client.Execute(request, Method.Post);

            return response.Content != null ? JsonSerializer.Deserialize<Comment>(response.Content) : null;
        }

        public Comment? EditCommentOnGitHubIssue(string repo, long commentId, string newBody)
        {
            RestRequest request = new RestRequest($"/{repo}/issues/comments/{commentId}", Method.Patch);
            request.AddJsonBody(new { body = newBody });

            RestResponse response = client.Execute(request, Method.Patch);

            return response.Content != null ? JsonSerializer.Deserialize<Comment>(response.Content) : null;
        }

        public bool DeleteCommentOnGitHubIssue(string repo, long commentId)
        {
            RestRequest request = new RestRequest($"/{repo}/issues/comments/{commentId}", Method.Delete);
            RestResponse response = client.Execute(request, Method.Delete);

            return response.IsSuccessful;
        }
    }
}
