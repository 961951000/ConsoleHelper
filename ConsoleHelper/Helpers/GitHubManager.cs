using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace ConsoleHelper.Helpers
{
    public class GitHubManager
    {
        public readonly IGitHubClient _githubClient;

        public GitHubManager()
        {
            _githubClient = _githubClient ?? InitializeGithubClient();
        }

        public static IGitHubClient InitializeGithubClient()
        {
            var tokens = ConfigurationManager.AppSettings["GitHubTokens"].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var githubClient = new GitHubClient(new ProductHeaderValue("ApplicationName"))
            {
                Credentials = new Credentials(tokens.FirstOrDefault())
            };

            return githubClient;
        }

        public async Task<IEnumerable<User>> GetUserListAsync(string gitHubTokens)
        {
            var tokens = gitHubTokens.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var tasks = tokens.Select(GetCurrentUserAsync);

            return await Task.WhenAll(tasks);
        }

        public async Task<int> GetPullRequestNumberFromShaAsync(string owner, string name, string sha)
        {
            var requiredPullRequest = await GetPullRequest(owner, name, sha).ConfigureAwait(false);
            return requiredPullRequest.Number;
        }

        public async Task<IEnumerable<PullRequest>> GetAllOpenPullRequestsAsync(string owner, string name)
        {
            return await _githubClient.PullRequest.GetAllForRepository(owner, name);
        }

        private async Task<string> GetFileContent(string owner, string name, PullRequestFile file)
        {
            var blob = await _githubClient.Git.Blob.Get(owner, name, file.Sha);
            var fileData = Convert.FromBase64String(blob.Content);
            return Encoding.UTF8.GetString(fileData);
        }
        private async Task<PullRequest> GetPullRequest(string owner, string name, string sha)
        {
            var pullRequests = await GetAllOpenPullRequestsAsync(owner, name);
            var requiredPullRequest = pullRequests.FirstOrDefault(x => x.Head?.Sha != null && x.Head.Sha.Equals(sha));

            return requiredPullRequest;
        }

        private async Task<User> GetCurrentUserAsync(string token)
        {
            _githubClient.Connection.Credentials = new Credentials(token);

            return await _githubClient.User.Current();
        }
    }
}
