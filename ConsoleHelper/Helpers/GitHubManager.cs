using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper.Helpers
{
    public class GitHubManager
    {
        private readonly IGitHubClient _githubClient;
        public GitHubManager()
        {
            _githubClient = _githubClient ?? new GitHubClient(new ProductHeaderValue("ApplicationName"));
        }
        public async Task<IEnumerable<User>> GetUserListAsync(string gitHubTokens)
        {
            var tokens = gitHubTokens.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var tasks = tokens.Select(GetCurrentUserAsync);

            return await Task.WhenAll(tasks);
        }

        private async Task<User> GetCurrentUserAsync(string token)
        {
            _githubClient.Connection.Credentials = new Credentials(token);

            return await _githubClient.User.Current();
        }
    }
}
